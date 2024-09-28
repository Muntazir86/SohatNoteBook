using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.Entities.DbSet
{
    public class HealthData: BaseEntity
    {
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Race { get; set; }
        public string BloodType { get; set; }
        public bool UseGlasses { get; set; }
    }
}
