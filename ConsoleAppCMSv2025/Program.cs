using ConsoleAppCMSv2025.Model;
using ConsoleAppCMSv2025.Repository;
using ConsoleAppCMSv2025.Service;
using ConsoleAppCMSv2025.Utility;
using System;
using System.Text.RegularExpressions;

namespace ClinicCMS
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // LOGIN LOOP
            while (true)
            {
            lblUserName:
                Console.Clear();
                Console.WriteLine("-------------------");
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("   L O G I N   ");
                Console.ResetColor();
                Console.WriteLine("-------------------");

                // Username
                Console.Write("Enter Username : ");
                string userName = Console.ReadLine();

                // Validate username
                if (!CustomValidation.IsValidUserName(userName))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Invalid username, try again...");
                    Console.ResetColor();
                    goto lblUserName;
                }

            lblPassword:
                // Password
                Console.Write("Enter Password : ");
                string password = CustomValidation.ReadPassword();

                // Validate password
                if (!CustomValidation.IsValidPassword(password))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Invalid password, try again...");
                    Console.ResetColor();
                    goto lblPassword;
                }

                // Call UserService (BLL) with UserRepository (DAL)
                IUserService userService = new UserServiceImpl(new UserRepositoryImpl());
                int roleId = await userService.AuthenticateUserByRoleIdAsync(userName, password);

                if (roleId > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n User validated successfully!\n");
                    Console.ResetColor();

                    // Redirect based on RoleId
                    switch (roleId)
                    {
                        case 1: // Doctor
                            await ShowDoctorMenuAsync();
                            break;

                        case 2: // Receptionist
                            await ShowReceptionistMenuAsync();
                            break;

                        case 3: // Lab Technician
                            await ShowLabTechnicianMenuAsync();
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(" Invalid role: ACCESS DENIED!");
                            Console.ResetColor();
                            break;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Invalid credentials. Try again.");
                    Console.ResetColor();
                }

                Console.WriteLine("\nPress any key to return to login...");
                Console.ReadKey();
            }
        }

        // Doctor Menu
        private static async Task ShowDoctorMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("----- Doctor Dashboard ----");
                Console.WriteLine("1. View Appointments");
                Console.WriteLine("2. Write Prescription");
                Console.WriteLine("3. View Patient History (by MMR No)");
                Console.WriteLine("4. Medicine Prescription");
                Console.WriteLine("5. Consultation Details");
                Console.WriteLine("6. Prescribe Lab Test for Patient");
                Console.WriteLine("7. Logout");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine(" View Appointments (AppointmentService)");
                        break;
                    case "2":
                        Console.WriteLine(" Write Prescription (MedicineService/ConsultationService)");
                        break;
                    case "3":
                        Console.WriteLine(" View Patient History by MMR No (PatientService)");
                        break;
                    case "4":
                        Console.WriteLine(" Medicine Prescription (MedicineService)");
                        break;
                    case "5":
                        Console.WriteLine(" Consultation Details (ConsultationService)");
                        break;
                    case "6":
                        Console.WriteLine(" Prescribe Lab Test for Patient (LabTestService)");
                        break;
                    case "7":
                        return; // back to login
                    default:
                        Console.WriteLine(" Invalid choice! Try again...");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        // Receptionist Menu
        private static async Task ShowReceptionistMenuAsync()
        {
            IPatientService patientService = new PatientServiceImpl(new PatientRepositoryImpl());
            while (true)
            {
                Console.Clear();
                Console.WriteLine("------ Receptionist Dashboard ------");
                Console.WriteLine("1. Register New Patient");
                Console.WriteLine("2. Create Appointment");
                Console.WriteLine("3. Search Patient by MMR No");
                Console.WriteLine("4. Search Patient by Phone Number");
                Console.WriteLine("5. Exit");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await RegisternewPatientAsync(patientService);
                        break;
                    case "2":
                        IAppointmentService appointmentService = new AppointmentServiceImpl(new AppointmentRepositoryImpl());
                        await CreateAppointmentAsync(appointmentService);
                        break;
                    case "3":
                        await SearchPatientByMMRAsync(patientService);
                        break;
                    case "4":
                        Console.WriteLine(" Search Patient by Phone Number (PatientRepository)");
                        break;
                    case "5":
                        return; // back to login
                    default:
                        Console.WriteLine(" Invalid choice! Try again...");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        

        // Register Patient Method
        private static async Task RegisternewPatientAsync(IPatientService patientService)
        {
            Console.Clear();
            Console.WriteLine("---- Register New Patient ----");

            var patient = new Patient();

            Console.Write("Enter Patient Name: ");
            patient.PatientName = Console.ReadLine();

            Console.Write("Enter Date of Birth (yyyy-mm-dd): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime dob))
                patient.DateOfBirth = dob;

            Console.Write("Enter Gender: ");
            patient.Gender = Console.ReadLine();

            Console.Write("Enter Blood Group: ");
            patient.BloodGroup = Console.ReadLine();

            Console.Write("Enter Mobile Number: ");
            patient.MobileNumber = Console.ReadLine();

            Console.Write("Enter Address: ");
            patient.Address = Console.ReadLine();

            Console.Write("Enter MembershipId (or leave blank): ");
            string membershipInput = Console.ReadLine();
            patient.MembershipId = string.IsNullOrWhiteSpace(membershipInput) ? null : int.Parse(membershipInput);

            int newPatientId = await patientService.RegisternewPatientAsync(patient);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n Patient registered successfully with PatientId = {newPatientId}");
            Console.ResetColor();

            // ✅ Menu after registration
            Console.WriteLine("\nWhat would you like to do next?");
            Console.WriteLine("1. Register another patient");
            Console.WriteLine("2. Create an appointment for this patient");
            Console.WriteLine("3. Go back to Receptionist Dashboard");
            Console.WriteLine("4. Exit");

            string nextChoice = Console.ReadLine();

            switch (nextChoice)
            {
                case "1":
                    await RegisternewPatientAsync(patientService); // loop back
                    break;
                case "2":
                    IAppointmentService appointmentService = new AppointmentServiceImpl(new AppointmentRepositoryImpl());
                    await CreateAppointmentAsync(appointmentService); // go to appointment
                    break;
                case "3":
                    return; // back to receptionist menu
                case "4":
                    Environment.Exit(0); // exit app
                    break;
                default:
                    Console.WriteLine("Invalid choice, returning to Receptionist Dashboard...");
                    return;
            }
        }

        //create an appointment method
        private static async Task CreateAppointmentAsync(IAppointmentService appointmentService)
        {
            Console.Clear();
            Console.WriteLine("---- Create Appointment ----");
            var appointment = new Appointment();

            Console.Write("Enter Appointment Date (yyyy-mm-dd): ");
            appointment.AppointmentDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Enter Appointment Time Slot: ");
            appointment.AppointmentTimeSlot = Console.ReadLine();

            Console.Write("Enter Consultation Status: ");
            appointment.ConsultationStatus = Console.ReadLine();

            Console.Write("Enter Patient ID: ");
            appointment.PatientId = int.Parse(Console.ReadLine());

            Console.Write("Enter Doctor ID: ");
            appointment.DoctorId = int.Parse(Console.ReadLine());

            Console.Write("Enter User ID (Receptionist ID): ");
            appointment.UserId = int.Parse(Console.ReadLine());

            appointment.IsActive = true;

            Console.Write("Enter Time Slot: ");
            appointment.TimeSlot = Console.ReadLine();

            var (appointmentId, tokenNumber) = await appointmentService.CreateAppointmentAsync(appointment);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n Appointment created successfully!");
            Console.WriteLine($" Appointment ID: {appointmentId}");
            Console.WriteLine($" Token Number: {tokenNumber}");
            Console.ResetColor();

            Console.WriteLine("\nWhat would you like to do next?");
            Console.WriteLine("1. Create another appointment");
            Console.WriteLine("2. Go back to Receptionist Dashboard");
            Console.WriteLine("3. Exit");

            string nextChoice = Console.ReadLine();

            switch (nextChoice)
            {
                case "1":
                    await CreateAppointmentAsync(appointmentService); // loop back
                    break;
                case "2":
                    return; // back to menu
                case "3":
                    Environment.Exit(0); // exit app
                    break;
                default:
                    Console.WriteLine("Invalid choice, returning to Receptionist Dashboard...");
                    return;
            }
        }

        //search patient by MMRNo
        public static async Task SearchPatientByMMRAsync(IPatientService patientService)
        {
            Console.Write("\nEnter MMR Number: ");
            string mmrNo = Console.ReadLine();

            var patient = await patientService.GetPatientByMMRAsync(mmrNo);

            if (patient != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nPatient Found!");
                Console.WriteLine($"ID: {patient.PatientId}");
                Console.WriteLine($"Name: {patient.PatientName}");
                Console.WriteLine($"Date of Birth: {patient.DateOfBirth:dd-MM-yyyy}");
                Console.WriteLine($"Gender: {patient.Gender}");
                Console.WriteLine($"Blood Group: {patient.BloodGroup}");
                Console.WriteLine($"Mobile: {patient.MobileNumber}");
                Console.WriteLine($"Address: {patient.Address}");
                Console.WriteLine($"Membership ID: {(patient.MembershipId.HasValue ? patient.MembershipId.ToString() : "N/A")}");
                Console.WriteLine($"MMR No: {patient.MMRNo}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo patient found with that MMR number.");
                Console.ResetColor();
            }

            // Menu-driven options after search
            Console.WriteLine("\nWhat would you like to do next?");
            Console.WriteLine("1. Search another patient by MMR");
            Console.WriteLine("2. Go back to Receptionist Dashboard");
            Console.WriteLine("3. Exit");

            string nextChoice = Console.ReadLine();

            switch (nextChoice)
            {
                case "1":
                    await SearchPatientByMMRAsync(patientService); // recursive search again
                    break;
                case "2":
                    return; // back to dashboard
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice, returning to Receptionist Dashboard...");
                    return;
            }
        }


        // Lab Technician Menu
        private static async Task ShowLabTechnicianMenuAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(" -------- Lab Technician Dashboard ------");
                Console.WriteLine("1. View Lab Tests of Patient (by MMR No)");
                Console.WriteLine("2. Add Lab Report");
                Console.WriteLine("3. View Patient Lab History (by MMR No)");
                Console.WriteLine("4. Exit");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine(" View Lab Tests of Patient by MMR No (LabTestRepository)");
                        break;
                    case "2":
                        Console.WriteLine(" Add Lab Report (LabReportService)");
                        break;
                    case "3":
                        Console.WriteLine(" View Patient Lab History by MMR No (LabReportService)");
                        break;
                    case "4":
                        return; // back to login
                    default:
                        Console.WriteLine(" Invalid choice! Try again...");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }
    }
}