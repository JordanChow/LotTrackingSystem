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
using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data
{
    public class DataContext : DbContext
    { 
        public DataContext (DbContextOptions<DataContext> options) : base(options) {}
        public DataContext() { }
        public virtual DbSet<AppUser> Users {get; set;}
        public virtual DbSet<Lot> Lots { get; set; }
    }
}