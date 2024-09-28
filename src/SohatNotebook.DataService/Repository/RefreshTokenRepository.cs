using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SohatNotebook.DataService.Data;
using SohatNotebook.DataService.IRepository;
using SohatNotebook.Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.DataService.Repository
{
    internal class RefreshTokenRepository: GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        protected AppDbContext _context;
        protected ILogger _logger;
        internal DbSet<RefreshToken> _dbSet;
        public RefreshTokenRepository(AppDbContext context, ILogger logger)
            : base(context, logger)
        {
            _context = context;
            _logger = logger;
            _dbSet = _context.Set<RefreshToken>();

        }

        public override async Task<IEnumerable<RefreshToken>> GetAll()
        {
            try
            {
                return await _dbSet.Where(x => x.Status == 1)
                                   .AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetAll have an error while executing request", "RefreshToken");
                return new List<RefreshToken>();
            }
        }

        public async Task<RefreshToken> GetByRefreshToken(string refreshToken)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(x => x.Token == refreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetByRefreshToken have an error while executing request", "RefreshToken");
                return null;
            }
        }
    }
}
