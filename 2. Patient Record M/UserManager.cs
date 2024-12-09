using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PatientRecordSystem
{
    public class UserManager
    {
        private readonly string _jsonFilePath;
        private List<User> _users;

        public UserManager(string jsonFilePath)
        {
            _jsonFilePath = jsonFilePath;
            _users = LoadUsers();
        }

        private List<User> LoadUsers()
        {
            try
            {
                string jsonString = File.ReadAllText(_jsonFilePath);
                return JsonSerializer.Deserialize<List<User>>(jsonString);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The file {_jsonFilePath} was not found.");
                return new List<User>();
            }
            catch (JsonException)
            {
                Console.WriteLine("Error: The file is not a valid JSON.");
                return new List<User>();
            }
        }

        internal bool AddOrUpdateUser(User user, string password)
        {
            string hashedPassword = AuthenticatorManager.EncodePassword(password);

            // Check for unique email
            if (_users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                // Update existing user
                var existingUser = _users.First(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));
                existingUser.PasswordHash = hashedPassword; // Update password with hashed value
                existingUser.UserType = user.UserType;
                existingUser.IsEnabled = true; // Enable user by default on update
                SaveUsers();
                return true; // User updated
            }
            else
            {
                // Add new user
                user.PasswordHash = hashedPassword; // Set password
                user.IsEnabled = true; // Enable user by default on add
                _users.Add(user);
                SaveUsers();
                return true; // User added
            }
        }

        internal void SaveUsers()
        {
            string jsonString = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_jsonFilePath, jsonString);
        }
    
        public void ListAllUsers()
        {
            Console.WriteLine("All users:");
            foreach (var user in _users)
            {
                Console.WriteLine($"Email: {user.Email}, UserType: {user.UserType}, IsEnabled: {user.IsEnabled}");
            }
        }

        internal bool ChangeUserStatus(string email, string adminEmail, bool enable)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            var admin = _users.FirstOrDefault(u => u.Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase));

            if (admin == null || IsAdmin(adminEmail))
            {
                Console.WriteLine("Only admins can change user status.");
                return false;
            }

            if (user == null)
            {
                Console.WriteLine("User  does not exist.");
                return false;
            }

            if (enable)
            {
                user.IsEnabled = true;
                Console.WriteLine($"User  {email} has been enabled.");
            }
            else
            {
                user.IsEnabled = false;
                Console.WriteLine($"User  {email} has been disabled.");
            }

            return true;
        }

        private bool IsAdmin(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return user != null && user.UserType.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }
    }
}
