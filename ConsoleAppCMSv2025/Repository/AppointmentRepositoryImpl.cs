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
    public class AppointmentRepositoryImpl : IAppointmentRepository
    {
        private readonly string _connString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        public async Task<(int AppointmentId, int TokenNumber)> CreateAppointmentAsync(Appointment appointment)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                await conn.OpenAsync();

                SqlCommand cmd = new SqlCommand("CreateAppointment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                cmd.Parameters.AddWithValue("@AppointmentTimeSlot", appointment.AppointmentTimeSlot ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ConsultationStatus", appointment.ConsultationStatus ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                cmd.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                cmd.Parameters.AddWithValue("@UserId", appointment.UserId);
                cmd.Parameters.AddWithValue("@IsActive", appointment.IsActive);
                cmd.Parameters.AddWithValue("@TimeSlot", appointment.TimeSlot ?? (object)DBNull.Value);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                int appointmentId = 0;
                int tokenNumber = 0;

                if (await reader.ReadAsync())
                {
                    appointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId"));
                    tokenNumber = reader.GetInt32(reader.GetOrdinal("TokenNumber"));
                }

                return (appointmentId, tokenNumber);
            }
        }
    }
}