using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleAppCMSv2025.Utility
{
    public class CustomValidation
    {
        #region 1 - UserName validation
        //user name should not be empty
        //user name should contain only letters,numbers,underscores, and dot
        public static bool IsValidUserName(string userName)
        {
            return !string.IsNullOrWhiteSpace(userName) &&
                userName.Length <= 20 &&
                Regex.IsMatch(userName, @"^[a-zA-Z0-9_.]+$");
        }
        #endregion
        #region 2 - Password validation
        //password should have at least 4 characters ---San@123
        //including at least one uppercase letter , one lowercase letter
        //one digit and one special character
        public static bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) &&
                Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@\W_]).{4,}$");

        }
        #endregion
        #region * symbol for password

        public static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                //Each keystroke from the user, replaces with asterisk *
                //and add it to the password string
                //until the user presses enter key
                if (key.Key != ConsoleKey.Backspace
                    && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace
                    && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.ReadKey();
            return password;
        }
        #endregion



        //validatins for TblPatient Table

          //for mobile number
        public static ValidationResult ValidateMobileNumber(string mobile, ValidationContext context)
        {
            if (string.IsNullOrEmpty(mobile))
                return new ValidationResult("Mobile number is required.");

            if (mobile.Length != 10)   // must be 10 digits
                return new ValidationResult("Mobile number must be exactly 10 digits.");

            if (!mobile.StartsWith("9"))  // must start with 9
                return new ValidationResult("Mobile number must start with 9.");

            return ValidationResult.Success;
        }

        //for DOB
        public static ValidationResult ValidateDOB(DateTime dob, ValidationContext context)
        {
            if (dob < new DateTime(1990, 1, 1)) // Must not be before 1990
                return new ValidationResult("Date of birth cannot be before 1990.");

            if (dob > DateTime.Today) // Must not be in the future
                return new ValidationResult("Date of birth cannot be in the future.");

            return ValidationResult.Success;
        }
    }
}

