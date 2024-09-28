using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.Authentication.Models.Generic
{
    public class TokenData
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
