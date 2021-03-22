using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vegit_backend_api.Models;

namespace vegit_backend_api.Services.Abstract
{
    public interface IUserService : IService<User>
    {
        Task<(string, bool, User)> Login(Login login);
    }
}
