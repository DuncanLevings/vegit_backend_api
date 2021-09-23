using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using vegit_backend_api.Models.Food;
using vegit_backend_api.Services.Abstract;
using vegit_backend_api.Services.Helpers;
using System.Text;

namespace vegit_backend_api.Services
{
    public class IngredientService : IIngredientService
    {
        public async Task<List<IngredientModel>> GetAll()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    var ingredients = await connection.QueryAsync<IngredientModel>($"SELECT F.ID, F.NAME, F.DIET_TYPE, D.NAME AS DIET_NAME, D.LEVEL AS DIET_LEVEL, " +
                        $"F.DESCRIPTION, F.[GROUP], f.SUB_GROUP FROM FOODS F INNER JOIN DIET_TYPES D ON(F.DIET_TYPE = D.ID)");

                    if (ingredients != null && ingredients.Count() > 0)
                    {
                        return ingredients.ToList();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IngredientModel> GetById(int id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    var ingredient = await connection.QueryAsync<IngredientModel>($"SELECT F.ID, F.NAME, F.DIET_TYPE, D.NAME AS DIET_NAME, D.LEVEL AS DIET_LEVEL, " +
                        $"F.DESCRIPTION, F.[GROUP], F.SUB_GROUP, F.DATA_SOURCE FROM FOODS F INNER JOIN DIET_TYPES D ON(F.DIET_TYPE = D.ID) WHERE F.ID = {id}");


                    if (ingredient != null && ingredient.Count() > 0)
                    {
                        return ingredient.FirstOrDefault();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<IngredientModel>> GetByDataSource(int dataSource)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    var ingredients = await connection.QueryAsync<IngredientModel>($"SELECT F.ID, F.NAME, F.DIET_TYPE, D.NAME AS DIET_NAME, D.LEVEL AS DIET_LEVEL, " +
                        $"F.DESCRIPTION, F.[GROUP], F.SUB_GROUP, F.DATA_SOURCE FROM FOODS F INNER JOIN DIET_TYPES D ON(F.DIET_TYPE = D.ID) WHERE F.DATA_SOURCE = {dataSource}");


                    if (ingredients != null && ingredients.Count() > 0)
                    {
                        return ingredients.ToList();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<SearchSingle>> GetNames()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    var ingredients = await connection.QueryAsync<SearchSingle>($"SELECT NAME FROM FOODS");

                    if (ingredients != null && ingredients.Count() > 0)
                    {
                        return ingredients.ToList();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<(string, bool, IngredientModel model)> Add(IngredientModel model)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();
                    
                    model.created_date = DateTime.Now.ToString();
                    model.updated_date = DateTime.Now.ToString();
                    model.data_source = 4; // user submitted data source

                    var param = this.SetParameters(model);
                    await connection.QueryAsync<IngredientModel>("spAddFood",
                                param,
                                commandType: CommandType.StoredProcedure);

                    IngredientModel ingredient = null;
                    if (param.Get<Int32>("foodID") > -1)
                    {
                        ingredient = await GetById(param.Get<Int32>("foodID"));
                    }

                    return (param.Get<String>("responseMessage"), param.Get<Boolean>("successVal"), ingredient);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (ex.Message, false, null);
            }
        }

        // only allowing deletion of user submitted ingredients for now
        public async Task<bool> Delete(int id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    await connection.QueryAsync($"DELETE FOODS WHERE DATA_SOURCE = 4 AND Id = {id}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        // only allowing updating of user submitted ingredients for now
        public async Task<bool> Update(int id, IngredientModel model)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    model.updated_date = DateTime.Now.ToString();

                    var query = @"UPDATE FOODS SET name = @name, diet_type = @diet_type, description = @description, updated_date = @updated_date WHERE DATA_SOURCE = 4 AND Id = " + id;
                    await connection.ExecuteAsync(query, model);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<IngredientModel> SearchIngredient(SearchSingle single)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    single.name = removePlural(single.name);

                    // start search at data source 4 and decrementing data source until result is found
                    // exact word search
                    for (int i = 4; i > 0; i--)
                    {
                        var ingredient = await connection.QueryAsync<IngredientModel>($"SELECT TOP 1 F.ID, F.NAME, F.DIET_TYPE, D.NAME AS DIET_NAME, D.LEVEL AS DIET_LEVEL, " +
                        $"F.DESCRIPTION, F.[GROUP], F.SUB_GROUP, F.DATA_SOURCE FROM FOODS F INNER JOIN DIET_TYPES D ON(F.DIET_TYPE = D.ID) WHERE DATA_SOURCE = {i} AND F.NAME LIKE '{single.name}' OR F.NAME LIKE '%{single.name}%'");

                        if (ingredient != null && ingredient.Count() > 0)
                        {
                            return ingredient.FirstOrDefault();
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<IngredientModel>> SearchIngredientList(List<SearchSingle> searchList)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    if (searchList.Count <= 0) return null;

                    var param = this.SetParametersList(searchList);
                    var ingredients = await connection.QueryAsync<IngredientModel>("spSearchList",
                                param,
                                commandType: CommandType.StoredProcedure);

                    if (ingredients != null && ingredients.Count() > 0)
                    {
                        return computeBestMatch(searchList, ingredients.ToList());
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private DynamicParameters SetParameters(IngredientModel ingredient)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pname", ingredient.name);
            parameters.Add("@pdiet_type", ingredient.diet_type);
            parameters.Add("@pdescription", ingredient.description);
            parameters.Add("@ppublic_id", ingredient.public_Id);
            parameters.Add("@ppublic_id_int", ingredient.public_Id_Int);
            parameters.Add("@pgroup", ingredient.group);
            parameters.Add("@psub_group", ingredient.sub_group);
            parameters.Add("@pcreated_date", ingredient.created_date);
            parameters.Add("@pupdated_date", ingredient.updated_date);
            parameters.Add("@pdata_source", ingredient.data_source);

            parameters.Add("@responseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
            parameters.Add("@successVal", dbType: DbType.Boolean, direction: ParameterDirection.Output);
            parameters.Add("@foodID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            return parameters;
        }

        private DynamicParameters SetParametersList(List<SearchSingle> searchList)
        {
            DynamicParameters parameters = new DynamicParameters();
            StringBuilder sb = new StringBuilder();

            String[] delimiterChars = { " ", "-" };
            foreach (SearchSingle item in searchList)
            {
                var name = item.name.Trim();
                
                if (name.IndexOf(" ") != -1 || name.IndexOf("-") != -1)
                {
                    string[] names = name.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                    foreach (String n in names)
                    {
                        String _n = removePlural(n);

                        sb.AppendFormat("\"{0}\" OR ", _n.Trim());
                    }
                }

                String _name = removePlural(name);
                sb.AppendFormat("\"{0}\" OR ", _name);
            }
            
            sb.Length -= 4; // removes last OR

            parameters.Add("@searchList", sb.ToString());

            return parameters;
        }

        private string removePlural(string name)
        {
            if (name.EndsWith("ies"))
            {
                name = name.Substring(0, name.Length - 3);
            }
            else if (name.EndsWith("s"))
            {
                name = name.Substring(0, name.Length - 1);
            }
            return name;
        }

        private List<IngredientModel> computeBestMatch(List<SearchSingle> searchList, List<IngredientModel> initialResults)
        {
            List<IngredientModel> bestResults = new List<IngredientModel>();
            IngredientModel bestMatch = null;
            int bestScore = 9999999;
            foreach (SearchSingle item in searchList)
            {
                foreach (IngredientModel ingredient in initialResults)
                {
                    string name = ingredient.name.Replace("-", "").ToLower();
                    string target = item.name.Replace("-", "").ToLower();
                    int score = Levenshtein.ComputeDistance(name, target);
                    if (score <= bestScore)
                    {
                        bestScore = score;
                        bestMatch = ingredient;
                    }

                    // best possible match, exit out here
                    if (score == 0)
                    {
                        bestMatch = ingredient;
                        break;
                    }
                }

                if (bestScore < 3 && bestMatch != null) 
                {
                    bestResults.Add(bestMatch);
                } else
                {
                    bestResults.Add(createUnknownIngredient(item.name));
                }
                bestScore = 9999999;
                bestMatch = null;
            }
            return bestResults;
        }

        private IngredientModel createUnknownIngredient(String name)
        {
            return new IngredientModel
            {
                name = name,
                diet_type = 5,
                diet_name = "Unspecified",
                diet_level = 1
            };
        }
    }
}
