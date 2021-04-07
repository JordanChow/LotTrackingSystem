#region Copyright
//
// ©PEER Intellectual Property Inc., 2021
// 
// This software contains confidential and trade secret information belonging to
// PEER Intellectual Property Inc. All rights reserved. 
//
// No part of this software may be reproduced or transmitted in any form 
// or by any means, electronic, mechanical, photocopying, recording or 
// otherwise, without the prior written consent of PEER Intellectual Property Inc.
//
#endregion
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LotRepository : ILotRepository
    {
        private readonly DataContext m_context;
        public LotRepository(DataContext context)
        {
            m_context = context;
        }

        /// <summary>
        /// Create a lot and save to database
        /// </summary>
        /// <param name="lot">created lot</param>
        /// <returns>lot</returns>
        public async Task<ActionResult<Lot>> CreateLotAsync(Lot lot)
        {
            // Add lot to database
            m_context.Lots.Add(lot);

            await m_context.SaveChangesAsync();

            return lot;
        }  

        /// <summary>
        /// Move lot, update plant location and state
        /// </summary>
        /// <param name="lot">target lot</param>
        /// <returns>true if moved successfully</returns>
        public async Task<bool> MoveLotAsync(Lot lot)
        {
            // Find current lot
            var currLot = await m_context.Lots.FirstOrDefaultAsync
                (x => x.PlantId == lot.PlantId);
            
            // Increment plant id and update state
            currLot.PlantId++;
            currLot.State = States.process;

            // Save to database
            if(await m_context.SaveChangesAsync() > 0) return true;

            return false;
        }

        /// <summary>
        /// Delete lot from database, used for last plant after processing is complete
        /// </summary>
        /// <param name="lot">lto to be deleted</param>
        /// <returns>true if deleted</returns>
        public async Task<bool> DeleteLotAsync(Lot lot)
        {
            // Find lot in database
            var removeLot = await m_context.Lots.FirstOrDefaultAsync
                (x => x.PlantId == lot.PlantId);

            // Remove lot
            m_context.Lots.Remove(removeLot);

            // Save to database
            if(await m_context.SaveChangesAsync() > 0) return true;

            return false;
        }

        /// <summary>
        /// Check if next plant is full from database
        /// </summary>
        /// <param name="plantNumber">plant Id</param>
        /// <returns>true if next is full</returns>
        public async Task<bool> IsNextFullAsync(int plantNumber)
        {
            // Check if there exists a lot with a proceeding plant Id
            return await m_context.Lots.AnyAsync(x => x.PlantId == (plantNumber + 1));
        }

        /// <summary>
        /// Check if lot exists in database
        /// </summary>
        /// <param name="currentLot">lot id</param>
        /// <returns>true if lot exists in db</returns>
        public async Task<bool> LotExistsAsync (int lotId)
        {
            // Check database
            return await m_context.Lots.AnyAsync(x => x.UserLot == lotId);
        }

        /// <summary>
        /// Helper class for states
        /// </summary>
        public static class States
        {
            public const string move = "MOVE";
            public const string complete = "COMPLETE";
            public const string process = "PROCESS";
        }
    }
}