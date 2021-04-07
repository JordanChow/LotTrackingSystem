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
namespace API.DTOs
{
    /// <summary>
    /// Used once a user has logged in
    /// </summary>
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}