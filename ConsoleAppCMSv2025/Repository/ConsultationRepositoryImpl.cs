using ConsoleAppCMSv2025.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Repository
{
    public class ConsultationRepositoryImpl : IConsultationRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;


        public async Task AddConsultationAsync(Consultation consultation)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO TblConsultation 
                            (Symptoms, Diagnosis, Notes, AppointmentId, IsActive) 
                             VALUES (@Symptoms, @Diagnosis, @Notes, @AppointmentId, @IsActive)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Symptoms", consultation.Symptoms ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Diagnosis", consultation.Diagnosis ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Notes", consultation.Notes ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AppointmentId", consultation.AppointmentId);
                cmd.Parameters.AddWithValue("@IsActive", consultation.IsActive);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}