#region Copyright
//
// �PEER Intellectual Property Inc., 2021
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
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PlantRepository : IPlantRepository
    {
        private readonly DataContext m_context;
        public PlantRepository(DataContext context)
        {
            m_context = context;
        }

        /// <summary>
        ///  Get lot data of a certain lot
        /// </summary>
        /// <param name="plantId">plant Id</param>
        /// <returns>lot</returns>
        public async Task<ActionResult<Lot>> GetLotDataAsync(int plantId)
        {
            // Returns lot from database
            var lot = await m_context.Lots.FirstOrDefaultAsync(x => x.PlantId == plantId);

            if(lot != null)
                return lot;
            
            // Dummy lot to help initialize plants
            else{
                var fakeLot = new Lot {
                    PlantId = -1,
                    UserLot = -1,
                    Supplier = "",
                    WaferAmount = -1
                };

                return fakeLot;
            }
        }

        /// <summary>
        /// Update lot state in database
        /// </summary>
        /// <param name="lot">target lot</param>
        public async Task UpdateLotStateAsync(Lot lot)
        {
            var updateLot = await m_context.Lots.FirstOrDefaultAsync(x => x.PlantId == lot.PlantId);
            updateLot.State = lot.State;
            await m_context.SaveChangesAsync();
        }

        /// <summary>
        /// Delete Lot dataset from database, used for reset
        /// </summary>
        public async Task ClearPlantsAsync()
        {
            Lot[] lots = await m_context.Lots.ToArrayAsync();
            m_context.RemoveRange(lots);
            await m_context.SaveChangesAsync();
        }
    }
}