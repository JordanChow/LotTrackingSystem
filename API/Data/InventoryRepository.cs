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
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DataContext m_context;
        public InventoryRepository(DataContext context)
        {
            m_context = context;
        }

        /// <summary>
        /// Dequeue a lot
        /// </summary>
        /// <param name="lotNumber">lot Id</param>
        /// <returns>true if lot dequeued</returns>
        public async Task<bool> DequeueLotAsync(int lotNumber)
        {
            // Find lot in database based on lot Id
            var lotInDB = await m_context.Lots.FirstOrDefaultAsync
                (x => x.UserLot == lotNumber);

            // Increment plant Id if the lot exists
            if (lotInDB != null)
                lotInDB.PlantId++;
            else
                return false;

            // Return if a change was made successfully
            return (await m_context.SaveChangesAsync() > 0);
        }
    }
}