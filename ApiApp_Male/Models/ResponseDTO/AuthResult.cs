using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApp_Male.Models.ResponseDTO
{
    public class AuthResult
    {

        public Boolean Success { get; set; }

        public String UserId { get; set; }
        public String UserName { get; set; }
        public String  Token { get; set; }

        public String ErrorCode { get; set; }
    }
}
