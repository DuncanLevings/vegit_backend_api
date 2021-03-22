using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vegit_backend_api.Repository.Abstract
{
    public interface IRepository<T> where T:class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<(string, bool, T model)> Add(T model);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, T model);
    }
}
