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

        public async Task<(string, bool)> Add(User model)
        {
            return await _userService.Add(model);
        }

        public Task<bool> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(int id, User model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<(string, bool)> Login(Login login)
        {
            return await _userService.Login(login);
        }
    }
}
