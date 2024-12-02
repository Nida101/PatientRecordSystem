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
            //InitUsers();

            //SaveUsers();
            // InitPatients()
        }

        //private void InitUsers()
        //{
        //    this.PatientRecordData.Users = new List<User>();
        //    this.PatientRecordData.Users.Add(new User("James Adam", "doctor1@hospital.com", "password1", Role.Doctor));
        //    this.PatientRecordData.Users.Add(new User("James Ben", "admin1@hospital.com", "password2", Role.Admin));
        //    this.PatientRecordData.Users.Add(new User("Ken Ben", "nurse1@hospital.com", "password3", Role.Nurse));
        //}

        internal void SaveAll()
        {
            SaveUsers();
            // Save more
        }

        internal void LoadAll()
        {
            LoadUsers();
            // Load others
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

