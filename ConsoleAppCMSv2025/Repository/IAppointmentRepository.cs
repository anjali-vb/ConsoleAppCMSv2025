using ConsoleAppCMSv2025.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Repository
{
    public interface IAppointmentRepository
    {
        //to craete an appointment
        Task<(int AppointmentId, int TokenNumber)> CreateAppointmentAsync(Appointment appointment);

        //to view the doctor appointment

        Task<List<Appointment>> GetAppointmentsByDoctorUserIdAsync(int doctorId);

        Task<List<Appointment>> GetAppointments();

    }
}