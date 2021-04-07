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
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Interfaces
{
    /// <summary>
    /// Interface for the queue of lots
    /// </summary>
    public interface IInventoryRepository
    {
        // Remove lot from queue and start it in the system
        Task<bool> DequeueLotAsync(int lotNum);
    }
}