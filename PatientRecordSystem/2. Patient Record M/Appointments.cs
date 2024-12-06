using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace PatientRecordSystem
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int DoctorID { get; set; }
        public string PatientNHSNumber { get; set; } // Changed from int PatientID to string PatientNHSNumber
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsCancelled { get; set; } // To track if the appointment is cancelled

        public Appointment(int appointmentId, int doctorId, string patientNHSNumber, DateTime startTime, DateTime endTime)
        {
            AppointmentId = appointmentId;
            DoctorID = doctorId;
            PatientNHSNumber = patientNHSNumber; // Set the NHS number
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
        public bool AddAppointment(int doctorId, string patientNHSNumber, DateTime startTime, DateTime endTime) 
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
            return true;
        }

        // Method to check for scheduling conflicts
        private bool IsConflict(int doctorId, DateTime startTime, DateTime endTime)
        {
            return appointments.Any(a =>
                a.DoctorID == doctorId &&
                ((startTime < a.EndTime && endTime > a.StartTime))); // Check for overlap
        }

        // Method to list all appointments
        public List<Appointment> ListAppointments()
        {
            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments scheduled.");
                return new List<Appointment>(); // Return an empty list
            }

            foreach (var appointment in appointments)
            {
                Console.WriteLine($"Appointment ID: {appointment.AppointmentId}, Doctor ID: {appointment.DoctorID}, Patient NHS Number: {appointment.PatientNHSNumber}, Start: {appointment.StartTime}, End: {appointment.EndTime}, Cancelled: {appointment.IsCancelled}");
            }

            return new List<Appointment>(appointments); // Return a copy of the list
        }



        // Method to cancel an appointment
        public void CancelAppointment(int appointmentId)
        {
            var appointment = appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
            if (appointment == null)
            {
                throw new ArgumentException($"No appointment found with ID {appointmentId}.");
            }

            appointment.IsCancelled = true; // Mark as cancelled
            Console.WriteLine($"Appointment with ID {appointmentId} has been cancelled successfully.");
        }
    }
}
