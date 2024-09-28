using SohatNotebook.Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.DataService.IRepository
{
    public interface IRefreshTokenRepository: IGenericRepository<RefreshToken>
    {
        public Task<RefreshToken> GetByRefreshToken(string refreshToken);
    }
}
