using SohatNotebook.DataService.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.DataService.IConfiguration
{
    public interface IUnitOfWork
    {
        IUsersRepository Users { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IHealthDataRepository HealthData { get; }

        Task CompleteAsync();
    }
}
