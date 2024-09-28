using SohatNotebook.Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.DataService.IRepository
{
    public interface IHealthDataRepository: IGenericRepository<HealthData>
    {
    }
}
