using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using static PatientRecordSystem.Prescription;

namespace PatientRecordSystem
{
    internal class PatientService
    {
        private const string FilePath = "patients.json"; // Path to the JSON file

        // Load patients from the JSON file
        internal static List<Patient> LoadPatients()
        {
            if (!File.Exists(FilePath))
            {
                return new List<Patient>(); // Return an empty list if the file does not exist
            }

            var jsonData = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<Patient>>(jsonData);
        }

        // Save patients to the JSON file
        internal void SavePatients(List<Patient> patients)
        {
            var jsonData = JsonConvert.SerializeObject(patients, Formatting.Indented);
            File.WriteAllText(FilePath, jsonData);
        }
    }
}
