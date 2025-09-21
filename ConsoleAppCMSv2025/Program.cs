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
                User user = await userService.AuthenticateUserByRoleIdAsync(userName, password);
                if (user == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n Invalid credentials. Try again.");
                    Console.ResetColor();
                    Console.WriteLine("\nPress any key to return to login...");
                    Console.ReadKey();
                    continue;
                }
                int roleId = user.RoleId;
                if (roleId > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n User validated successfully!\n");
                    Console.ResetColor();

                    // Redirect based on RoleId
                    switch (roleId)
                    {
                        case 1: // Doctor
                            await ShowDoctorMenuAsync(user);
                            break;

                        case 2: // Receptionist
                            await ShowReceptionistMenuAsync();
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
        private static async Task ShowDoctorMenuAsync(User user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("-----" + user.FullName + " Doctor Dashboard ----");
                Console.WriteLine("1. View Appointments");
                Console.WriteLine("2. Logout");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        IAppointmentService appointmentService = new AppointmentServiceImpl(new AppointmentRepositoryImpl());
                        await ViewAppointmentsAsync(appointmentService, user);
                        break;
                    case "2":
                        return; // back to login
                    default:
                        Console.WriteLine(" Invalid choice! Try again...");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }


        //method to view the appointment of doctor by doctor.
        public static async Task ViewAppointmentsAsync(IAppointmentService appointmentService, User user)
        {
            var appointments = await appointmentService.GetAppointmentsByDoctorUserIdAsync(user.UserId);

            if (appointments == null || appointments.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo appointments found.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n--- Appointments for Doctor {user.FullName} ---\n");
                Console.ResetColor();

                foreach (var app in appointments)
                {
                    Console.Write($"Appointment ID : {app.AppointmentId}|");
                    Console.Write($"Date : {app.AppointmentDate:dd-MM-yyyy}|");
                    Console.Write($"Period : {app.PeriodName}|");
                    Console.Write($"Time Slot  : {app.TimeSlot}|");
                    Console.Write($"Token Number : {app.TokenNumber}|");
                    Console.Write($"Status : {app.ConsultationStatus}|");
                    Console.Write($"Patient Name : {app.PatientName}|");
                    Console.Write($"MMR No : {app.MMRNo}|\n");
                    Console.WriteLine("----------------------------------------");
                }

                // Prompt for Appointment ID to add consultation
                Console.WriteLine("\nEnter Appointment ID to add consultation or press Enter to go back:");
                string input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    if (int.TryParse(input, out int appointmentId))
                    {
                        IConsultationService consultationService = new ConsultationServiceImpl(new ConsultationRepositoryImpl());
                        await AddConsultationAsync(consultationService, appointmentId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Appointment ID.");
                    }
                }
            }
        }

        private static async Task AddConsultationAsync(IConsultationService consultationService, int appointmentId)
        {
            Console.Clear();
            Console.WriteLine("---- Add consultation ----");
            Console.WriteLine($"For Appointment ID: {appointmentId}");

            Console.Write("Enter Symptoms: ");
            string symptoms = Console.ReadLine();

            Console.Write("Enter Diagnosis: ");
            string diagnosis = Console.ReadLine();

            Console.Write("Enter Notes: ");
            string notes = Console.ReadLine();

            Consultation consultation = new Consultation
            {
                AppointmentId = appointmentId,
                Symptoms = symptoms,
                Diagnosis = diagnosis,
                Notes = notes,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            await consultationService.AddConsultationAsync(consultation);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nConsultation added successfully!");
            Console.ResetColor();

            // Sub-menu for further actions
            while (true)
            {
                Console.WriteLine("\nWhat would you like to do next?");
                Console.WriteLine("1. Write Medicine Prescription");
                Console.WriteLine("2. Prescribe Lab Test for Patient");
                Console.WriteLine("3. Go back to Appointments");
                Console.WriteLine("4. Logout");
                string nextChoice = Console.ReadLine();
                switch (nextChoice)
                {
                    case "1":
                        await WriteMedicinePrescriptionAsync(appointmentId);
                        break;
                    case "2":
                        await PrescribeLabTestAsync(appointmentId);
                        break;
                    case "3":
                        return;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice, try again...");
                        break;
                }
            }
        }

        //method to write medicine prescription
        private static async Task WriteMedicinePrescriptionAsync(int appointmentId)
        {
            IMedicineService medicineService = new MedicineServiceImpl(new MedicineRepositoryImpl());
            var medicines = medicineService.GetAllMedicines();
            Console.WriteLine("\nAvailable Medicines:");
            foreach (var med in medicines)
            {
                Console.WriteLine($"ID: {med.MedicineId}, Name: {med.MedicineName}, Unit: {med.Unit}, Expiry: {med.ExpiryDate:dd-MM-yyyy}");
            }
            Console.Write("Enter Medicine ID from the list (or press Enter to skip): ");
            string medIdInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(medIdInput) && int.TryParse(medIdInput, out int medicineId))
            {
                // List all dosages before entering dosage id
                IDosageService dosageService = new DosageServiceImpl(new DosageRepositoryImpl());
                var dosages = dosageService.GetAllDosages();
                Console.WriteLine("\nAvailable Dosages:");
                foreach (var dosage in dosages)
                {
                    Console.WriteLine($"ID: {dosage.DosageId}, Name: {dosage.DosageName}");
                }
                Console.Write("Enter Dosage ID from the list: ");
                int dosageId = int.Parse(Console.ReadLine());
                Console.Write("Enter Quantity: ");
                string quantity = Console.ReadLine();
                Console.Write("Enter Duration: ");
                string duration = Console.ReadLine();

                MedicinePrescription prescription = new MedicinePrescription
                {
                    MedicineId = medicineId,
                    DosageId = dosageId,
                    Quantity = quantity,
                    Duration = duration,
                    AppointmentId = appointmentId,
                    IsActive = true
                };
                IMedicinePrescriptionService prescriptionService = new MedicinePrescriptionServiceImpl(new MedicinePrescriptionRepositoryImpl());
                prescriptionService.AddMedicinePrescription(prescription);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nPrescription saved: MedicineId={medicineId}, DosageId={dosageId}, Quantity={quantity}, Duration={duration}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("No medicine selected.");
            }
        }

        private static async Task PrescribeLabTestAsync(int appointmentId)
        {
            Console.WriteLine("\n--- Prescribe Lab Test ---");
            Console.Write("Enter Test Name: ");
            string testName = Console.ReadLine();
            Console.Write("Enter Test Description: ");
            string testDescription = Console.ReadLine();

            LabTest labTest = new LabTest
            {
                AppointmentId = appointmentId,
                TestName = testName,
                TestDescription = testDescription
            };
            ILabTestService labTestService = new LabTestServiceImpl(new LabTestRepositoryImpl());
            labTestService.AddLabTest(labTest);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nLab test prescribed and saved successfully!");
            Console.ResetColor();
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
                Console.WriteLine("2. Show doctor avaialability");
                Console.WriteLine("3. Create Appointment");
                Console.WriteLine("4. Search Patient by MMR No");
                Console.WriteLine("5. Search Patient by Phone Number");
                Console.WriteLine("6. Generate bill");
                Console.WriteLine("7. Exit");
                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await RegisternewPatientAsync(patientService);
                        break;
                    case "2":
                        IDoctorService doctorService = new DoctorServiceImpl(new DoctorRepositoryImpl());
                        await ShowDoctorAvailabilityAsync(doctorService);
                        break;
                    case "3":
                        IAppointmentService appointmentService = new AppointmentServiceImpl(new AppointmentRepositoryImpl());
                        await CreateAppointmentAsync(appointmentService);
                        break;
                    case "4":
                        await SearchPatientByMMRAsync(patientService);
                        break;
                    case "5":
                        await SearchPatientByPhoneAsync(patientService);
                        break;
                    case "6":
                        await GenerateBillAsync();
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

            //  Menu after registration
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

        //method to show availability of doctors
        private static async Task ShowDoctorAvailabilityAsync(IDoctorService doctorService)
        {
            Console.Clear();
            Console.WriteLine("---- Doctor Availability ----");

            var doctors = await doctorService.GetAllDoctorsAsync();

            if (doctors.Count == 0)
            {
                Console.WriteLine("No doctors found.");
            }
            else
            {
                foreach (var doc in doctors)
                {
                    Console.Write($"\nDoctor ID: {doc.DoctorId} |");
                    Console.Write($"Name: {doc.DoctorName} |");
                    Console.Write($"Consultation Fee: {doc.ConsultationFee:0.00} |");
                    Console.Write($"Department: {doc.Department} |");
                    Console.Write($"Period: {doc.PeriodName} |");
                    Console.Write($"Time Slot: {doc.TimeSlot} |");
                    Console.Write($"Active: {(doc.IsActive ? "Yes" : "No")} |");
                }
            }

            // Menu navigation
            Console.WriteLine("\nWhat would you like to do next?");
            Console.WriteLine("1. View again");
            Console.WriteLine("2. Go back to Receptionist Dashboard");
            Console.WriteLine("3. Exit");

            string nextChoice = Console.ReadLine();

            switch (nextChoice)
            {
                case "1":
                    await ShowDoctorAvailabilityAsync(doctorService);
                    break;
                case "2":
                    return;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice, returning to dashboard...");
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

            Console.Write("Enter PeriodName: ");
            appointment.PeriodName = Console.ReadLine();

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

        //method to search patient by phone number
        public static async Task SearchPatientByPhoneAsync(IPatientService patientService)
        {
            Console.Write("\nEnter Phone Number: ");
            string phone = Console.ReadLine();

            var patient = await patientService.GetPatientByPhoneAsync(phone);

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
                Console.WriteLine($"Membership ID: {patient.MembershipId}");
                Console.WriteLine($"MMR No: {patient.MMRNo}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nNo patient found with that phone number.");
                Console.ResetColor();
            }

            // Menu-driven options after search
            Console.WriteLine("\nWhat would you like to do next?");
            Console.WriteLine("1. Search another patient by Phone");
            Console.WriteLine("2. Go back to Receptionist Dashboard");
            Console.WriteLine("3. Exit");

            string nextChoice = Console.ReadLine();

            switch (nextChoice)
            {
                case "1":
                    await SearchPatientByPhoneAsync(patientService); // recursive search again
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

        private static async Task GenerateBillAsync()
        {
            IAppointmentService appointmentService = new AppointmentServiceImpl(new AppointmentRepositoryImpl());
            IBillingService billingService = new BillingServiceImpl(new BillingRepositoryImpl());
            while (true)
            {
                // List all appointments (for all patients)
                var appointments = await appointmentService.GetAppointments(); // 0 or overload for all
                if (appointments == null || appointments.Count == 0)
                {
                    Console.WriteLine("No appointments found.");
                    return;
                }

                Console.WriteLine("\n--- All Appointments ---");
                foreach (var app in appointments)
                {
                    Console.WriteLine($"Appointment ID: {app.AppointmentId} | Patient Name: {app.PatientName} | Date: {app.AppointmentDate:dd-MM-yyyy} | Status: {app.ConsultationStatus}");
                }

                // Prompt for AppointmentId
                Console.Write("\nEnter Appointment ID to generate bill: ");
                if (!int.TryParse(Console.ReadLine(), out int appointmentId))
                {
                    Console.WriteLine("Invalid Appointment ID.");
                    return;
                }

                // Find the selected appointment
                var selectedAppointment = appointments.Find(a => a.AppointmentId == appointmentId);
                if (selectedAppointment == null)
                {
                    Console.WriteLine("Appointment not found.");
                    return;
                }

                // Prompt for ConsultationFee
                Console.Write("Enter Consultation Fee: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal fee))
                {
                    Console.WriteLine("Invalid Fee.");
                    return;
                }

                // Prompt for IsPaid
                Console.Write("Is Paid? (yes/no): ");
                string isPaidInput = Console.ReadLine();
                bool isPaid = isPaidInput.Equals("yes", StringComparison.OrdinalIgnoreCase);

                // Create and save bill
                var bill = new Billing
                {
                    AppointmentId = appointmentId,
                    ConsultationFee = fee,
                    BillDate = DateTime.Now,
                    IsPaid = isPaid
                };

                await billingService.AddBillingAsync(bill);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nBill generated successfully!");
                Console.ResetColor();

                Console.WriteLine("\nWhat would you like to do next?");
                Console.WriteLine("1. Generate another Bill");
                Console.WriteLine("2. Go back to Receptionist Dashboard");
                Console.WriteLine("3. Logout");
                string nextChoice = Console.ReadLine();
                switch (nextChoice)
                {
                    case "1":
                        continue;
                    case "2":
                        return;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice, returning to Receptionist Dashboard...");
                        return;
                }
            }
        }
    }
}

