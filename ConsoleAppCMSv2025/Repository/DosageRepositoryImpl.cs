using ConsoleAppCMSv2025.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ConsoleAppCMSv2025.Repository
{
    internal class DosageRepositoryImpl : IDosageRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        public List<Dosage> GetAllDosages()
        {
            var dosages = new List<Dosage>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllDosages", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dosages.Add(new Dosage
                    {
                        DosageId = Convert.ToInt32(reader["DosageId"]),
                        DosageName = reader["DosageName"].ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    });
                }
            }
            return dosages;
        }

        public Dosage GetDosageById(int dosageId)
        {
            Dosage dosage = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM TblDosage WHERE DosageId = @DosageId AND IsActive = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DosageId", dosageId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dosage = new Dosage
                    {
                        DosageId = Convert.ToInt32(reader["DosageId"]),
                        DosageName = reader["DosageName"].ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    };
                }
            }
            return dosage;
        }

        public int AddDosage(Dosage dosage)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO TblDosage (DosageName, IsActive) VALUES (@DosageName, @IsActive); SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DosageName", dosage.DosageName);
                cmd.Parameters.AddWithValue("@IsActive", dosage.IsActive);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public bool UpdateDosage(Dosage dosage)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE TblDosage SET DosageName=@DosageName, IsActive=@IsActive WHERE DosageId=@DosageId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DosageId", dosage.DosageId);
                cmd.Parameters.AddWithValue("@DosageName", dosage.DosageName);
                cmd.Parameters.AddWithValue("@IsActive", dosage.IsActive);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteDosage(int dosageId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE TblDosage SET IsActive = 0 WHERE DosageId = @DosageId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DosageId", dosageId);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
