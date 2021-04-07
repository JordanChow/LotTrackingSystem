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
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PlantController : BaseApiController
    {
        private readonly IPlantRepository m_plantRepository;
        public PlantController(IPlantRepository plantRepository)
        {
            m_plantRepository = plantRepository;
        }

        /// <summary>
        /// Get lot data based on a plantId
        /// </summary>
        /// <param name="plantId">target plantId of the lot</param>
        /// <returns>Lot at the plantId</returns>
        [HttpGet("{plantId}")]
        public async Task<ActionResult<Lot>> GetLotData(int plantId)
        {
            return await m_plantRepository.GetLotDataAsync(plantId);
        }

        /// <summary>
        /// Update lots state
        /// </summary>
        /// <param name="lot">target lot</param>
        [HttpPost("updateLot")]
        public async Task UpdateLotState(Lot lot)
        {
            await m_plantRepository.UpdateLotStateAsync(lot);
        }

        /// <summary>
        /// Deletes the plants array
        /// </summary>
        /// <returns></returns>
        [HttpDelete("clear")]
        public async Task ClearPlants()
        {
            await m_plantRepository.ClearPlantsAsync();
        }
    }
}