// ...existing code...
            if (patient != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nPatient Found!");
                Console.WriteLine($"ID: {patient.PatientId}");
                Console.WriteLine($"Name: {patient.PatientName}");
                Console.WriteLine($"Date of Birth: {patient.DateOfBirth:dd-MM-yyyy}");
                int age = DateTime.Today.Year - patient.DateOfBirth.Year;
                if (patient.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
                Console.WriteLine($"Age: {age}");
                Console.WriteLine($"Gender: {patient.Gender}");
                Console.WriteLine($"Blood Group: {patient.BloodGroup}");
                Console.WriteLine($"Mobile: {patient.MobileNumber}");
                Console.WriteLine($"Address: {patient.Address}");
                Console.WriteLine($"Membership ID: {(patient.MembershipId.HasValue ? patient.MembershipId.ToString() : "N/A")}");
                Console.WriteLine($"MMR No: {patient.MMRNo}");
                Console.ResetColor();
            }
// ...existing code...
            if (patient != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nPatient Found!");
                Console.WriteLine($"ID: {patient.PatientId}");
                Console.WriteLine($"Name: {patient.PatientName}");
                Console.WriteLine($"Date of Birth: {patient.DateOfBirth:dd-MM-yyyy}");
                int age = DateTime.Today.Year - patient.DateOfBirth.Year;
                if (patient.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
                Console.WriteLine($"Age: {age}");
                Console.WriteLine($"Gender: {patient.Gender}");
                Console.WriteLine($"Blood Group: {patient.BloodGroup}");
                Console.WriteLine($"Mobile: {patient.MobileNumber}");
                Console.WriteLine($"Address: {patient.Address}");
                Console.WriteLine($"Membership ID: {patient.MembershipId}");
                Console.WriteLine($"MMR No: {patient.MMRNo}");
                Console.ResetColor();
            }
// ...existing code...
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n Patient registered successfully with PatientId = {newPatientId}");
            Console.WriteLine($"Name: {patient.PatientName}");
            Console.WriteLine($"Date of Birth: {patient.DateOfBirth:dd-MM-yyyy}");
            int age = DateTime.Today.Year - patient.DateOfBirth.Year;
            if (patient.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
            Console.WriteLine($"Age: {age}");
            Console.WriteLine($"Gender: {patient.Gender}");
            Console.WriteLine($"Blood Group: {patient.BloodGroup}");
            Console.WriteLine($"Mobile: {patient.MobileNumber}");
            Console.WriteLine($"Address: {patient.Address}");
            Console.WriteLine($"Membership ID: {(patient.MembershipId.HasValue ? patient.MembershipId.ToString() : "N/A")}");
            Console.WriteLine($"MMR No: {patient.MMRNo}");
            Console.ResetColor();
// ...existing code...
            Console.WriteLine($"{"ID",-5} {"MMR No",-10} {"Name",-25} {"Mobile",-15} {"DOB",-12} {"Age",-5} {"Gender",-8} {"Blood Group",-10}");
            Console.WriteLine(new string('-', 100));

            foreach (var patient in patients)
            {
                int age = DateTime.Today.Year - patient.DateOfBirth.Year;
                if (patient.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
                Console.WriteLine($"{patient.PatientId,-5} {patient.MMRNo,-10} {patient.PatientName?.Substring(0, Math.Min(patient.PatientName.Length, 24)),-25} " +
                                 $"{patient.MobileNumber,-15} {patient.DateOfBirth:dd-MM-yyyy,-12} {age,-5} {patient.Gender,-8} {patient.BloodGroup,-10}");
            }
// ...existing code...