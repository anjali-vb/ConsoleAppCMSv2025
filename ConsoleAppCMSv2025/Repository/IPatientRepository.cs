using ConsoleAppCMSv2025.Model;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Repository
{
    //interface to register patient
    public interface IPatientRepository
    {
        Task<int> RegisterPatientAsync(Patient patient);

        //interface to get patient by MMR number
        Task<Patient> GetPatientByMMRAsync(string mmrNo);

        Task<Patient> GetPatientByPhoneAsync(string phoneNumber);


    }
}
