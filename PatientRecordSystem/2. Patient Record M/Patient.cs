using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PatientRecordSystem
{
    internal class Patient
    {
        // Properties - https://www.w3schools.com/cs/cs_properties.php + https://www.webdevtutor.net/blog/c-sharp-class-property-method#google_vignette + https://dotnettutorials.net/lesson/properties-csharp/?utm_content=cmp-true
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ContactDetails { get; set; }
        public string NHSNumber { get; set; }
        public string HospitalNumber { get; set; }
        //public int PatientID { get; set; }


        public Patient() { } // Parameterless constructor (implicitly added if not explicitly defined)
        internal List<Notes> note { get; set; } = new List<Notes>();
        public static List<Patient> PatientList = new List<Patient>();



        public Patient(int patientId, string firstName, string lastName) // Optional constructor for initialization
        {
            //PatientID = patientId;
            FirstName = firstName;
            LastName = lastName;
        }



        // Constructor
        public Patient(string firstName, string lastName, DateTime dateOfBirth, string contactDetails, string nhsNumber, string hospitalNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            ContactDetails = contactDetails;
            NHSNumber = nhsNumber;
            HospitalNumber = hospitalNumber;
        }

        // Input for 2.3 
        public static string PromptForNHSNumber()
        {
            Console.Write("Enter NHS Number: ");
            string nhsNumber = Console.ReadLine();

            // Validate input
            while (string.IsNullOrWhiteSpace(nhsNumber))
            {
                Console.WriteLine("NHS Number cannot be empty. Please try again.");
                Console.Write("Enter NHS Number: ");
                nhsNumber = Console.ReadLine();
            }

            return nhsNumber;
        }

        public static Patient GetPatientByNHSNumber()
        {
            string nhsNumber = PromptForNHSNumber(); // Prompt the user for the NHS number
            Patient patient = PatientManager.SearchPatientByNHSNumber(nhsNumber); // Search for the patient

            if (patient == null)
            {
                Console.WriteLine("No patient found with the given NHS Number.");
            }

            return patient; // Returns null if no patient is found
        }

        public static DateTime PromptForDate(string promptMessage)
        {
            Console.Write(promptMessage);
            string input = Console.ReadLine();

            DateTime parsedDate;

            // Validate and parse the input
            while (!DateTime.TryParse(input, out parsedDate))
            {
                Console.WriteLine("Invalid date format. Please enter a valid date (e.g., yyyy-MM-dd HH:mm).");
                Console.Write(promptMessage);
                input = Console.ReadLine();
            }

            return parsedDate;
        }

        public List<Patient> DeserializePatients(string json)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                };

                var patients = JsonSerializer.Deserialize<List<Patient>>(json, options);

                return patients;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing patients: {ex.Message}");
                return null;
            }
        }

        internal static void Add(Patient patient)
        {
            throw new NotImplementedException();
        }
    }
}