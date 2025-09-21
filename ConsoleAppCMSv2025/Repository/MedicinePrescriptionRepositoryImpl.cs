using ConsoleAppCMSv2025.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace ConsoleAppCMSv2025.Repository
{
    internal class MedicinePrescriptionRepositoryImpl : IMedicinePrescriptionRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        public int AddMedicinePrescription(MedicinePrescription prescription)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_AddMedicinePrescription", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MedicineId", prescription.MedicineId);
                cmd.Parameters.AddWithValue("@DosageId", prescription.DosageId);
                cmd.Parameters.AddWithValue("@Quantity", prescription.Quantity);
                cmd.Parameters.AddWithValue("@Duration", prescription.Duration ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AppointmentId", prescription.AppointmentId);
                cmd.Parameters.AddWithValue("@IsActive", prescription.IsActive);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public List<MedicinePrescription> GetMedicinePrescriptionsByAppointmentId(int appointmentId)
        {
            var prescriptions = new List<MedicinePrescription>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetMedicinePrescriptionsByAppointmentId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AppointmentId", appointmentId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prescriptions.Add(new MedicinePrescription
                    {
                        MedicinePrescriptionId = Convert.ToInt32(reader["MedicinePrescriptionId"]),
                        MedicineId = Convert.ToInt32(reader["MedicineId"]),
                        DosageId = Convert.ToInt32(reader["DosageId"]),
                        Quantity = reader["Quantity"].ToString(),
                        Duration = reader["Duration"].ToString(),
                        AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    });
                }
            }
            return prescriptions;
        }

        public MedicinePrescription GetMedicinePrescriptionById(int medicinePrescriptionId)
        {
            MedicinePrescription prescription = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetMedicinePrescriptionById", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MedicinePrescriptionId", medicinePrescriptionId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    prescription = new MedicinePrescription
                    {
                        MedicinePrescriptionId = Convert.ToInt32(reader["MedicinePrescriptionId"]),
                        MedicineId = Convert.ToInt32(reader["MedicineId"]),
                        DosageId = Convert.ToInt32(reader["DosageId"]),
                        Quantity = reader["Quantity"].ToString(),
                        Duration = reader["Duration"].ToString(),
                        AppointmentId = Convert.ToInt32(reader["AppointmentId"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    };
                }
            }
            return prescription;
        }

        public bool UpdateMedicinePrescription(MedicinePrescription prescription)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE TblMedicinePrescription SET MedicineId=@MedicineId, DosageId=@DosageId, Quantity=@Quantity, Duration=@Duration, AppointmentId=@AppointmentId, IsActive=@IsActive WHERE MedicinePrescriptionId=@MedicinePrescriptionId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MedicinePrescriptionId", prescription.MedicinePrescriptionId);
                cmd.Parameters.AddWithValue("@MedicineId", prescription.MedicineId);
                cmd.Parameters.AddWithValue("@DosageId", prescription.DosageId);
                cmd.Parameters.AddWithValue("@Quantity", prescription.Quantity);
                cmd.Parameters.AddWithValue("@Duration", prescription.Duration ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@AppointmentId", prescription.AppointmentId);
                cmd.Parameters.AddWithValue("@IsActive", prescription.IsActive);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteMedicinePrescription(int medicinePrescriptionId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE TblMedicinePrescription SET IsActive = 0 WHERE MedicinePrescriptionId = @MedicinePrescriptionId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MedicinePrescriptionId", medicinePrescriptionId);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
