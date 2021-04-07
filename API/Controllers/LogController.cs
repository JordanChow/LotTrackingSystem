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
using System.Collections.Generic;
using System.Linq;
using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LogController : BaseApiController
    {
        private static List<string> log = new List<string>();

        /// <summary>
        /// Update log with new message
        /// </summary>
        /// <param name="input"></param>
        /// <returns>boolean if update was successful</returns>
        [HttpPut("update")]
        public bool UpdateLog(MessageDto input)
        {
            // Display message
            var displayText = $"{input.Message}, {input.Date}";

            // Insert message at start of list (so message is displayed at the top of the log)
            log.Insert(0, displayText);

            if(log.ElementAt(0) != null)
                return true;
            return false;
        }

        /// <summary>
        /// Display log, convert to array so it can be used in Angular
        /// </summary>
        /// <returns>string array (log)</returns>
        [HttpGet("display")]
        public string[] DisplayLog()
        {
            return log.ToArray();
        }

        /// <summary>
        /// Clear contents of log
        /// </summary>
        /// <returns>string array (log)</returns>
        [HttpDelete("clear")]
        public string[] ClearLog()
        {
            log.Clear();
            
            return log.ToArray();
        }
    }
}