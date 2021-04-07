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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class InventoryController : BaseApiController
    {
        private static HashSet<Lot> inventory = new HashSet<Lot>();
        private readonly ILotRepository m_lotRepository;
        private readonly IInventoryRepository m_inventoryRepository;

        public InventoryController(ILotRepository lotRepository, IInventoryRepository inventoryRepository)
        {
            m_inventoryRepository = inventoryRepository;

            m_lotRepository = lotRepository;
        }

        /// <summary>
        /// Add a lot to the queue
        /// </summary>
        /// <param name="lot"></param>
        /// <returns>Boolean if adding a lot was successful</returns>
        [HttpPut("add")]
        public bool EnqueueLot(Lot lot)
        {
            if(lot != null)
            {
                // Insert lot at end of list
                inventory.Add(lot);

                return true;
            }
            return false;
        }

        /// <summary>
        /// Starting a lot from queue
        /// </summary>
        /// <returns>Started lot</returns>
        [HttpGet("start")]
        public async Task<ActionResult<Lot>> DequeueLot()
        {
            // Check if plant 1 is full
            if (await m_lotRepository.IsNextFullAsync(0))
                return BadRequest("Next plant is full");

            // Get first lot in the queue
            Lot first = inventory.ElementAt(0);

            if(first == null) return BadRequest("Unable to dequeue lot");

            // Dequeue first lot
            var success = await m_inventoryRepository.DequeueLotAsync(first.UserLot);

            if(Convert.ToBoolean(success))
            {
                // Remove lot from queue and set plant location to plant 1
                inventory.Remove(first);
                first.PlantId = 1;
            }
            else
                return BadRequest("Unable to dequeue lot");

            return first;
        }

        /// <summary>
        /// Convert list to array to allow usage in Angular
        /// </summary>
        /// <returns>lot array</returns>
        [HttpGet("lotdata")]
        public Lot[] QueueToArray()
        {
            Lot[] inventoryArray = inventory.ToArray();

            return inventoryArray;
        }

        /// <summary>
        /// Delete contents of the queue
        /// </summary>
        /// <returns>empty lot array</returns>
        [HttpDelete("delete")]
        public Lot[] DeleteQueue()
        {
            inventory.Clear();
            
            return inventory.ToArray();
        }
    }
}