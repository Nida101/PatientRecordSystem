using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordSystem
{
    internal class PatientManager
    {
        private static List<Patient> patients = new List<Patient>();
        private static readonly PatientService patientService = new PatientService();

        // Method to view all patients
        public static void ListAllPatients()
        {
            var patients = patientService.LoadPatients();

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



        public static List<Patient> SearchPatientByDobAndName(DateTime dateOfBirth, string fullName)
        {
            var patients = patientService.LoadPatients();
            return patients.Where(p => p.DateOfBirth == dateOfBirth && $"{p.FirstName} {p.LastName}".Equals(fullName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static Patient SearchPatientByHospitalNumber(string hospitalNumber) //Method to search patient by Hospital Number
        {
            var patients = patientService.LoadPatients();
            return patients.FirstOrDefault(p => p.HospitalNumber.Equals(hospitalNumber, StringComparison.OrdinalIgnoreCase));
        }

        public static Patient SearchPatientByNHSNumber(string nhsNumber) //Method to search patient by NHS Number
        {
            var patients = patientService.LoadPatients();
            return patients.FirstOrDefault(p => p.NHSNumber.Equals(nhsNumber, StringComparison.OrdinalIgnoreCase));
        }

        public static Patient SearchPatientByNHSOrHospitalNumber(string identifier) //Method to search a patient using either NHS Number or Hospital Number
        {
            var patientByHospitalNumber = SearchPatientByHospitalNumber(identifier);
            if (patientByHospitalNumber != null)
                return patientByHospitalNumber;

            var patientByNHSNumber = SearchPatientByNHSNumber(identifier);
            return patientByNHSNumber;
        }

        internal static void ViewPatientDetails(Patient patient, List<Appointment> appointments)
        {
            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            Console.WriteLine($"Name: {patient.FirstName} {patient.LastName}");
            Console.WriteLine($"Date of Birth: {patient.DateOfBirth.ToShortDateString()}");
            Console.WriteLine($"Contact Detail: {patient.ContactDetails}");
            Console.WriteLine($"NHS Number: {patient.NHSNumber}");
            Console.WriteLine($"Hospital Number:{patient.HospitalNumber}");
            Console.WriteLine("Appointments: ");
            foreach (var appointment in appointments)
            {
                Console.WriteLine($" Date: {appointment.AppointmentDate}");
                Console.WriteLine($" Doctor: {appointment.DoctorName}");
                Console.WriteLine($" Department: {appointment.Department}");
                Console.WriteLine();
            }
        }

        public static void UpdatePatientDetails(string firstName, string lastName, DateTime dateOfBirth, string contactDetail, string nhsNumber, string hospitalNumber)
        {
            var patient = SearchPatientByNHSNumber(nhsNumber);
            if (patient != null)
            {
                patient.FirstName = firstName;
                patient.LastName = lastName;
                patient.DateOfBirth = dateOfBirth;
                patient.ContactDetails = contactDetail;
                patient.NHSNumber = nhsNumber;
                patient.HospitalNumber = hospitalNumber;
            }
        }

        public static void AddAppointment(string nhsNumber, Appointment appointment)
        {
            var patient = SearchPatientByNHSNumber(nhsNumber);
            if (patient != null)
            {
                Appointment.Add(appointment);
            }
        }

        internal static void CancelAppointment(int appointmentId, List<Appointment> appointments) 
        {
            var appointment = appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
            if (appointment == null)
            {
                throw new ArgumentException($"No appointment found with ID {appointmentId}.");
            }

            appointments.Remove(appointment); 
            Console.WriteLine($"Appointment with ID {appointmentId} has been canceled successfully.");
        }


        public static void AddPrescription(string nhsNumber, Prescription prescription)
        {
            var patient = SearchPatientByNHSNumber(nhsNumber);
            if (patient != null)
            {
                Prescription.Add(prescription);
            }
        }

        public static void AddNote(string nhsNumber, Notes note)
        {
            var patient = patients.FirstOrDefault(p => p.NHSNumber == nhsNumber);

            if (patient == null)
            {
                throw new ArgumentException("Patient not found.");
            }

            patient.Note.Add(note);
            Console.WriteLine("Note added successfully.");
        }

        public static List<Notes> GetPatientNotes(string nhsNumber)
        {
            var patient = patients.FirstOrDefault(p => p.NHSNumber.Equals(nhsNumber, StringComparison.OrdinalIgnoreCase));

            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return null; // Or throw a custom exception
            }

            return PatientManager.GetPatientNotes(nhsNumber);
        }

    }

}
