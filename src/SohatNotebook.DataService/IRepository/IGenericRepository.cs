using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SohatNotebook.DataService.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetById(Guid id);

        public Task<bool> Add(T entity);
        public Task<bool> Upsert(T entity);

        public Task<bool> Delete(Guid id);
    }
}
