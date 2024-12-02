using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PatientRecordSystem
{
    internal class PatientService
    {
        private readonly Dictionary<string, int> dailyPatientCount = new Dictionary<string, int>(); //2.1 
        string patient = LoadPatient().ToString();
        private List<Patient> patients = LoadPatient();

        public Patient CreatePatient(string firstName, string lastName, DateTime dateOfBirth, string contactDetails, string nhsNumber, string hospitalNumber)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(contactDetails) || string.IsNullOrWhiteSpace(nhsNumber) || string.IsNullOrWhiteSpace(hospitalNumber))
            {
                throw new ArgumentException("All fields are mandatory.");
            }
            if (string.IsNullOrWhiteSpace(hospitalNumber))
            {
                hospitalNumber = GenerateHospitalNumber();
            }

            {
                var newPatient = new Patient
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    ContactDetails = contactDetails,
                    NHSNumber = nhsNumber,
                    HospitalNumber = hospitalNumber
                };

                patients.Add(newPatient);
                Console.WriteLine(patients);
                SavePatients(patients);
                Console.WriteLine(patients);

                return newPatient;
            }
        }

        public string GenerateHospitalNumber()
        {
            string today = DateTime.Now.ToString("ddMMyy");

            if (!dailyPatientCount.ContainsKey(today))
            {
                dailyPatientCount[today] = 1;
            }
            else
            {
                dailyPatientCount[today]++;
            }

            string sequenceNumber = dailyPatientCount[today].ToString("D2");
            return ($"PRS-{today}-{sequenceNumber}");
        }

        // JSON for 2.1 - storing the patient record
        private static string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "patients.json");

        // Load existing patients from JSON
        public List<Patient> LoadPatients()
        {
            if (!File.Exists(FilePath))
            {
                // If the file doesn't exist, create it and return an empty list
                File.WriteAllText(FilePath, "[]");
                return new List<Patient>();
            }

            string jsonData = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Patient>>(jsonData) ?? new List<Patient>();
        }

        // Save patients to JSON
        public void SavePatients(List<Patient> patients)
        {
            if (!File.Exists(FilePath))
            {
                // If the file doesn't exist, create it and return an empty list
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                File.WriteAllText(FilePath, "[]");
            }
            string jsonData = JsonSerializer.Serialize(patients, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, jsonData);
        }

        // Create a new patient record
        public Patient JSONCreatePatient(string firstName, string lastName, DateTime dateOfBirth, string contactDetails, string nhsNumber, string hospitalNumber)
        {
            var patients = LoadPatients();

            var newPatient = new Patient
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                ContactDetails = contactDetails,
                NHSNumber = nhsNumber,
                HospitalNumber = hospitalNumber
            };

            patients.Add(newPatient);
            SavePatients(patients);

            return newPatient;
        }

        private const string FileNamePath = "patients.json";

        public static List<Patient> LoadPatient()
        {
            if (!System.IO.File.Exists(FilePath))
            {
                System.IO.File.WriteAllText(FilePath, "[]");
            }

            string json = System.IO.File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Patient>>(json) ?? new List<Patient>();
        }

        public static void SavePatient(List<Patient> patients)
        {
            string json = JsonSerializer.Serialize(patients, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(FilePath, json);
        }

        public void AddNoteToPatient(string nhsNumber, Notes newNote)
        {
            // Load existing patients
            var patients = LoadPatients();

            // Find the patient by NHS Number
            var patient = patients.FirstOrDefault(p => p.NHSNumber == nhsNumber);
            if (patient == null)
            {
                throw new Exception("Patient not found.");
            }

            // Add the note to the patient's notes list
            patient.Note.Add(newNote);
        }
    }
}
