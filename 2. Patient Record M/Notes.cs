using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using Newtonsoft.Json;


namespace PatientRecordSystem
{
    public class Notes
    {
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }

        internal const string FilePath = "patients.json";
        internal List<Patient> patients;

        // Constructor to load patients from the JSON file
        public Notes()
        {
            PatientService.LoadPatients();
        }

        // Add Note
        internal static void AddNoteToPatient(string nhsNumber, Notes notes)
        {
            var PatientService = new PatientService();
            var patients = PatientService.LoadPatients();
            var patient = patients.FirstOrDefault(p => p.NHSNumber.Equals(nhsNumber, StringComparison.OrdinalIgnoreCase));

            if (patient == null)
            {
                throw new ArgumentException("Patient not found.");
            }

            string note = "This is a new note.";
            patient.Notes.Add(note);
            PatientService.SavePatients(patients);
            Console.WriteLine("Note added successfully.");
        }

        // Get Notes
        public static List<Notes> GetPatientNotes(string nhsNumber)
        {
            var patientService = new PatientService();
            var patients = PatientService.LoadPatients();
            var patient = patients.FirstOrDefault(p => p.NHSNumber.Equals(nhsNumber, StringComparison.OrdinalIgnoreCase));

            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return null;
            }

            return patient.notes;
        }


        public static void ViewUpdatePatientNotes()
        {
            Console.Write("Enter the NHS Number of the patient: ");
            string nhsNumber = Console.ReadLine();

            var patientService = new PatientService();
            var patients = PatientService.LoadPatients();
            var patient = patients.FirstOrDefault(p => p.NHSNumber.Equals(nhsNumber, StringComparison.OrdinalIgnoreCase));

            if (patient != null)
            {
                // Display current patient notes
                Console.WriteLine("Current Patient Notes:");

                if (patient.Notes != null && patient.Notes.Any())
                {
                    foreach (var note in patient.Notes)
                    {
                        Console.WriteLine($"- {note}");
                    }
                }
                else
                {
                    Console.WriteLine("No notes available for this patient.");
                }

                // Prompt for updates
                Console.WriteLine("Enter new note (leave blank to keep current notes):");
                string newNoteContent = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(newNoteContent))
                {
                    patient.Notes.Add(newNoteContent); // Add the new note directly to the patient
                }

                // Save updated patient details
                patientService.SavePatients(patients); // Save the updated list of patients
                Console.WriteLine("Patient notes updated successfully.");
            }
            else
            {
                Console.WriteLine("Patient not found.");
            }
        }
    }
}
