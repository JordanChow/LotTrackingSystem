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
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext m_context;
        private readonly ITokenService m_tokenService;
        public AccountRepository(DataContext context, ITokenService tokenService)
        {
            m_tokenService = tokenService;
            m_context = context;
        }

        /// <summary>
        ///  Register an AppUser
        /// </summary>
        /// <param name="user">target user</param>
        /// <returns>userDto of registered user</returns>
        public async Task<UserDto> RegisterAsync(AppUser user)
        {
            // Add to database
            m_context.Users.Add(user);

            await m_context.SaveChangesAsync();

            // Return user as UserDto
            return new UserDto
            {
                Username = user.UserName,
                Token = m_tokenService.CreateToken(user)
            };
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="loginDto">user's loginDto</param>
        /// <returns>AppUser of logged in user</returns>
        public async Task<AppUser> LoginAsync(LoginDto loginDto)
        {
            // Find method in database
            var user = await m_context.Users
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

            return user;
        }

        /// <summary>
        /// Check is user exists in database
        /// </summary>
        /// <param name="username">target username</param>
        /// <returns>true if user exists</returns>
        public async Task<bool> UserExistsAsync(string username)
        {
            // Query database
            var user = await m_context.Users.FirstOrDefaultAsync(x => x.UserName == username.ToLower());

            if (user != null)
                return true;
            return false;
        }

        /// <summary>
        /// Convert from AppUser to UserDto
        /// </summary>
        /// <param name="user">AppUser of user</param>
        /// <returns>UserDto</returns>
        public UserDto NewUser(AppUser user)
        {
            return new UserDto
            {
                Username = user.UserName,
                Token = m_tokenService.CreateToken(user)
            };
        }
    }
}