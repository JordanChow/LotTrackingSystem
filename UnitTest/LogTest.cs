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
using API.Controllers;
using API.DTOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    /// <summary>
    /// Test message log functionality
    /// </summary>
    [TestClass]
    public class LogTest
    {
        public static List<string> log = new List<string>();

        LogController logController = new LogController();

        // Fake message
        MessageDto messageDto = new MessageDto
        {
            Message = "hello",
            Date = "1/1/2021"
        };

        /// <summary>
        /// Test messages are created succcessfully in the dto
        /// </summary>
        [TestMethod]
        public void MessageDtoTest()
        {
            Assert.AreEqual("hello", messageDto.Message);
            Assert.AreEqual("1/1/2021", messageDto.Date);
        }

        /// <summary>
        /// Test update log method, inserting a new message into the log
        /// </summary>
        [TestMethod]
        public void LogInsertTest()
        {
            // Check if messageDto inserted into log
            Assert.IsTrue(logController.UpdateLog(messageDto));
        }

        /// <summary>
        /// Convert log to array (from list)
        /// </summary>
        [TestMethod]
        public void LogToArrayTest()
        {
            var arr = logController.DisplayLog();

            // Check if queue is now of type string[]
            Assert.AreEqual(typeof(string[]), arr.GetType());

        }

        /// <summary>
        /// Test clearing the log of all contents, used on reset
        /// </summary>
        [TestMethod]
        public void LotClearTest()
        {
            var log = logController.ClearLog();

            // Check if log is now empty
            Assert.AreEqual(0, log.Length);
        }
    }
}
