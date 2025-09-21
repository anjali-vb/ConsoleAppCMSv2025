using ConsoleAppCMSv2025.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Data.SqlClient;


namespace ConsoleAppCMSv2025.Repository
{
    internal class LabTestRepositoryImpl : ILabTestRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        public int AddLabTest(LabTest labTest)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddLabTest", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", labTest.AppointmentId);
                cmd.Parameters.AddWithValue("@TestName", labTest.TestName);
                cmd.Parameters.AddWithValue("@TestDescription", labTest.TestDescription);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
