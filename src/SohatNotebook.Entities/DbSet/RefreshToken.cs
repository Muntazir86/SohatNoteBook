using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.Entities.DbSet
{
    public class RefreshToken: BaseEntity
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime Expiry { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevocked { get; set; }

        [ForeignKey("UserId")]
        IdentityUser User { get; set; }
    }
}
