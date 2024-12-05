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
            var patients = patientService.LoadPatients(); // Ensure LoadPatients is accessible

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
            var patients = patientService.LoadPatients();
            return patients.Where(p => p.DateOfBirth == dateOfBirth &&
                                       $"{p.FirstName} {p.LastName}".Equals(fullName, StringComparison.OrdinalIgnoreCase))
                           .ToList();
        }

        // Search by Hospital Number
        public static Patient SearchPatientByHospitalNumber(string hospitalNumber)
        {
            return patientService.LoadPatients()
                                 .FirstOrDefault(p => p.HospitalNumber.Equals(hospitalNumber, StringComparison.OrdinalIgnoreCase));
        }

        // Search by NHS Number
        public static Patient SearchPatientByNHSNumber(string nhsNumber)
        {
            return patientService.LoadPatients()
                                 .FirstOrDefault(p => p.NHSNumber.Equals(nhsNumber, StringComparison.OrdinalIgnoreCase));
        }

        // Search by either NHS Number or Hospital Number
        public static Patient SearchPatientByNHSOrHospitalNumber(string identifier)
        {
            return SearchPatientByHospitalNumber(identifier) ?? SearchPatientByNHSNumber(identifier);
        }

        // View Patient Details
        internal static void ViewPatientDetails(Patient patient, List<Appointment> appointments)
        {
            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            Console.WriteLine($"Name: {patient.FirstName} {patient.LastName}");
            Console.WriteLine($"Date of Birth: {patient.DateOfBirth:yyyy-MM-dd}");
            Console.WriteLine($"Contact Details: {patient.ContactDetails}");
            Console.WriteLine($"NHS Number: {patient.NHSNumber}");
            Console.WriteLine($"Hospital Number: {patient.HospitalNumber}");

            Console.WriteLine("Appointments:");
            foreach (var appointment in appointments)
            {
                Console.WriteLine($" - Date: {appointment.AppointmentDate}");
                Console.WriteLine($" - Doctor: {appointment.DoctorName}");
                Console.WriteLine($" - Department: {appointment.Department}");
                Console.WriteLine();
            }
        }

        // Update Patient Details
        public static void UpdatePatientDetails(string nhsNumber, string firstName, string lastName, DateTime dateOfBirth, string contactDetail, string hospitalNumber)
        {
            var patients = patientService.LoadPatients();
            var patient = patients.FirstOrDefault(p => p.NHSNumber.Equals(nhsNumber, StringComparison.OrdinalIgnoreCase));

            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            patient.FirstName = firstName;
            patient.LastName = lastName;
            patient.DateOfBirth = dateOfBirth;
            patient.ContactDetails = contactDetail;
            patient.HospitalNumber = hospitalNumber;

            patientService.SavePatients();
            Console.WriteLine("Patient details updated successfully.");
        }

        // Add Note
        internal static void AddNoteToPatient(string nhsNumber, Notes note)
        {
            var patients = patientService.LoadPatients();
            var patient = patients.FirstOrDefault(p => p.NHSNumber.Equals(nhsNumber, StringComparison.OrdinalIgnoreCase));

            if (patient == null)
            {
                throw new ArgumentException("Patient not found.");
            }

            patient.note.Add(note);
            patientService.SavePatients();
            Console.WriteLine("Note added successfully.");
        }

        // Get Notes
        public static List<Notes> GetPatientNotes(string nhsNumber)
        {
            var patients = patientService.LoadPatients();
            var patient = patients.FirstOrDefault(p => p.NHSNumber.Equals(nhsNumber, StringComparison.OrdinalIgnoreCase));

            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return null;
            }

            return patient.note;
        }
    }
}
