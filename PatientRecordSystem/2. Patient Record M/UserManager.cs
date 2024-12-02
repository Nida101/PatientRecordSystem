using PatientRecordSystem._2._Patient_Record_M;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PatientRecordSystem
{
    public class UserManager
    {
        private readonly string _jsonFilePath;
        private List<User> _users;
        private string jsonFilePath;

        public UserManager(string jsonFilePath)
        {
            this.jsonFilePath = jsonFilePath;
        }

        private void SaveUsers()
        {
            string jsonString = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_jsonFilePath, jsonString);
        }

        internal static List<User> ListAllUsers(User user)
        {
            var users = UserManager.ListAllUsers(user);
            foreach (var User in users)
            {
                Console.WriteLine($"Email: {user.Email}, UserType: {user.UserType}, IsEnabled: {user.IsEnabled}");
            }
            return new List<User>();

        }

        internal bool AddUser(User newUser, string password)
        {
            if (_users.Any(u => u.Email.Equals(newUser.Email, StringComparison.OrdinalIgnoreCase)))
                return false; // Email must be unique

            _users.Add(newUser);
            SaveUsers();
            return true;
        }

        internal bool UpdateUser(string email, Action<User> updateAction)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return false;

            updateAction(user);
            SaveUsers();
            return true;
        }

        public bool SetUserType(string email, string userType)
        {
            return UpdateUser(email, user => user.UserType = userType);
        }

        public bool EnableUser(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return false;

            user.IsEnabled = true;
            SaveUsers();
            return true;
        }

        public bool DisableUser(string email, string adminEmail)
        {
            if (email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase))
                return false; // Admin cannot disable their own account

            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user == null)
                return false;

            user.IsEnabled = false;
            SaveUsers();
            return true;
        }

        internal static IEnumerable<object> ListAllStaff()
        {
            throw new NotImplementedException();
        }

        internal IEnumerable<User?> ListAllUsers(List<User> addUser)
        {
            throw new NotImplementedException();
        }
    }
}

