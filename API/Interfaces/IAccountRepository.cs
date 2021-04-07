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
    /// Account interface for signing in and registering users
    /// </summary>
    public interface IAccountRepository
    {
        // Register user
        Task<UserDto> RegisterAsync(AppUser user);

        // Login user
        Task<AppUser> LoginAsync(LoginDto loginDto);

        // Check is user exists
        Task<bool> UserExistsAsync(string username);

        // Register new user, convert AppUser to UserDto
        UserDto NewUser(AppUser user);
    }
}