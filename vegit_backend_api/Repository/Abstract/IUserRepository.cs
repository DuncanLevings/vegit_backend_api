using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vegit_backend_api.Models;
using vegit_backend_api.Repository.Abstract;

namespace vegit_backend_api.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<(string, bool)> Login(Login login);
    }
}
