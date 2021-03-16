using System;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using vegit_backend_api.Models;
using vegit_backend_api.Services.Abstract;
using vegit_backend_api.Services.Helpers;
using System.Linq;

namespace vegit_backend_api.Services
{
    public class UserService : IUserService
    {
        public async Task<List<User>> GetAll()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    var users = await connection.QueryAsync<User>($"SELECT ID, EMAIL, FIRSTNAME, LASTNAME FROM USERS");

                    if (users != null && users.Count() > 0)
                    {
                        return users.ToList();
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

        public async Task<User> GetById(int id)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    var user = await connection.QueryAsync<User>($"SELECT ID, EMAIL, FIRSTNAME, LASTNAME FROM USERS WHERE Id = {id}");


                    if (user != null && user.Count() > 0)
                    {
                        return user.FirstOrDefault();
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
        public async Task<(string, bool)> Add(User model)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    var param = this.SetParameters(model);
                    await connection.QueryAsync<User>("spAddUser",
                                param,
                                commandType: CommandType.StoredProcedure);

                    return (param.Get<String>("responseMessage"), param.Get<Boolean>("successVal"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (ex.Message, false);
            }
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(int id, User model)
        {
            throw new NotImplementedException();
        }

        public async Task<(string, bool)> Login(Login login)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(SqlHelper.connectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    var param = this.SetLoginParameters(login);
                    await connection.QueryAsync<User>("spLogin",
                                param,
                                commandType: CommandType.StoredProcedure);

                    return (param.Get<String>("responseMessage"), param.Get<Boolean>("successVal"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return (ex.Message, false);
            }
        }

        private DynamicParameters SetParameters(User user)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pemail", user.email);
            parameters.Add("@ppassword", user.password);
            parameters.Add("@pfirstName", user.firstName);
            parameters.Add("@plastName", user.lastName);

            parameters.Add("@responseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
            parameters.Add("@successVal", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            return parameters;
        }

        private DynamicParameters SetLoginParameters(Login login)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@pemail", login.email);
            parameters.Add("@ppassword", login.password);

            parameters.Add("@responseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
            parameters.Add("@successVal", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            return parameters;
        }
    }
}
