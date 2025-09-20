using ConsoleAppCMSv2025.Model;
using ConsoleAppCMSv2025.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Service
{
    public class PatientServiceImpl : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientServiceImpl(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public Task<Patient> GetPatientByMMRAsync(string mmrNo)
        {
            return _patientRepository.GetPatientByMMRAsync(mmrNo);
        }

        public Task<int> RegisternewPatientAsync(Patient patient)
        {
            return _patientRepository.RegisterPatientAsync(patient);
        }

        public Task<Patient> GetPatientByPhoneAsync(string phoneNumber)
        {
            return _patientRepository.GetPatientByPhoneAsync(phoneNumber);
        }
    }
}


