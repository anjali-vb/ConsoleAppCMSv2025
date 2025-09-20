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
//for registering patient
{
    public class PatientRepositoryImpl : IPatientRepository
    {
        string winConnString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        public async Task<int> RegisterPatientAsync(Patient patient)
        {
            try
            {
                using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
                {
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    SqlCommand command = new SqlCommand("sp_RegisterPatient", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PatientName", patient.PatientName);
                    command.Parameters.AddWithValue("@DateOfBirth", patient.DateOfBirth);
                    command.Parameters.AddWithValue("@Gender", patient.Gender);
                    command.Parameters.AddWithValue("@BloodGroup", patient.BloodGroup);
                    command.Parameters.AddWithValue("@MobileNumber", patient.MobileNumber);
                    command.Parameters.AddWithValue("@Address", patient.Address);
                    command.Parameters.AddWithValue("@MembershipId", (object)patient.MembershipId ?? DBNull.Value);

                    SqlParameter outputId = new SqlParameter("@PatientId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputId);

                    await command.ExecuteNonQueryAsync();

                    return (int)outputId.Value; // return PatientId
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
        public async Task<Patient> GetPatientByMMRAsync(string mmrNo)
        {
            Patient patient = null;

            using (SqlConnection conn = new SqlConnection(winConnString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_GetPatientByMMR", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MMRNo", mmrNo);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            patient = new Patient
                            {
                                PatientId = (int)reader["PatientId"],
                                PatientName = reader["PatientName"].ToString(),
                                DateOfBirth = (DateTime)reader["DateOfBirth"],
                                Gender = reader["Gender"].ToString(),
                                BloodGroup = reader["BloodGroup"].ToString(),
                                MobileNumber = reader["MobileNumber"].ToString(),
                                Address = reader["Address"].ToString(),
                                MembershipId = reader["MembershipId"] == DBNull.Value ? null : (int?)reader["MembershipId"],
                                MMRNo = reader["MMRNo"].ToString()
                            };
                        }
                    }
                }
            }

            return patient;
        }



        public async Task<Patient> GetPatientByPhoneAsync(string phoneNumber)
        {
            Patient patient = null;

            using (SqlConnection conn = new SqlConnection(winConnString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_GetPatientByPhone", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            patient = MapReaderToPatient(reader);
                        }
                    }
                }
            }

            return patient;
        }
        private Patient MapReaderToPatient(SqlDataReader reader)
        {
            return new Patient
            {
                PatientId = Convert.ToInt32(reader["PatientId"]),
                PatientName = reader["PatientName"].ToString(),
                MMRNo = reader["MMRNo"].ToString(),
                MobileNumber = reader["MobileNumber"].ToString(),
                Gender = reader["Gender"].ToString(),
                DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                Address = reader["Address"].ToString(),
                BloodGroup = reader["BloodGroup"].ToString(),

                // Safe handling for NULL MembershipId
                MembershipId = reader["MembershipId"] == DBNull.Value
                                ? null
                                : (int?)Convert.ToInt32(reader["MembershipId"])
            };
        }
    }
}





