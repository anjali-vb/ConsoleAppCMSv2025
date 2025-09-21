using ConsoleAppCMSv2025.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Service
{
    public interface IAppointmentService
    {
        Task<(int AppointmentId, int TokenNumber)> CreateAppointmentAsync(Appointment appointment);
        Task<List<Appointment>> GetAppointmentsByDoctorUserIdAsync(int userId);

    }
}