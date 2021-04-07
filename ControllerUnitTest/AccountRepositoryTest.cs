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
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;

namespace ControllerUnitTest
{
    public class AccountRepositoryTest
    {
        private readonly AccountRepository m_sut;
        private readonly Mock<DataContext> m_dataContextMock = new Mock<DataContext>();
        private readonly Mock<ITokenService> m_tokenServiceMock = new Mock<ITokenService>();

        /// <summary>
        /// Testing against Account Repository
        /// </summary>
        public AccountRepositoryTest()
        {
            using var hmac = new HMACSHA512();

            // Fake app users
            var userData = new List<AppUser>
            {
                new AppUser
                {
                    UserName = "user-1",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password")),
                    PasswordSalt = hmac.Key
                },
                new AppUser
                {
                    UserName = "user-2",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password")),
                    PasswordSalt = hmac.Key
                }
            }.AsQueryable();

            // Create mock db set
            var mockUserSet = CreateMockDBSet(userData);

            m_dataContextMock.Setup(c => c.Users).Returns(mockUserSet.Object);

            m_sut = new AccountRepository(m_dataContextMock.Object, m_tokenServiceMock.Object);
        }

        /// <summary>
        /// Convert list of app users to mock db set
        /// </summary>
        /// <typeparam name="T">takes in type of app user</typeparam>
        /// <param name="data">list of app users</param>
        /// <returns>mock app user db set</returns>
        private Mock<DbSet<T>> CreateMockDBSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }
        
        /// <summary>
        /// Ensure user is returned when registered
        /// </summary>
        [Fact]
        public async Task RegisterAsync_ShouldReturnUser_WhenUserRegistered()
        {
            using var hmac = new HMACSHA512();

            var appUser = new AppUser
            {
                UserName = "user",
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password")),
                PasswordSalt = hmac.Key
            };

            // Register new user
            var user = await m_sut.RegisterAsync(appUser);

            // Check UserDto is same as AppUser
            Assert.Equal(appUser.UserName, user.Username);
        }
    }
}
