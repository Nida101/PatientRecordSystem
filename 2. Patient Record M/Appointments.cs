using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace PatientRecordSystem
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int DoctorID { get; set; }
        public string PatientNHSNumber { get; set; } 
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsCancelled { get; set; } 

        public Appointment(int appointmentId, int doctorId, string patientNHSNumber, DateTime startTime, DateTime endTime)
        {
            AppointmentId = appointmentId;
            DoctorID = doctorId;
            PatientNHSNumber = patientNHSNumber; 
            StartTime = startTime;
            EndTime = endTime;
            IsCancelled = false; 
        }
    }

    internal class AppointmentManager
    {
        private List<Appointment> appointments = new List<Appointment>();
        private int appointmentCounter = 0; // Counter for generating unique appointment IDs

        // Method to add a new appointment
        public bool AddAppointment(int doctorId, string patientNHSNumber, DateTime startTime, DateTime endTime, string filePath) 
        {
            // Validate inputs
            if (doctorId <= 0 || string.IsNullOrWhiteSpace(patientNHSNumber))
            {
                throw new ArgumentException("Doctor ID must be valid and Patient NHS Number cannot be empty.");
            }

            if (startTime >= endTime)
            {
                throw new ArgumentException("Start time must be before end time.");
            }

            if (IsConflict(doctorId, startTime, endTime))
            {
                throw new InvalidOperationException("The appointment conflicts with an existing schedule.");
            }

            // Create a new appointment
            var appointment = new Appointment(++appointmentCounter, doctorId, patientNHSNumber, startTime, endTime);
            appointments.Add(appointment);

            // Save the updated appointments to the JSON file
            SaveAppointmentsToFile(filePath);

            return true;
        }

        // Method to check for scheduling conflicts
        private bool IsConflict(int doctorId, DateTime startTime, DateTime endTime)
        {
            // Check if appointments list is null
            if (appointments == null)
            {
                throw new InvalidOperationException("Appointments list is not initialized.");
            }

            return appointments.Any(a =>
                a.DoctorID == doctorId &&
                a.StartTime != null && a.EndTime != null && // Ensure StartTime and EndTime are not null
                (startTime < a.EndTime && endTime > a.StartTime)); // Check for overlap
        }

        // Method to list all appointments
        public List<Appointment> ListAppointments(string filePath)
        {
            // Load Appointment from the JSON file
            LoadAppointmentsFromFile(filePath);

            return new List<Appointment>(appointments); // Return a copy of the list
        }



        // Method to cancel an appointment
        public void CancelAppointment(int appointmentId, string filePath)
        {
            var appointment = appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
            if (appointment == null)
            {
                throw new ArgumentException($"No appointment found with ID {appointmentId}.");
            }

            appointment.IsCancelled = true; // Mark as cancelled
            Console.WriteLine($"Appointment with ID {appointmentId} has been cancelled successfully.");

            // Save the updated appointments to the JSON file
            SaveAppointmentsToFile(filePath);
        }

        // Method to save appointments to JSON file
        public void SaveAppointmentsToFile(string filePath)
        {
            var json = JsonConvert.SerializeObject(appointments, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public void LoadAppointmentsFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var loadedAppointments = JsonConvert.DeserializeObject<List<Appointment>>(json);

                // Validate loaded appointments
                if (loadedAppointments != null)
                {
                    appointments = loadedAppointments.Where(a => a.StartTime != default && a.EndTime != default).ToList();
                }
                else
                {
                    Console.WriteLine("No valid appointments found in the file.");
                }
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }

        public static void ViewUpdateAppointmentDetails(int appointmentId, string filePath)
        {
            var appointmentManager = new AppointmentManager();
            var appointments = appointmentManager.ListAppointments(filePath);
            var appointment = appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                Console.WriteLine("Appointment not found.");
                return;
            }

            // Display current appointment details
            Console.WriteLine("Current Appointment Details:");
            Console.WriteLine($"Appointment ID: {appointment.AppointmentId}");
            Console.WriteLine($"Doctor ID: {appointment.DoctorID}");
            Console.WriteLine($"Patient NHS Number: {appointment.PatientNHSNumber}");
            Console.WriteLine($"Start Time: {appointment.StartTime}");
            Console.WriteLine($"End Time: {appointment.EndTime}");
            Console.WriteLine($"Is Cancelled: {appointment.IsCancelled}");
            Console.WriteLine();

            // Prompt for updates
            Console.WriteLine("Enter new details (leave blank to keep current value):");

            Console.Write($"Doctor ID (current: {appointment.DoctorID}): ");
            string doctorIdInput = Console.ReadLine();
            if (int.TryParse(doctorIdInput, out int newDoctorId) && newDoctorId > 0)
            {
                appointment.DoctorID = newDoctorId;
            }

            Console.Write($"Patient NHS Number (current: {appointment.PatientNHSNumber}): ");
            string patientNHSNumber = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(patientNHSNumber))
            {
                appointment.PatientNHSNumber = patientNHSNumber;
            }

            Console.Write($"Start Time (current: {appointment.StartTime}): ");
            string startTimeInput = Console.ReadLine();
            if (DateTime.TryParse(startTimeInput, out DateTime newStartTime))
            {
                appointment.StartTime = newStartTime;
            }

            Console.Write($"End Time (current: {appointment.EndTime}): ");
            string endTimeInput = Console.ReadLine();
            if (DateTime.TryParse(endTimeInput, out DateTime newEndTime))
            {
                appointment.EndTime = newEndTime;
            }

            // Save updated appointment details
            appointmentManager.SaveAppointmentsToFile(filePath); // Save the updated list of appointments
            Console.WriteLine("Appointment details updated successfully.");
        }
    }
}
