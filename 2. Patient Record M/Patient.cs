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
        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public List<string> Notes { get; set; } = new List<string>();


        public List<string> note { get; set; } = new List<string>();
        internal List<Notes> notes;

        public Patient()
        {
            Notes = new List<string>();
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
    
        internal class AddPatient
        {
            private const string FilePath = "patients.json";

            // Method to add a new patient
            public void AddNewPatient(string firstName, string lastName, DateTime dateOfBirth, string contactDetails, string nhsNumber)
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                    string.IsNullOrWhiteSpace(contactDetails) || string.IsNullOrWhiteSpace(nhsNumber))
                {
                    throw new ArgumentException("All fields must be filled out.");
                }

                // Load existing patients
                List<Patient> patients = LoadPatients();

                // Check if the patient already exists
                if (patients.Any(p => p.NHSNumber == nhsNumber))
                {
                    Console.WriteLine("A patient with this NHS Number already exists.");
                    return;
                }

                // Generate hospital number
                string hospitalNumber = GenerateHospitalNumber(patients);

                // Create a new patient
                var newPatient = new Patient
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    ContactDetails = contactDetails,
                    NHSNumber = nhsNumber,
                    HospitalNumber = hospitalNumber
                };

                // Add the new patient to the list
                patients.Add(newPatient);
                Console.WriteLine($"Patient {firstName} {lastName} added successfully with Hospital Number: {hospitalNumber}");

                // Save updated patient list
                SavePatients(patients);
            }

            // Method to generate a unique hospital number
            private string GenerateHospitalNumber(List<Patient> patients)
            {
                string dateAdded = DateTime.Now.ToString("ddMMyy");
                int sequenceNumber = patients.Count(p => p.HospitalNumber.StartsWith($"PRS-{dateAdded}")) + 1;
                return $"PRS-{dateAdded}-{sequenceNumber:D2}"; // Format: PRS-DDMMYY-XX
            }

            // Method to load patients from the JSON file - appointment.json
            private List<Patient> LoadPatients()
            {
                if (File.Exists(FilePath))
                {
                    var json = File.ReadAllText(FilePath);
                    return JsonSerializer.Deserialize<List<Patient>>(json) ?? new List<Patient>();
                }
                return new List<Patient>();
            }

            // Method to save patients to the JSON file
            private void SavePatients(List<Patient> patients)
            {
                var json = JsonSerializer.Serialize(patients, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, json);
            }
        }
    }
}
