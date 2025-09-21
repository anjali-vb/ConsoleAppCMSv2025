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
    public class BillingRepositoryImpl : IBillingRepository
    {
        private readonly string _connString = ConfigurationManager.ConnectionStrings["CsWinSql"].ConnectionString;

        public async Task AddBillingAsync(Billing billing)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_AddBilling", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@AppointmentId", billing.AppointmentId);
                    cmd.Parameters.AddWithValue("@ConsultationFee", billing.ConsultationFee);
                    cmd.Parameters.AddWithValue("@IsPaid", billing.IsPaid);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}