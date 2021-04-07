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
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    /// <summary>
    /// Lot entity for keeping track of lots as it moves through the system
    /// </summary>
    public class Lot
    {
        public int Id { get; set; }

        [Required]
        public int UserLot { get; set; }

        [Required]
        public string Supplier { get; set; }
        
        [Required]
        public int WaferAmount { get; set; } 
        public int PlantId { get; set; }   
        public string State { get; set; }
    }
}