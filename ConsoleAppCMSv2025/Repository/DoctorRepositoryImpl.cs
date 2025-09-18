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
    public class DoctorRepositoryImpl : IDoctorRepository
    {
        string winConnString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            var doctors = new List<Doctor>();
            using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllDoctors", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    doctors.Add(new Doctor
                    {
                        DoctorId = Convert.ToInt32(reader["DoctorId"]),
                       
                        ConsultationFee = Convert.ToDecimal(reader["ConsultationFee"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    });
                }
            }
            return doctors;
        }

        public async Task<Doctor> GetDoctorByIdAsync(int doctorId)
        {
            Doctor doctor = null;
            using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetDoctorById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    doctor = new Doctor
                    {
                        DoctorId = Convert.ToInt32(reader["DoctorId"]),
                      
                        ConsultationFee = Convert.ToDecimal(reader["ConsultationFee"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    };
                }
            }
            return doctor;
        }

        public async Task<int> AddDoctorAsync(Doctor doctor)
        {
            using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddDoctor", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ConsultationFee", doctor.ConsultationFee);
                cmd.Parameters.AddWithValue("@IsActive", doctor.IsActive);

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> UpdateDoctorAsync(Doctor doctor)
        {
            using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateDoctor", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorId", doctor.DoctorId);
                cmd.Parameters.AddWithValue("@ConsultationFee", doctor.ConsultationFee);
                cmd.Parameters.AddWithValue("@IsActive", doctor.IsActive);

                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<int> DeleteDoctorAsync(int doctorId)
        {
            using (SqlConnection conn = ConnectionManager.OpenConnection(winConnString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteDoctor", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);

                return await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}