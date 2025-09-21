using ConsoleAppCMSv2025.Model;
using ConsoleAppCMSv2025.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Service
{
    public class AppointmentServiceImpl : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentServiceImpl(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public Task<(int AppointmentId, int TokenNumber)> CreateAppointmentAsync(Appointment appointment)
        {
            return _appointmentRepository.CreateAppointmentAsync(appointment);
        }



        //To view the doctor appointments by the doctor

        private static async Task ViewAppointmentsAsync(object appointmentService, object doctorId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Appointment>> GetAppointmentsByDoctorUserIdAsync(int userId)
        {
            return await _appointmentRepository.GetAppointmentsByDoctorUserIdAsync(userId);

        }
    }
    }






