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
    public class HealthDataRepository : GenericRepository<HealthData>, IHealthDataRepository
    {
        protected AppDbContext _context;
        protected ILogger _logger;
        internal DbSet<HealthData> _dbSet;
        public HealthDataRepository(AppDbContext context, ILogger logger)
            : base(context, logger)
        {
            _context = context;
            _logger = logger;
            _dbSet = _context.Set<HealthData>();

        }

        public override async Task<IEnumerable<HealthData>> GetAll()
        {
            try
            {
                return await _dbSet.Where(x => x.Status == 1)
                                   .AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Error occured in this repo");
                return new List<HealthData>();
            }
        }

        public async Task<bool> UpdateUserProfile(HealthData healthData)
        {
            try
            {
                var existingData = await _dbSet.Where(x => x.Id == healthData.Id).
                                                    FirstOrDefaultAsync();
                if (existingData == null)
                {
                    return false;
                }

                existingData.Height = healthData.Height;
                existingData.Weight = healthData.Weight;
                existingData.Race = healthData.Race;
                existingData.BloodType = healthData.BloodType;
                existingData.UseGlasses = healthData.UseGlasses;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} UpdateUserProfile Error occured in this repo");
                return false;
            }
        }

        public async Task<HealthData> GetUserByIdentityId(Guid identityId)
        {
            try
            {
                var user = await _dbSet.Where(x => x.Status == 1 &&
                                                    x.Id == identityId).
                                                    AsNoTracking().
                                                    FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} GetUserByIdentityId Error occured in this repo");
                return null;
            }
        }
    }
}
