using ClassLibraryDatabaseConnection;
using ConsoleAppCMSv2025.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Repository
{
    public class UserRepositoryImpl : IUserRepository

    {
        string winConnString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;


        #region LOGIN - Authenticate user by roleid
        public async Task<int> AuthenticateUserByRoleIdAsync(string username, string password)
        {
            //declare variable for roleid
            int roleId = 0;
            try
            {
                //sqlconnection -- connectionstring
                using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    SqlCommand command = new SqlCommand("sp_UserLogin", conn);
                    //commandType
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    //input parameters
                    command.Parameters.AddWithValue("@UserName", username);
                    command.Parameters.AddWithValue("@Password", password);
                    //output parameters
                    SqlParameter roleIdParameter = new SqlParameter("RoleId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                    };

                    command.Parameters.Add(roleIdParameter);
                    await command.ExecuteNonQueryAsync();
                    if (roleIdParameter.Value != DBNull.Value)                 //check if the output parameter is DBNull before conversion

                    {
                        roleId = Convert.ToInt32(roleIdParameter.Value);
                    }
                    return roleId;
                }

            }
            catch (SqlException es)
            {
                Console.WriteLine("An SqlException error Occurred :" + es.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An Error Occurred :" + ex.Message);
                throw;
            }
        }

        #endregion
    }
}