using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordSystem
{
    internal class Notes
    {
        public DateTime DateCreated { get; set; }
        public string Content { get; set; }

        public List<Notes> note { get; set; } = new List<Notes>();
    }
}
