using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vegit_backend_api.Models;
using vegit_backend_api.Models.Food;
using vegit_backend_api.Repository.Abstract;

namespace vegit_backend_api.Controllers
{
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientRepository _ingredientRepository;
        private const string ERROR_MESSAGE = "There is no data related with ID: {0}";
        private const string ERROR_MESSAGE_SOURCE = "There is no data related with Data Source: {0}";
        private const string ERROR_MESSAGE_SEARCH_SINGLE = "There is no data related with name: {0}";

        public IngredientController(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        [HttpGet("/ingredients/getall")]
        public async Task<ActionResult> GetAll()
        {
            var ingredientList = await _ingredientRepository.GetAll();
            return StatusCode(200, ingredientList);
        }

        [HttpGet("/ingredients/get/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var ingredient = await _ingredientRepository.GetById(id);
            if (ingredient == null)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE, id) });

            return StatusCode(200, ingredient);
        }

        [HttpGet("/ingredients/get/dataSource/{source}")]
        public async Task<ActionResult> GetByDataSource(int source)
        {
            var ingredientList = await _ingredientRepository.GetByDataSource(source);
            if (ingredientList == null)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE_SOURCE, source) });

            return StatusCode(200, ingredientList);
        }

        [HttpPost("/ingredients/add")]
        public async Task<ActionResult> Add([FromBody] IngredientModel ingredient)
        {
            var result = await _ingredientRepository.Add(ingredient);

            if (!result.Item2)
                return StatusCode(500, new ErrorModel { ErrorMessage = result.Item1 });

            return StatusCode(201, result.Item3);
        }

        [HttpDelete("/ingredients/delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var isDeleted = await _ingredientRepository.Delete(id);
            if (!isDeleted)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE, id) });

            return StatusCode(204);
        }

        [HttpPut("/ingredients/update/{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] IngredientModel ingredient)
        {
            var isUpdated = await _ingredientRepository.Update(id, ingredient);
            if (!isUpdated)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE, id) });

            return StatusCode(204);
        }

        [HttpPost("/ingredients/search/single")]
        public async Task<ActionResult> SearchIngredient([FromBody] SearchSingle single)
        {
            var ingredient = await _ingredientRepository.SearchIngredient(single);
            if (ingredient == null)
                return StatusCode(403, new ErrorModel { ErrorMessage = String.Format(ERROR_MESSAGE_SEARCH_SINGLE, single.name) });

            return StatusCode(200, ingredient);
        }

        [HttpPost("/ingredients/search/list")]
        public async Task<ActionResult> SearchIngredientList([FromBody] List<SearchSingle> searchList)
        {
            var ingredientList = await _ingredientRepository.SearchIngredientList(searchList);
            return StatusCode(200, ingredientList);
        }
    }
}
