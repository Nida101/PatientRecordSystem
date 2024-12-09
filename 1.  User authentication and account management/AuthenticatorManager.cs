using PatientRecordSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordSystem
{
    internal class AuthenticatorManager
    {
        static internal User CurrentUser { get; private set; }

        private static PatientRecordDataManager patientRecordDataManager;
        internal static void AttemptLogin(string username, string password)
        {
            patientRecordDataManager = new PatientRecordDataManager();

            foreach (User user in patientRecordDataManager.PatientRecordData.Users)
            {
                if (user.Email == username && user.PasswordHash == EncodePassword(password))
                {
                    CurrentUser = user;
                    return;
                }
            }

            CurrentUser = null;
        }

        internal static void Logout()
        {
            CurrentUser = null;
        }

        internal static string EncodePassword(string password)
        {
            string passwordHash = string.Empty;
            MD5 md5 = MD5.Create();
            SHA512 sha512 = SHA512.Create();

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordHashBytes = md5.ComputeHash(passwordBytes);
            //byte[] passwordHashBytes = sha512.ComputeHash(passwordBytes);

            passwordHash = Convert.ToBase64String(passwordHashBytes);
            return passwordHash;
        }
    }
}
