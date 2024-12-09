using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PatientRecordSystem
{
    internal class Prescription
    {
        public string NHSNumber { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public DateTime DatePrescribed { get; set; }
        public bool IsActive { get; set; } = true; // Indicates if the prescription is active
    }

    internal class PrescriptionManager
    {
        internal const string FilePath = "prescriptions.json";
        internal List<Patient> patients;

        // Constructor to load patients from the JSON file
        public PrescriptionManager()
        {
            LoadPatients();
        }

        // Method to load patients from the JSON file
        private void LoadPatients()
        {
            if (File.Exists(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                Console.WriteLine("Debug: JSON file loaded.");
                patients = JsonSerializer.Deserialize<List<Patient>>(json) ?? new List<Patient>();
                Console.WriteLine($"Debug: {patients.Count} patients loaded.");
            }
            else
            {
                Console.WriteLine("Debug: JSON file not found, creating new list.");
                patients = new List<Patient>();
                SavePatients();
            }
        }

        // Method to save patients to the JSON file
        private void SavePatients()
        {
            var json = JsonSerializer.Serialize(patients, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        // Method to add a prescription for a patient
        internal void AddPrescription(string nhsNumber, string medication, string dosage)
        {
            var patient = patients.Find(p => p.NHSNumber == nhsNumber);
            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            var newPrescription = new Prescription
            {
                NHSNumber = nhsNumber,
                Medication = medication,
                Dosage = dosage,
                DatePrescribed = DateTime.Now
            };

            patient.Prescriptions.Add(newPrescription);
            Console.WriteLine($"Prescription for {medication} added successfully for patient {patient.FirstName} {patient.LastName}.");
            SavePatients(); // Save changes to the JSON file
        }

        // Method to cancel a prescription for a patient
        internal void CancelPrescription(string nhsNumber, string medication)
        {
            var patient = patients.Find(p => p.NHSNumber == nhsNumber);
            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            var prescription = patient.Prescriptions.FirstOrDefault(p => p.Medication == medication && p.IsActive);
            if (prescription == null)
            {
                Console.WriteLine("Active prescription not found.");
                return;
            }

            prescription.IsActive = false; // Mark the prescription as canceled
            Console.WriteLine($"Prescription for {medication} canceled successfully for patient {patient.FirstName} {patient.LastName}.");
            SavePatients(); // Save changes to the JSON file
        }

        // Method to view and update patient prescriptions
        public void ViewUpdatePatientPrescriptions(string nhsNumber)
        {
            //Console.WriteLine($"Debug: ViewUpdatePatientPrescriptions called with NHSNumber: {nhsNumber}");

            //var patient = patients.Find(p => p.NHSNumber == nhsNumber);
            //List<Patient> patients = new List<Patient>();
            var patient = patients.Find(p => p.NHSNumber == nhsNumber);
            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            // Display current prescriptions
           // Console.WriteLine($"Debug: Patient {patient.FirstName} {patient.LastName} found.");

            Console.WriteLine($"Current Prescriptions for {patient.FirstName} {patient.LastName}:");
            foreach (var prescription in patient.Prescriptions.Where(p => p.IsActive))
            {
                Console.WriteLine($"- Medication: {prescription.Medication}, Dosage: {prescription.Dosage}, Date Prescribed: {prescription.DatePrescribed}");
            }

            Console.WriteLine();

            // Prompt for updates
            while (true)
            {
                Console.WriteLine("Enter the medication name to update or cancel (leave blank to exit):");
                string medicationInput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(medicationInput))
                {
                    Console.WriteLine("Exiting update process.");
                    return; // Exit if no input
                }

                var prescriptionToUpdate = patient.Prescriptions.FirstOrDefault(p => p.Medication.Equals(medicationInput, StringComparison.OrdinalIgnoreCase) && p.IsActive);
                if (prescriptionToUpdate == null)
                {
                    Console.WriteLine("Active prescription not found. Please try again.");
                    continue; // Ask for input again
                }

                Console.WriteLine("Enter new dosage (leave blank to keep current):");
                string newDosage = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newDosage))
                {
                    prescriptionToUpdate.Dosage = newDosage;
                }

                Console.WriteLine("Do you want to cancel this prescription? (yes/no)");
                string cancelInput = Console.ReadLine();
                if (cancelInput?.ToLower() == "yes")
                {
                    prescriptionToUpdate.IsActive = false; // Mark as cancelled
                    Console.WriteLine($"Prescription for {medicationInput} has been cancelled.");
                }

                SavePatients(); // Save changes to the JSON file
                Console.WriteLine("Prescription details updated successfully.");

                // Optionally, you can ask if the user wants to update another prescription
                Console.WriteLine("Do you want to update another prescription? (yes/no)");
                string anotherInput = Console.ReadLine();
                if (anotherInput?.ToLower() != "yes")
                {
                    Console.WriteLine("Exiting update process.");
                    break; // Exit the loop if the user does not want to continue
                }
            }
        }
    }
}
