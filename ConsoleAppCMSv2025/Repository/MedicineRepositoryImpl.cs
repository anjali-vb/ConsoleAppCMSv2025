using ConsoleAppCMSv2025.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Configuration;


namespace ConsoleAppCMSv2025.Repository
{
    internal class MedicineRepositoryImpl : IMedicineRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        public List<Medicine> GetAllMedicines()
        {
            var medicines = new List<Medicine>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllMedicines", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    medicines.Add(new Medicine
                    {
                        MedicineId = Convert.ToInt32(reader["MedicineId"]),
                        MedicineName = reader["MedicineName"].ToString(),
                        ManufacturingDate = Convert.ToDateTime(reader["ManufacturingDate"]),
                        ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"]),
                        MedicineCategoryId = Convert.ToInt32(reader["MedicineCategoryId"]),
                        Unit = reader["Unit"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    });
                }
            }
            return medicines;
        }

        public Medicine GetMedicineById(int medicineId)
        {
            Medicine medicine = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM TblMedicine WHERE MedicineId = @MedicineId AND IsActive = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MedicineId", medicineId);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    medicine = new Medicine
                    {
                        MedicineId = Convert.ToInt32(reader["MedicineId"]),
                        MedicineName = reader["MedicineName"].ToString(),
                        ManufacturingDate = Convert.ToDateTime(reader["ManufacturingDate"]),
                        ExpiryDate = Convert.ToDateTime(reader["ExpiryDate"]),
                        MedicineCategoryId = Convert.ToInt32(reader["MedicineCategoryId"]),
                        Unit = reader["Unit"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        IsActive = Convert.ToBoolean(reader["IsActive"])
                    };
                }
            }
            return medicine;
        }

        public int AddMedicine(Medicine medicine)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO TblMedicine (MedicineName, ManufacturingDate, ExpiryDate, MedicineCategoryId, Unit, CreatedDate, IsActive) 
                                 VALUES (@MedicineName, @ManufacturingDate, @ExpiryDate, @MedicineCategoryId, @Unit, @CreatedDate, @IsActive);
                                 SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MedicineName", medicine.MedicineName);
                cmd.Parameters.AddWithValue("@ManufacturingDate", medicine.ManufacturingDate);
                cmd.Parameters.AddWithValue("@ExpiryDate", medicine.ExpiryDate);
                cmd.Parameters.AddWithValue("@MedicineCategoryId", medicine.MedicineCategoryId);
                cmd.Parameters.AddWithValue("@Unit", medicine.Unit);
                cmd.Parameters.AddWithValue("@CreatedDate", medicine.CreatedDate);
                cmd.Parameters.AddWithValue("@IsActive", medicine.IsActive);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public bool UpdateMedicine(Medicine medicine)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE TblMedicine SET MedicineName=@MedicineName, ManufacturingDate=@ManufacturingDate, ExpiryDate=@ExpiryDate, MedicineCategoryId=@MedicineCategoryId, Unit=@Unit, IsActive=@IsActive WHERE MedicineId=@MedicineId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MedicineId", medicine.MedicineId);
                cmd.Parameters.AddWithValue("@MedicineName", medicine.MedicineName);
                cmd.Parameters.AddWithValue("@ManufacturingDate", medicine.ManufacturingDate);
                cmd.Parameters.AddWithValue("@ExpiryDate", medicine.ExpiryDate);
                cmd.Parameters.AddWithValue("@MedicineCategoryId", medicine.MedicineCategoryId);
                cmd.Parameters.AddWithValue("@Unit", medicine.Unit);
                cmd.Parameters.AddWithValue("@IsActive", medicine.IsActive);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteMedicine(int medicineId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE TblMedicine SET IsActive = 0 WHERE MedicineId = @MedicineId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MedicineId", medicineId);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
