using ConsoleAppCMSv2025.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Repository
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetAllDoctorsAsync();
        Task<Doctor> GetDoctorByIdAsync(int doctorId);
        Task<int> AddDoctorAsync(Doctor doctor);
        Task<int> UpdateDoctorAsync(Doctor doctor);
        Task<int> DeleteDoctorAsync(int doctorId);
    }
}
