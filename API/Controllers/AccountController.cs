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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAccountRepository m_accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            m_accountRepository = accountRepository;
        }

        /// <summary>
        /// Register a new user, checks username does not already exist
        /// Creates password hash and salt and saves in db
        /// </summary>
        /// <param name="registerDto">Takes in reisterDto which contains username and password</param>
        /// <returns>New registered userDto</returns>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            // Check for existing username
            if (await UserExists(registerDto.Username)) 
                return BadRequest("Username already exists");

            using var hmac = new HMACSHA512();

            // Create new user properties
            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            // Save user in database
            return await m_accountRepository.RegisterAsync(user);
        }

        /// <summary>
        /// Login a user using username and password
        /// Checks user against database
        /// </summary>
        /// <param name="loginDto">loginDto contains username and password</param>
        /// <returns>UserDto</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            // Retrieve user from database
            var user = await m_accountRepository.LoginAsync(loginDto);

            // User not found
            if (user == null) return Unauthorized("Invalid username");

            // Decrypt password
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            // Check passwords match
            for (int i = 0; i < computedHash.Length; i++)
            {
                //Check if invalid password
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            // Return user
            return m_accountRepository.NewUser(user);
        }

        /// <summary>
        ///  Helper function - checks if user exist in database
        /// </summary>
        /// <param name="username">Takes in a username to search the database</param>
        /// <returns>True - user exists, False - user does not exist</returns>
        private async Task<bool> UserExists(string username)
        {
            return await m_accountRepository.UserExistsAsync(username);
        }
    }
}