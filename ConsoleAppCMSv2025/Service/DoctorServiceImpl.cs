using ConsoleAppCMSv2025.Model;
using ConsoleAppCMSv2025.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Service
{
    public class DoctorServiceImpl : IDoctorService
    {
        private readonly IDoctorRepository doctorRepository;

        public DoctorServiceImpl(IDoctorRepository doctorRepository)
        {
            this.doctorRepository = doctorRepository;
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await doctorRepository.GetAllDoctorsAsync();
        }

    }
}