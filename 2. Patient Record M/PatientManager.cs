using System;
using System.Collections.Generic;
using System.Linq;
using PatientRecordSystem;

namespace PatientRecordSystem
{
    internal static class PatientManager
    {
        public static PatientService patientService = new PatientService();

        public static object notes { get; private set; }

        // Method to view all patients
        public static void ListAllPatients()
        {
            var patients = PatientService.LoadPatients(); // Ensure LoadPatients is accessible

            if (patients.Count == 0)
            {
                Console.WriteLine("No patients found.");
                return;
            }
            foreach (var patient in patients)
            {
                Console.WriteLine($"Name: {patient.FirstName} {patient.LastName}");
                Console.WriteLine($"Date of Birth: {patient.DateOfBirth:yyyy-MM-dd}");
                Console.WriteLine($"NHS Number: {patient.NHSNumber}");
                Console.WriteLine($"Hospital Number: {patient.HospitalNumber}");
                Console.WriteLine(new string('-', 30));
            }
        }


        // Search by DOB and full name
        public static List<Patient> SearchPatientByDobAndName(DateTime dateOfBirth, string fullName)
        {
            var patients = PatientService.LoadPatients();
            return patients.Where(p => p.DateOfBirth == dateOfBirth &&
                                       $"{p.FirstName} {p.LastName}".Equals(fullName, StringComparison.OrdinalIgnoreCase))
                           .ToList();
        }

        // Search by Hospital Number
        public static Patient SearchPatientByHospitalNumber(string hospitalNumber)
        {
            return PatientService.LoadPatients()
                                 .FirstOrDefault(p => p.HospitalNumber.Equals(hospitalNumber, StringComparison.OrdinalIgnoreCase));
        }

        // Search by NHS Number
        public static Patient SearchPatientByNHSNumber(string nhsNumber)
        {
            return PatientService.LoadPatients()
                                 .FirstOrDefault(p => p.NHSNumber.Equals(nhsNumber, StringComparison.OrdinalIgnoreCase));
        }

        // View Patient Details
        public static void ViewUpdatePatientDetails(string nhsNumber)
        {
            var patients = PatientService.LoadPatients();
            var patient = patients.FirstOrDefault(p => p.NHSNumber.Equals(nhsNumber, StringComparison.OrdinalIgnoreCase));

            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            // Display current patient details using a foreach loop
            var patientDetails = new Dictionary<string, string>
        {
            { "First Name", patient.FirstName },
            { "Last Name", patient.LastName },
            { "Date of Birth", patient.DateOfBirth.ToShortDateString() },
            { "Contact Details", patient.ContactDetails },
        };

            Console.WriteLine("Current Patient Details:");
            foreach (var detail in patientDetails)
            {
                Console.WriteLine($"{detail.Key}: {detail.Value}");
            }
            Console.WriteLine();

            // Prompt for updates
            Console.WriteLine("Enter new details (leave blank to keep current value):");

            Console.Write($"First Name (current: {patient.FirstName}): ");
            string firstName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(firstName)) patient.FirstName = firstName;

            Console.Write($"Last Name (current: {patient.LastName}): ");
            string lastName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(lastName)) patient.LastName = lastName;

            Console.Write($"Date of Birth (current: {patient.DateOfBirth.ToShortDateString()}) (YYYY-MM-DD): ");
            string dobInput = Console.ReadLine();
            if (DateTime.TryParse(dobInput, out DateTime newDob)) patient.DateOfBirth = newDob;

            Console.Write($"Contact Details (current: {patient.ContactDetails}): ");
            string contactDetail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(contactDetail)) patient.ContactDetails = contactDetail;

            // Save updated patient details
            patientService.SavePatients(patients);
            Console.WriteLine("Patient details updated successfully.");
        }
    }
}
