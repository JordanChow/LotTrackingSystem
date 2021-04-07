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
using API.DTOs;
using API.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.Text;

namespace UnitTest
{
    /// <summary>
    /// Test account functionality
    /// </summary>
    [TestClass]
    public class AccountTest
    {
        /// <summary>
        /// Register new user
        /// </summary>
        [TestMethod]
        public void RegisterTest()
        {
            var user = new RegisterDto
            {
                Username = "user",
                Password = "password"
            };

            // Check RegisterDto username and password
            Assert.AreEqual("user", user.Username);
            Assert.AreEqual("password", user.Password);

            using var hmac = new HMACSHA512();

            var appUser = new AppUser
            {
                UserName = user.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password)),
                PasswordSalt = hmac.Key
            };

            // Check AppUser username
            Assert.AreEqual("user", appUser.UserName);

            // Check PasswordHash does not equal PasswordSalt
            Assert.IsFalse((appUser.PasswordHash).Equals(appUser.PasswordSalt));
        }

        /// <summary>
        /// Ensure user can login
        /// </summary>
        [TestMethod]
        public void LoginTest()
        {
            var user = new LoginDto
            {
                Username = "user",
                Password = "password"
            };

            // Check RegisterDto username and password
            Assert.AreEqual("user", user.Username);
            Assert.AreEqual("password", user.Password);
        }
    }
}