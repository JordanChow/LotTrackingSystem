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
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    /// <summary>
    /// Plant interface, retrieving information at specific plants
    /// </summary>
    public interface IPlantRepository
    {
        // Get lot data at a specific plant
        Task<ActionResult<Lot>> GetLotDataAsync(int plantId);

        // Update the state of a lot at a plant (processing, complete, process, etc.)
        Task UpdateLotStateAsync (Lot lot);

        // Clear plant data, used for reset
        Task ClearPlantsAsync();
    }
}