using System.Collections.Generic;
using System.Threading.Tasks;
using vegit_backend_api.Interfaces;
using vegit_backend_api.Models;
using vegit_backend_api.Services.Abstract;

namespace vegit_backend_api.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserService _userService;
        public UserRepository(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<List<User>> GetAll()
        {
            var data = await _userService.GetAll();
            if (data != null)
            {
                return data;
            }
            return null;
        }

        public async Task<User> GetById(int id)
        {
            var data = await _userService.GetById(id);
            if (data != null)
            {
                return data;
            }
            return null;
        }

        public async Task<(string, bool, User)> Add(User model)
        {
            return await _userService.Add(model);
        }

        public async Task<bool> Delete(int id)
        {
            var data = await GetById(id);
            if (data != null)
            {
                var result = await _userService.Delete(id);
                return result;
            }
            return false;
        }

        public async Task<bool> Update(int id, User model)
        {
            var data = await GetById(id);
            if (data != null)
            {
                var result = await _userService.Update(id, model);
                return result;
            }
            return false;
        }

        public async Task<(string, bool, User)> Login(Login login)
        {
            return await _userService.Login(login);
        }
    }
}
