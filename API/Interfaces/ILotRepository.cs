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
using Microsoft.AspNetCore.Mvc;

namespace API.Interfaces
{
    /// <summary>
    /// Lot Interface responsible for all lot actions in the system
    /// </summary>
    public interface ILotRepository
    {
        // Create a lot
        Task<ActionResult<Lot>> CreateLotAsync (Lot lot);

        // Move lot from plant to plant
        Task<bool> MoveLotAsync (Lot lot);

        // Delete lot, used for last plant
        Task<bool> DeleteLotAsync (Lot lot);

        // Check if next plant is full
        Task<bool> IsNextFullAsync (int plantNumber);

        // Verify if a lot exists in database to prevent duplicates
        Task<bool> LotExistsAsync (int lot);
    }
}