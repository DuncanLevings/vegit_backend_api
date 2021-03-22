using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vegit_backend_api.Interfaces;
using vegit_backend_api.Models;

namespace vegit_backend_api.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private const string ERROR_MESSAGE = "There is no data related with ID: {0}";

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("/users/getall")]
        public async Task<ActionResult> GetAll()
        {
            var userList = await _userRepository.GetAll();
            return StatusCode(200, userList);
        }

        [HttpGet("/users/get/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE, id) });

            return StatusCode(200, user);
        }

        [HttpPost("/users/add")]
        public async Task<ActionResult> Add([FromBody] User user)
        {
            var result = await _userRepository.Add(user);

            if (!result.Item2)
                return StatusCode(500, new ErrorModel { ErrorMessage = result.Item1 });

            return StatusCode(201, result.Item3);
        }

        [HttpDelete("/users/delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var isDeleted = await _userRepository.Delete(id);
            if (!isDeleted)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE, id) });

            return StatusCode(204);
        }

        [HttpPut("/users/update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] User user)
        {
            var isUpdated = await _userRepository.Update(id, user);
            if (!isUpdated)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE, id) });

            return StatusCode(204);
        }

        [HttpPost("/users/login")]
        public async Task<ActionResult> Login([FromBody] Login login)
        {
            var result = await _userRepository.Login(login);

            if (!result.Item2)
                return StatusCode(401, new ErrorModel { ErrorMessage = result.Item1 });

            return StatusCode(200, result.Item3);
        }
    }
}
