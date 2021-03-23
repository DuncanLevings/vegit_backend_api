using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vegit_backend_api.Models.Food;
using vegit_backend_api.Repository.Abstract;
using vegit_backend_api.Services.Abstract;

namespace vegit_backend_api.Repository
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly IIngredientService _ingredientService;
        public IngredientRepository(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        public async Task<List<IngredientModel>> GetAll()
        {
            var data = await _ingredientService.GetAll();
            if (data != null)
            {
                return data;
            }
            return null;
        }

        public async Task<IngredientModel> GetById(int id)
        {
            var data = await _ingredientService.GetById(id);
            if (data != null)
            {
                return data;
            }
            return null;
        }

        public async Task<List<IngredientModel>> GetByDataSource(int dataSource)
        {
            var data = await _ingredientService.GetByDataSource(dataSource);
            if (data != null)
            {
                return data;
            }
            return null;
        }

        public async Task<(string, bool, IngredientModel model)> Add(IngredientModel model)
        {
            return await _ingredientService.Add(model);
        }

        public async Task<bool> Delete(int id)
        {
            var data = await GetById(id);
            if (data != null)
            {
                var result = await _ingredientService.Delete(id);
                return result;
            }
            return false;
        }

        public async Task<bool> Update(int id, IngredientModel model)
        {
            var data = await GetById(id);
            if (data != null)
            {
                var result = await _ingredientService.Update(id, model);
                return result;
            }
            return false;
        }

        public async Task<IngredientModel> SearchIngredient(SearchSingle single)
        {
            var data = await _ingredientService.SearchIngredient(single);
            if (data != null)
            {
                return data;
            }
            return null;
        }

        public async Task<List<IngredientModel>> SearchIngredientList(List<SearchSingle> searchList)
        {
            var data = await _ingredientService.SearchIngredientList(searchList);
            if (data != null)
            {
                return data;
            }
            return null;
        }
    }
}
