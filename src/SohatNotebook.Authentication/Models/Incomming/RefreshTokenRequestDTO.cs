using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.Authentication.Models.Incomming
{
    public class RefreshTokenRequestDTO
    {
        [Required]
        public string Token { get; set; }
        
        [Required]
        public string RefreshToken { get; set; }
    }
}
