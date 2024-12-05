using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PatientRecordSystem
{
    internal class PatientService
    {
        internal readonly List<Patient> patients;
        internal readonly Dictionary<string, int> dailyPatientCount = new Dictionary<string, int>();
        internal static string FilePath = ("patients.json");



        public PatientService()
        {
            patients = LoadPatients();
        }

        public Patient CreatePatient(string firstName, string lastName, DateTime dateOfBirth, string contactDetails, string nhsNumber, string hospitalNumber = null)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(contactDetails) || string.IsNullOrWhiteSpace(nhsNumber))
            {
                throw new ArgumentException("All fields except hospitalNumber are mandatory.");
            }

            hospitalNumber ??= GenerateHospitalNumber();

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
            SavePatients();
            return newPatient;
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
            return $"PRS-{today}-{sequenceNumber}";
        }


        internal List<Patient> LoadPatients()
        {
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "[]");
                return new List<Patient>();
            }

            try
            {
                string jsonData = File.ReadAllText(FilePath);
                Console.WriteLine($"JSON Data from File: {jsonData}"); // Debug output
                return JsonSerializer.Deserialize<List<Patient>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Patient>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error deserializing patients from file: {ex.Message}");
                return new List<Patient>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading patients from file: {ex.Message}");
                return new List<Patient>();
            }
        }



        internal void SavePatients()
        {
            try
            {
                string jsonData = JsonSerializer.Serialize(patients, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving patients: {ex.Message}");
            }
        }
    }
}