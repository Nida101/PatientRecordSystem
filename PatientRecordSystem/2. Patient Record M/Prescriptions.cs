using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordSystem
{
    internal class Prescription
    {
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public DateTime DatePrescribed { get; set; }

        public List<Prescription> Prescriptions { get; set; } = new List<Prescription>();

        internal static void Add(Prescription prescription)
        {
            throw new NotImplementedException();
        }
    }
}
