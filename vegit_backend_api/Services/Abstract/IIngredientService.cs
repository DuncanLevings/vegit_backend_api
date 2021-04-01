using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vegit_backend_api.Models.Food;

namespace vegit_backend_api.Services.Abstract
{
    public interface IIngredientService : IService<IngredientModel>
    {
        Task<List<IngredientModel>> GetByDataSource(int dataSource);
        Task<List<SearchSingle>> GetNames();
        Task<IngredientModel> SearchIngredient(SearchSingle single);
        Task<List<IngredientModel>> SearchIngredientList(List<SearchSingle> searchList);
    }
}
