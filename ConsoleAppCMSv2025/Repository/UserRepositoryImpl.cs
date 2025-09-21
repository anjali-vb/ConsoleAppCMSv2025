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
        public async Task<User> AuthenticateUserByRoleIdAsync(string username, string password)
        {
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

                    //output parameters - all fields from stored procedure
                    SqlParameter roleIdParameter = new SqlParameter("@RoleID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                    };
                    command.Parameters.Add(roleIdParameter);

                    SqlParameter fullNameParameter = new SqlParameter("@FullName", SqlDbType.NVarChar, 100)
                    {
                        Direction = ParameterDirection.Output,
                    };
                    command.Parameters.Add(fullNameParameter);

                    SqlParameter genderParameter = new SqlParameter("@Gender", SqlDbType.NVarChar, 10)
                    {
                        Direction = ParameterDirection.Output,
                    };
                    command.Parameters.Add(genderParameter);

                    SqlParameter joiningDateParameter = new SqlParameter("@JoiningDate", SqlDbType.Date)
                    {
                        Direction = ParameterDirection.Output,
                    };
                    command.Parameters.Add(joiningDateParameter);

                    SqlParameter mobileNumberParameter = new SqlParameter("@MobileNumber", SqlDbType.NVarChar, 15)
                    {
                        Direction = ParameterDirection.Output,
                    };
                    command.Parameters.Add(mobileNumberParameter);

                    SqlParameter userId = new SqlParameter("@UserId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                    };
                    command.Parameters.Add(userId);

                    // Execute the stored procedure
                    await command.ExecuteNonQueryAsync();

                    // Check if login was successful (RoleID should not be null)
                    if (roleIdParameter.Value != DBNull.Value)
                    {
                        // Create User object and populate with all retrieved data
                        User user = new User();

                        // Set RoleId (assuming User class has RoleId property, not UserId for role)
                        user.RoleId = Convert.ToInt32(roleIdParameter.Value);
                        user.UserId = Convert.ToInt32(userId.Value);
                        // Set other properties from output parameters
                        user.UserName = username;
                        user.Password = password; // Note: Consider not storing password in User object for security
                        user.FullName = fullNameParameter.Value != DBNull.Value ? fullNameParameter.Value.ToString() : string.Empty;
                        user.Gender = genderParameter.Value != DBNull.Value ? genderParameter.Value.ToString() : string.Empty;
                        user.JoiningDate = joiningDateParameter.Value != DBNull.Value ? Convert.ToDateTime(joiningDateParameter.Value) : DateTime.MinValue;
                        user.MobileNumber = mobileNumberParameter.Value != DBNull.Value ? mobileNumberParameter.Value.ToString() : string.Empty;
                        user.IsActive = true;
                        return user;
                    }
                    else
                    {
                        // Login failed - return null or throw exception
                        return null;
                    }
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