using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PatientRecordSystem
{
    enum Role
    {
        Admin,
        Doctor,
        Nurse
    }

    internal class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; }
        public string UserType { get; set; } = "Standard"; // Admin, Doctor, Nurse, etc.
        public bool IsEnabled { get; set; } = true; // Account active or disabled

        public User()
        {

        }

        public User(string name, string email, string password, Role role)
        {
            Name = name;
            Email = email;
            PasswordHash = AuthenticatorManager.EncodePassword(password);
            Role = role;
        }
    }

    public class UserService
    {
        private const string FilePath = "users.json";

        private List<User> LoadUsers() // Load all users from the JSON file
        {
            if (!File.Exists(FilePath))
            {
                return new List<User>();
            }

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private void SaveUsers(List<User> users) // Save all users to the JSON file
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public void AddUser(string adminEmail, string email, string password, string userType) // Add a new user (Admin only) - could be deleted
        {
            var users = LoadUsers();
            var newUser = new User(adminEmail, email, password, Role.Admin);

            if (users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase))) // Ensure unique email
            {
                throw new Exception("A user with this email already exists.");
            }
            users.Add(newUser);
            SaveUsers(users);
        }

        public void UpdateUser(string adminEmail, string targetEmail, string? newPassword = null, string? newUserType = null, bool? enableAccount = null) // could be deleted
        {
            var users = LoadUsers();

            var admin = users.FirstOrDefault(u => u.Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) && u.UserType == "Admin");
            if (admin == null)
            {
                throw new Exception("You do not have permission to update users.");
            }

            var user = users.FirstOrDefault(u => u.Email.Equals(targetEmail, StringComparison.OrdinalIgnoreCase));
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (enableAccount.HasValue && !enableAccount.Value && adminEmail.Equals(targetEmail, StringComparison.OrdinalIgnoreCase)) // Prevent admin from disabling their own account
            {
                throw new Exception("You cannot disable your own account.");
            }

            if (!string.IsNullOrEmpty(newUserType)) // Update user type
            {
                user.UserType = newUserType;
            }

            if (enableAccount.HasValue) // Update enable/disable status
            {
                user.IsEnabled = enableAccount.Value;
            }

            SaveUsers(users);
        }

        internal List<User> ListAllUsers(string adminEmail) // could be deleted
        {
            var users = LoadUsers();

            var admin = users.FirstOrDefault(u => u.Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) && u.UserType == "Admin"); // Verify admin permissions
            if (admin == null)
            {
                throw new Exception("You do not have permission to view users.");
            }

            return users;
        }
    }
}
