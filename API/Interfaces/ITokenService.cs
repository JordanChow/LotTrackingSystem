#region Copyright
//
// ?PEER Intellectual Property Inc., 2021
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

namespace API.Interfaces
{
    /// <summary>
    /// Token service for new users
    /// </summary>
    public interface ITokenService
    {
        // Create token for users
        string CreateToken(AppUser user);
    }
}