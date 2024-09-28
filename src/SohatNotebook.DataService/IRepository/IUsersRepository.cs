using SohatNotebook.Entities.DbSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.DataService.IRepository
{
    public interface IUsersRepository: IGenericRepository<User>
    {
        public Task<bool> UpdateUserProfile(User user);

        public Task<User> GetUserByIdentityId(Guid identityId);
    }
}
