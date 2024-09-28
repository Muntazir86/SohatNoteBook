using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.Authentication.Configuration
{
    public class JwtConfig
    {
        public string Secret { get; set; }

        public TimeSpan ExpireTimeFrame { get; set; }

    }
}
