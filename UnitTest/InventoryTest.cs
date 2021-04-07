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
using API.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    /// <summary>
    /// Test inventory functionality
    /// </summary>
    [TestClass]
    public class InventoryTest
    {
        public static List<Lot> queue = new List<Lot>();

        InventoryController inventory = new InventoryController(null, null);

        // Generate fake lot
        Lot firstLot = new Lot
        {
            Id = 1,
            Supplier = "test",
            UserLot = 1,
            WaferAmount = 1
        };

        /// <summary>
        /// Add a lot to the queue
        /// </summary>
        [TestMethod]
        public void InventoryAddTest()
        {
            // Added to inventory successfully
            Assert.IsTrue(inventory.EnqueueLot(firstLot));
        }

        /// <summary>
        /// Convert inventory to array (from hashset)
        /// </summary>
        [TestMethod]
        public void InventoryToArrayTest()
        {
            var arr = inventory.QueueToArray();

            // Check if queue is now of type Lot[]
            Assert.AreEqual(typeof(Lot[]), arr.GetType());
        }

        /// <summary>
        /// Delete a lot from the queue
        /// </summary>
        [TestMethod]
        public void InventoryDeleteTest()
        {
            var emptyArr = inventory.DeleteQueue();

            // Check queue is now empty
            Assert.AreEqual(0, emptyArr.Length);
        }
    }
}
