using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordSystem
{
    internal class Appointment //2.2
    {
        public DateTime AppointmentDate { get; set; }
        public int AppointmentId { get; set; }
        public string GetAppointments {  get; set; }
        public string DoctorName { get; set; }
        public int DoctorID { get; set; }
        public string Department { get; set; }
        public bool AppointmentCancelled { get; set; }
        public int PatientID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }


        public List<Appointment> Appointments { get; set; } = new List<Appointment>(); //Adding a property for Appointment


        internal static void Add(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        internal static object FirstOrDefault(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }

        internal static void Remove(object appointment)
        {
            throw new NotImplementedException();
        }
    }

    internal class AppointmentScheduler
    {
        internal static List<Appointment> appointments = new List<Appointment>();

        internal static bool ScheduleAppointment(int DoctorID, int PatientID, DateTime startTime, DateTime endTime)
        {
            if (DoctorID <= 0) // ensure a valid doctor is assigned
            {
                throw new ArgumentException("A valid doctor must be assigned to the appointment.");
            }

            if (IsConflict(DoctorID, PatientID, startTime, endTime)) // prevent scheduling conflicts 
            {
                throw new InvalidOperationException("The appointment conflicts with an existing schedule.");
            }

            var appointment = new Appointment // schedule the appointment
            {
                AppointmentId = GeneratedAppointmentID(),
                DoctorID = DoctorID,
                PatientID = PatientID,
                StartTime = startTime,
                EndTime = endTime
            };

            appointments.Add(appointment);
            return true;
        }

        private static bool IsConflict(int DoctorID, int PatientID, DateTime startTime, DateTime endTime)
        {
            return appointments.Any(a =>
                (a.DoctorID == DoctorID || a.PatientID == PatientID) &&
                ((startTime >= a.StartTime && startTime < a.EndTime) ||
                 (endTime > a.StartTime && endTime <= a.EndTime) ||
                 (startTime <= a.StartTime && endTime >= a.EndTime)));
        }


        public static int GeneratedAppointmentID()
        {
            return new Random().Next(1, 10000); // random ID generator
        }

        internal static void ListAppointment()
        {
            if (appointments.Count == 0)
            {
                Console.WriteLine("No appointments scheduled.");
                return;
            }

            foreach (var appointment in appointments)
            {
                Console.WriteLine($"Appointment ID: {appointment.AppointmentId}, Doctor ID: {appointment.DoctorID}, Patient ID: {appointment.PatientID}, Start: {appointment.StartTime}, End: {appointment.EndTime}");
            }
        }

        internal static void AddAppointment(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        internal static void CancelAppointment(int appointmentId) // New method added
        {
            var appointment = appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
            if (appointment == null)
            {
                throw new ArgumentException($"No appointment found with ID {appointmentId}.");
            }

            appointments.Remove(appointment);
            Console.WriteLine($"Appointment with ID {appointmentId} has been canceled successfully.");
        }

        public static List<Appointment> GetAppointments()
        {
            return new List<Appointment>(appointments); // Return a copy of the appointments list
        }
    }
}