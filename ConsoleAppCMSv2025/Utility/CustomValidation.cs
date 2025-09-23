using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
    

     // Regex pattern to match time slot formats like "09:00am-9:15pm", "10:10", "09:00am-11:30"
    private static readonly Regex TimeSlotPattern = new Regex(
        @"^(?:(?<start>\d{1,2}:\d{2}(?:am|pm)?)-(?<end>\d{1,2}:\d{2}(?:am|pm)?)|(?<single>\d{1,2}:\d{2}(?:am|pm)?))$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled
    );

        /// <summary>
        /// Validates if a time slot string follows the expected format
        /// Supports formats like: "09:00am-9:15pm", "10:10", "09:00am-11:30"
        /// </summary>
        /// <param name="timeSlot">The time slot string to validate</param>
        /// <returns>True if the format is valid, false otherwise</returns>
        public static bool IsValidTimeSlotFormat(string timeSlot)
        {
            if (string.IsNullOrWhiteSpace(timeSlot))
                return false;

            return TimeSlotPattern.IsMatch(timeSlot.Trim());
        }

        /// <summary>
        /// Validates time slot format and checks if the time range is logical (start time before end time)
        /// </summary>
        /// <param name="timeSlot">The time slot string to validate</param>
        /// <param name="errorMessage">Output parameter containing error details if validation fails</param>
        /// <returns>True if the time slot is valid and logical, false otherwise</returns>
        public static bool IsValidTimeSlot(string timeSlot, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(timeSlot))
            {
                errorMessage = "Time slot cannot be null or empty";
                return false;
            }

            timeSlot = timeSlot.Trim();

            if (!TimeSlotPattern.IsMatch(timeSlot))
            {
                errorMessage = "Invalid time slot format";
                return false;
            }

            var match = TimeSlotPattern.Match(timeSlot);

            // If it's a single time (not a range), it's valid
            if (match.Groups["single"].Success)
            {
                return ValidateTimeFormat(match.Groups["single"].Value, out errorMessage);
            }

            // Validate range format
            string startTime = match.Groups["start"].Value;
            string endTime = match.Groups["end"].Value;

            if (!ValidateTimeFormat(startTime, out errorMessage))
                return false;

            if (!ValidateTimeFormat(endTime, out errorMessage))
                return false;

            // Check if start time is before end time
            if (!IsStartTimeBeforeEndTime(startTime, endTime))
            {
                errorMessage = "Start time must be before end time";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates individual time format (e.g., "09:00am", "10:10", "9:15pm")
        /// </summary>
        /// <param name="time">Time string to validate</param>
        /// <param name="errorMessage">Error message if validation fails</param>
        /// <returns>True if time format is valid</returns>
        private static bool ValidateTimeFormat(string time, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // Try parsing with various formats
                string[] formats = { "h:mmtt", "hh:mmtt", "H:mm", "HH:mm" };

                foreach (string format in formats)
                {
                    if (DateTime.TryParseExact(time, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    {
                        return true;
                    }
                }

                errorMessage = $"Invalid time format: {time}";
                return false;
            }
            catch
            {
                errorMessage = $"Invalid time format: {time}";
                return false;
            }
        }

        /// <summary>
        /// Checks if start time is before end time in a time range
        /// </summary>
        /// <param name="startTime">Start time string</param>
        /// <param name="endTime">End time string</param>
        /// <returns>True if start time is before end time</returns>
        private static bool IsStartTimeBeforeEndTime(string startTime, string endTime)
        {
            try
            {
                DateTime start = ParseTimeString(startTime);
                DateTime end = ParseTimeString(endTime);

                // Handle case where end time is next day (e.g., 11:00pm - 2:00am)
                if (end < start)
                {
                    end = end.AddDays(1);
                }

                return start < end;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Parses a time string to DateTime
        /// </summary>
        /// <param name="timeString">Time string to parse</param>
        /// <returns>Parsed DateTime</returns>
        private static DateTime ParseTimeString(string timeString)
        {
            string[] formats = { "h:mmtt", "hh:mmtt", "H:mm", "HH:mm" };

            foreach (string format in formats)
            {
                if (DateTime.TryParseExact(timeString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }

            throw new FormatException($"Unable to parse time: {timeString}");
        }

    }
}

