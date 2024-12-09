using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PatientRecordSystem
{
    internal class PatientRecordDataManager
    {
        private const string userJsonFilePath = "users.json";
        internal PatientRecordData PatientRecordData { get; set; }


        public PatientRecordDataManager()
        {
            PatientRecordData = new PatientRecordData();
            LoadUsers();
        }

        internal void SaveAll()
        {
            SaveUsers();
        }

        internal void LoadAll()
        {
            LoadUsers();
        }

        internal void SaveUsers()
        {
            string userJson = JsonConvert.SerializeObject(this.PatientRecordData.Users);
            File.WriteAllText(userJsonFilePath, userJson);
        }

        internal void LoadUsers()
        {
            string userJson = File.ReadAllText(userJsonFilePath);
            this.PatientRecordData.Users = JsonConvert.DeserializeObject<List<User>>(userJson).ToList();
        }
    }
}

