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
    [Route("vegit/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private const string ERROR_MESSAGE = "There is no data related with ID: {0}";

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // vegit/users/getall
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var userList = await _userRepository.GetAll();
            return StatusCode(200, userList);
        }

        // vegit/users/get/id
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE, id) });

            return StatusCode(200, user);
        }

        // vegit/users/add
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] User user)
        {
            var result = await _userRepository.Add(user);

            if (!result.Item2)
                return StatusCode(500, new ErrorModel { ErrorMessage = result.Item1 });

            return StatusCode(201);
        }

        // vegit/users/login
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] Login login)
        {
            var result = await _userRepository.Login(login);

            if (!result.Item2)
                return StatusCode(401, new ErrorModel { ErrorMessage = result.Item1 });

            return StatusCode(200);
        }
    }
}
