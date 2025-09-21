using ConsoleAppCMSv2025.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Repository
{
    public class ConsultationRepositoryImpl : IConsultationRepository
    {
        private readonly string winConnString = "your_connection_string_here";

        public async Task AddConsultationAsync(Consultation consultation)
        {
            using (SqlConnection conn = new SqlConnection(winConnString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_AddConsultation", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Symptoms", consultation.Symptoms);
                    cmd.Parameters.AddWithValue("@Diagnosis", consultation.Diagnosis);
                    cmd.Parameters.AddWithValue("@Notes", consultation.Notes);
                    cmd.Parameters.AddWithValue("@AppointmentId", consultation.AppointmentId);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
