using ConsoleAppCMSv2025.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Service
{
    public interface IPatientService
    {
        // For registering a new patient
        Task<int> RegisternewPatientAsync(Patient patient);

        // For searching patient by MMR No
        Task<Patient> GetPatientByMMRAsync(string mmrNo);

        // For searching patient by  Phone number

        Task<Patient> GetPatientByPhoneAsync(string phoneNumber);
    }
}