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
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LotController : BaseApiController
    {
        private readonly ILotRepository m_lotRepository;
        private readonly IPlantRepository m_plantRepository;
        public LotController(ILotRepository lotRepository, IPlantRepository plantRepository)
        {
            m_plantRepository = plantRepository;
            m_lotRepository = lotRepository;
        }

        /// <summary>
        /// Create a new lot
        /// </summary>
        /// <param name="lot">Lot that is to be created</param>
        /// <returns>Lot</returns>
        [HttpPost("create")]
        public async Task<ActionResult<Lot>> CreateLot(Lot lot)
        {
            // Check if lot exists in database
            if (await LotExists(lot.UserLot))
                return BadRequest("Lot already exists");

            // Generate new lot
            var newLot = new Lot
            {
                UserLot = lot.UserLot,
                Supplier = lot.Supplier,
                WaferAmount = lot.WaferAmount,
                PlantId = 0,
                State = States.process
            };

            // Return new lot
            return await m_lotRepository.CreateLotAsync(newLot);
        }

        /// <summary>
        /// Process lot, wait 1s per wafer
        /// </summary>
        /// <param name="lot">lot the processing is conducted on</param>
        /// <returns>true when complete</returns>
        [HttpPut("process")]
        public async Task<bool> ProcessLot(Lot lot)
        {
            await Task.Delay(lot.WaferAmount * 1000);
            if(lot.PlantId != 10)
                lot.State = States.move;
            else
                lot.State = States.complete;
            
            // Updates lot state in database
            await m_plantRepository.UpdateLotStateAsync(lot);

            return true;
        }

        /// <summary>
        /// Move lot
        /// </summary>
        /// <param name="lot">lot to be moved</param>
        /// <returns>true if move is successful</returns>
        [HttpPut("move")]
        public async Task<ActionResult<bool>> MoveLot(Lot lot)
        {
            if (lot == null) return BadRequest("Unable to move lot");

            // Check if next plant is full
            if (await IsNextFull(lot.PlantId))
                return BadRequest("Next plant is full");

            return await m_lotRepository.MoveLotAsync(lot);
        }

        /// <summary>
        /// Delete lot from database in plant 10
        /// </summary>
        /// <param name="lot">lot at plant 10</param>
        /// <returns>true if deleted</returns>
        [HttpPut("delete")]
        public async Task<bool> DeleteLot(Lot lot)
        {
            return await m_lotRepository.DeleteLotAsync(lot);
        }

        /// <summary>
        /// Helper function - checks if next lot is full
        /// </summary>
        /// <param name="currentPlant">current plant the lot is at</param>
        /// <returns>true if next is full</returns>
        private async Task<bool> IsNextFull(int currentPlant)
        {
            return await m_lotRepository.IsNextFullAsync(currentPlant);
        }

        /// <summary>
        /// Helper function - checks if lot exists in database
        /// </summary>
        /// <param name="currentLot">current lot id</param>
        /// <returns>true if lot exists</returns>
        private async Task<bool> LotExists(int currentLot)
        {
            return await m_lotRepository.LotExistsAsync(currentLot);
        }

        /// <summary>
        /// Helper class - defining states for lot
        /// </summary>
        public static class States
        {
            public const string move = "MOVE";
            public const string complete = "COMPLETE";
            public const string process = "PROCESS";
        } 
    }
}