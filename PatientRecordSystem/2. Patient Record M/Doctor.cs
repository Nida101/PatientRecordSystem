using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordSystem._2._Patient_Record_M
{
    internal class Doctor
    {
        public int DoctorID { get; set; }
        public string Name { get; set; }
    }

    public static class DoctorManager
    {
        // A collection to store doctors
        private static List<Doctor> doctors = new List<Doctor>();

        // Method to add a doctor to the list
        internal static void Add(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new ArgumentNullException(nameof(doctor), "Doctor cannot be null.");
            }

            if (doctors.Any(d => d.DoctorID == doctor.DoctorID))
            {
                Console.WriteLine("Doctor with this ID already exists.");
            }
            else
            {
                doctors.Add(doctor);
                Console.WriteLine($"Doctor {doctor.Name} added successfully.");
            }
        }

        // Method to retrieve the list of doctors
        internal static List<Doctor> GetDoctors()
        {
            return new List<Doctor>(doctors); // Return a copy of the list
        }
    }
}
