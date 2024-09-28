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
    public class UsersRepository: GenericRepository<User>, IUsersRepository
    {
        protected AppDbContext _context;
        protected ILogger _logger;
        internal DbSet<User> _dbSet;
        public UsersRepository(AppDbContext context, ILogger logger)
            : base(context, logger) 
        {
            _context = context;
            _logger = logger;
            _dbSet = _context.Set<User>();

        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                return await _dbSet.Where(x => x.Status == 1)
                                   .AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Error occured in this repo");
                return new List<User>();
            }
        }

        public async Task<bool> UpdateUserProfile(User user)
        {
            try
            {
                var existingUser = await _dbSet.Where(x => x.Status == 1 &&
                                                    x.Id == user.Id).
                                                    FirstOrDefaultAsync();
                if (existingUser == null) 
                {
                    return false;
                }

                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Country = user.Country;
                existingUser.Address = user.Address;
                existingUser.Sex = user.Sex;
                existingUser.Phone = user.Phone;
                existingUser.Datebirth = user.Datebirth;
                existingUser.MobileNumber = user.MobileNumber;
                existingUser.UpdatedDate = DateTime.UtcNow;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} UpdateUserProfile Error occured in this repo");
                return false;
            }
        }

        public async Task<User> GetUserByIdentityId(Guid identityId)
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
