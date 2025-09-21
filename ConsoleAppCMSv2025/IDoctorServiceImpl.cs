using ConsoleAppCMSv2025.Model;
using ConsoleAppCMSv2025.Repository;
using ConsoleAppCMSv2025.Service;

namespace ClinicCMS
{
    internal class IDoctorServiceImpl : IDoctorService
    {
        private DoctorRepositoryImpl doctorRepositoryImpl;

        public IDoctorServiceImpl(DoctorRepositoryImpl doctorRepositoryImpl)
        {
            this.doctorRepositoryImpl = doctorRepositoryImpl;
        }

        public Task<List<Doctor>> GetAllDoctorsAsync()
        {
           return doctorRepositoryImpl.GetAllDoctorsAsync();
        }
    }
}