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
using API.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    /// <summary>
    /// Test the lot data type
    /// </summary>
    [TestClass]
    public class LotTest
    {
        /// <summary>
        /// Test the properties of a created lot
        /// </summary>
        [TestMethod]
        public void CreateLotTest()
        {
            var newLot = new Lot
            {
                UserLot = 1,
                Supplier = "supplier",
                WaferAmount = 1,
                PlantId = 0
            };

            // Check Lot values
            Assert.AreEqual(1, newLot.UserLot);
            Assert.AreEqual("supplier", newLot.Supplier);
            Assert.AreEqual(1, newLot.WaferAmount);
            Assert.AreEqual(0, newLot.PlantId);
        }
    }
}
