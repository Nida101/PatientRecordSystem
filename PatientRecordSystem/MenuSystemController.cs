using PatientRecordSystem;
using PatientRecordSystem._2._Patient_Record_M;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PatientRecordSystem
{
    internal class MenuSystemController
    {
        private static IEnumerable<object> users;
        //var appointmentManager = new AppointmentManager();

        public MenuSystemController()
        {
            if (AuthenticatorManager.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                throw new Exception("Access restricted");
            }
        }

        internal string GetMenuResponse()
        {
            string response = string.Empty;
            switch (AuthenticatorManager.CurrentUser.Role)
            {
                case Role.Admin:
                    response = GetAdminMenuResponse();
                    break;
                case Role.Nurse:
                    response = GetNurseMenuResponse();
                    break;
                case Role.Doctor:
                    response = GetDoctorMenuResponse();
                    break;
            }

            return response;
        }

        internal string GetNurseMenuResponse()
        {
            string response = string.Empty;

            Console.WriteLine("NURSE MAIN MENU");
            Console.WriteLine("1. List All Patients");
            Console.WriteLine("2. Searh Patient");
            Console.WriteLine("3. View Full Patient Record");
            Console.WriteLine("4. Manage Appointment");
            Console.WriteLine("5. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            NurseMenu(choice);
            return choice;

            static void NurseMenu(string choice)
            {
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("LIST PATIENTS");
                        Console.WriteLine(new string('-', 50));
                        PatientManager.ListAllPatients();
                        break;


                    case "2":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("SEARCH PATIENT");
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("\nSearch from the following: ");
                        Console.WriteLine("1. Full Name and Date of Birth");
                        Console.WriteLine("2. NHS Number");
                        Console.WriteLine("3. Hospital Number");
                        Console.Write("Enter your choice: ");
                        string searchChoice = Console.ReadLine();

                        switch (searchChoice)
                        {
                            case "1":
                                Console.Write("Enter Full Name (FirstName LastName): ");
                                string fullName = Console.ReadLine();

                                Console.Write("Enter Date of Birth (yyyy-mm-dd): ");
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime dob))
                                {
                                    Console.WriteLine("Invalid date format.");
                                    break;
                                }

                                var foundPatients = PatientManager.SearchPatientByDobAndName(dob, fullName);

                                if (foundPatients.Any())
                                {
                                    foreach (var patient in foundPatients)
                                    {
                                        DisplayPatientInfo(patient);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No patient found with the given details.");
                                }
                                break;

                            case "2":
                                Console.Write("Enter NHS Number: ");
                                string nhsNum = Console.ReadLine();

                                var patientByNHS = PatientManager.SearchPatientByNHSNumber(nhsNum);

                                if (patientByNHS != null)
                                {
                                    DisplayPatientInfo(patientByNHS);
                                }
                                else
                                {
                                    Console.WriteLine("No patient found with the given NHS Number.");
                                }
                                break;

                            case "3":
                                Console.Write("Enter Hospital Number: ");
                                string hospitalNum = Console.ReadLine();

                                var patientByHospital = PatientManager.SearchPatientByHospitalNumber(hospitalNum);

                                if (patientByHospital != null)
                                {
                                    DisplayPatientInfo(patientByHospital);
                                }
                                else
                                {
                                    Console.WriteLine("No patient found with the given Hospital Number.");
                                }
                                break;

                            default:
                                Console.WriteLine("Invalid Choice - Please select a valid option.");
                                break;
                        }
                        break;

                    case "3":
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine(new string('-', 50));
                            Console.WriteLine("PATIENT RECORD");
                            Console.WriteLine(new string('-', 50));
                            Console.WriteLine("\nSelect an option:");
                            Console.WriteLine("1. Update Patient Basic Details");
                            Console.WriteLine("2. Patient Appointment Details");
                            Console.WriteLine("3. Patient Prescriptions");
                            Console.WriteLine("4. View Patient Notes");
                            Console.WriteLine("5. Exit to Main Menu");
                            Console.Write("Enter your choice: ");
                            string select = Console.ReadLine();

                            switch (select)
                            {
                                case "1":
                                    Console.WriteLine("Incomplete");
                                    break;

                                case "2":
                                    Console.WriteLine("Incomplete");
                                    break;

                                case "3":
                                    Console.WriteLine("Incomplete");
                                    break;

                                case "4":
                                    Console.WriteLine("Patient Notes: ");
                                    var note = new Notes
                                    {
                                        DateCreated = DateTime.Now
                                    };
                                    Console.WriteLine($"Date: {note.DateCreated}");

                                    var nhsForViewNotes = PromptForNHSNumber();
                                    var notesList = PatientManager.GetPatientNotes(nhsForViewNotes);
                                    if (notesList != null && notesList.Any())
                                    {
                                        foreach (var Note in notesList)
                                        {
                                            Console.WriteLine($"Date: {note.DateCreated}, Content: {note.Content}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("No notes found or patient does not exist.");
                                    }
                                    break;

                                case "5":
                                    Console.WriteLine("Exiting Doctor Menu.");
                                    Environment.Exit(0);
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Try again.");
                                    break;
                            }
                        }
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("MANAGE APPOINTMENTS");
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("1. Add Doctor");
                        Console.WriteLine("2. Add Patient");
                        Console.WriteLine("3. Schedule Appointment");
                        Console.WriteLine("4. List Appointments");
                        Console.WriteLine("5. Cancel Appointment");
                        Console.WriteLine("6. Exit");
                        Console.Write("Select an option: ");

                        string appointmentChoice = Console.ReadLine();
                        var appointmentManager = new AppointmentManager();

                        switch (appointmentChoice)
                        {
                            case "1": // Add Doctor
                                Console.Write("Enter Doctor ID: ");
                                if (int.TryParse(Console.ReadLine(), out int doctorId))
                                {
                                    Console.Write("Enter Doctor Name: ");
                                    string name = Console.ReadLine();
                                    DoctorManager.Add(new Doctor { DoctorID = doctorId, Name = name });
                                    Console.WriteLine("Doctor added successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid ID. Doctor not added.");
                                }
                                Console.WriteLine("Press any key to return to the menu.");
                                Console.ReadKey();
                                break;

                            case "2": // Add Patient
                                Console.Write("INCOMPLETE ");
                                
                                break;

                            case "3": // Schedule Appointment
                                Console.Write("Enter Doctor ID: ");
                                while (!int.TryParse(Console.ReadLine(), out doctorId))
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid Doctor ID.");
                                }

                                Console.Write("Enter Patient NHS Number: ");
                                string patientNHSNumber; // Change to string
                                patientNHSNumber = Console.ReadLine(); // Read directly as string
                                while (string.IsNullOrWhiteSpace(patientNHSNumber)) // Check for empty input
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid Patient NHS Number.");
                                    patientNHSNumber = Console.ReadLine();
                                }

                                Console.Write("Enter Start Time (yyyy-MM-dd HH:mm): ");
                                DateTime startTime;
                                while (!DateTime.TryParse(Console.ReadLine(), out startTime))
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid date and time.");
                                }

                                Console.Write("Enter End Time (yyyy-MM-dd HH:mm): ");
                                DateTime endTime;
                                while (!DateTime.TryParse(Console.ReadLine(), out endTime))
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid date and time.");
                                }

                                appointmentManager.AddAppointment(doctorId, patientNHSNumber, startTime, endTime);
                                Console.WriteLine("Appointment scheduled successfully.");
                                break;

                            case "4": // List Appointments
                                var AppointmentManager = new AppointmentManager();
                                var appointments = AppointmentManager.ListAppointments();
                                if (appointments.Any())
                                {
                                    Console.WriteLine("Scheduled Appointments:");
                                    foreach (var appointment in appointments)
                                    {
                                        Console.WriteLine($"Appointment ID: {appointment.AppointmentId}, Doctor ID: {appointment.DoctorID}, Patient NHS Number: {appointment.PatientNHSNumber}");
                                        Console.WriteLine($"Start Time: {appointment.StartTime}, End Time: {appointment.EndTime}");
                                        Console.WriteLine(new string('-', 30));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No appointments scheduled.");
                                }
                                Console.WriteLine("Press any key to return to the menu.");
                                Console.ReadKey();
                                break;

                            case "5": // Cancel Appointment
                                Console.Write("Enter Appointment ID to cancel: ");
                                if (int.TryParse(Console.ReadLine(), out int appointmentId))
                                {
                                    try
                                    {
                                        appointmentManager.CancelAppointment(appointmentId);
                                        Console.WriteLine("Appointment canceled successfully.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Appointment ID.");
                                }
                                Console.WriteLine("Press any key to return to the menu.");
                                Console.ReadKey();
                                break;

                            case "6": // Exit
                                Console.WriteLine("Exiting Appointment Scheduling System.");
                                break;

                            default:
                                Console.WriteLine("Invalid choice. Please select a valid option.");
                                break;
                        }
                        break;


                    case "5":
                        Console.WriteLine("Exiting Nurse Menu.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }

            return response;
        }


        internal string GetDoctorMenuResponse()
        {
            string response = string.Empty;

            Console.WriteLine("DOCTOR MAIN MENU");
            Console.WriteLine("1. List All Patients");
            Console.WriteLine("2. Searh Patient");
            Console.WriteLine("3. View Full Patient Record");
            Console.WriteLine("4. Manage Appointment");
            Console.WriteLine("5. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            DoctorMenu(choice);
            return choice;



            static void DoctorMenu(string choice)
            {
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("LIST PATIENT");
                        Console.WriteLine(new string('-', 50));
                        PatientManager.ListAllPatients();
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("SEARCH PATIENT");
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("\nSearch from the following: ");
                        Console.WriteLine("1. Full Name and Date of Birth");
                        Console.WriteLine("2. NHS Number");
                        Console.WriteLine("3. Hospital Number");
                        Console.WriteLine("4. Exit to Main Menu");
                        Console.Write("Enter your choice: ");
                        string searchChoiceMain = Console.ReadLine();

                        switch (searchChoiceMain)
                        {
                            case "1":
                                Console.Write("Enter Full Name (FirstName LastName): ");
                                string fullName = Console.ReadLine();

                                Console.Write("Enter Date of Birth (yyyy-mm-dd): ");
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime dob))
                                {
                                    Console.WriteLine("Invalid date format.");
                                    break;
                                }

                                var foundPatients = PatientManager.SearchPatientByDobAndName(dob, fullName);

                                if (foundPatients.Any())
                                {
                                    foreach (var patient in foundPatients)
                                    {
                                        DisplayPatientInfo(patient);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No patient found with the given details.");
                                }
                                break;

                            case "2":
                                Console.Write("Enter NHS Number: ");
                                string nhsNum = Console.ReadLine();

                                var patientByNHS = PatientManager.SearchPatientByNHSNumber(nhsNum);

                                if (patientByNHS != null)
                                {
                                    DisplayPatientInfo(patientByNHS);
                                }
                                else
                                {
                                    Console.WriteLine("No patient found with the given NHS Number.");
                                }
                                break;

                            case "3":
                                Console.Write("Enter Hospital Number: ");
                                string hospitalNum = Console.ReadLine();

                                var patientByHospital = PatientManager.SearchPatientByHospitalNumber(hospitalNum);

                                if (patientByHospital != null)
                                {
                                    DisplayPatientInfo(patientByHospital);
                                }
                                else
                                {
                                    Console.WriteLine("No patient found with the given Hospital Number.");
                                }
                                break;

                            case "4":
                                Console.WriteLine("Exiting Doctor Menu.");
                                Environment.Exit(0);
                                break;

                            default:
                                Console.WriteLine("Invalid choice. Please select a valid option.");
                                break;
                        }
                        break;

                    case "3":

                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine(new string('-', 50));
                            Console.WriteLine("PATIENT RECORD");
                            Console.WriteLine(new string('-', 50));
                            Console.WriteLine("\nSelect an option:");
                            Console.WriteLine("1. Update Patient Basic Details");
                            Console.WriteLine("2. Patient Appointment Details");
                            Console.WriteLine("3. Patient Prescriptions");
                            Console.WriteLine("4. View Patient Notes");
                            Console.WriteLine("5. Exit to Main Menu");
                            Console.Write("Enter your choice: ");
                            string subChoice = Console.ReadLine();

                            switch (subChoice)
                            {
                                case "1":
                                    Console.WriteLine("Incomplete");
                                    break;

                                case "2":
                                    Console.WriteLine("Incomplete");
                                    break;

                                case "3":
                                    Console.WriteLine("Incomplete");
                                    break;
                                case "4":
                                    Console.WriteLine("Patient Notes: ");
                                    var note = new Notes
                                    {
                                        DateCreated = DateTime.Now
                                    };
                                    Console.WriteLine($"Date: {note.DateCreated}");

                                    var nhsForViewNotes = PromptForNHSNumber();
                                    var notesList = PatientManager.GetPatientNotes(nhsForViewNotes);
                                    if (notesList != null && notesList.Any())
                                    {
                                        foreach (var Note in notesList)
                                        {
                                            Console.WriteLine($"Date: {note.DateCreated}, Content: {note.Content}");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("No notes found or patient does not exist.");
                                    }
                                    break;

                                case "5":
                                    Console.WriteLine("Exiting Doctor Menu.");
                                    Environment.Exit(0);
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Try again.");
                                    break;
                            }
                        }
                        break;


                    case "4":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("MANAGE APPOINTMENTS");
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("1. Add Doctor");
                        Console.WriteLine("2. Add Patient");
                        Console.WriteLine("3. Schedule Appointment");
                        Console.WriteLine("4. List Appointments");
                        Console.WriteLine("5. Cancel Appointment");
                        Console.WriteLine("6. Exit");
                        Console.Write("Select an option: ");

                        string appointmentChoice = Console.ReadLine();
                        var appointmentManager = new AppointmentManager();

                        switch (appointmentChoice)
                        {
                            case "1": // Add Doctor
                                Console.Write("Enter Doctor ID: ");
                                if (int.TryParse(Console.ReadLine(), out int doctorId))
                                {
                                    Console.Write("Enter Doctor Name: ");
                                    string name = Console.ReadLine();
                                    DoctorManager.Add(new Doctor { DoctorID = doctorId, Name = name });
                                    Console.WriteLine("Doctor added successfully.");
                                }
                                else
                                {
                                    Console.WriteLine("Invalid ID. Doctor not added.");
                                }
                                Console.WriteLine("Press any key to return to the menu.");
                                Console.ReadKey();
                                break;

                            case "2": // Add Patient
                                Console.Write("INCOMPLETE");
                                
                                break;

                            case "3": // Add Appointment
                                Console.Write("Enter Doctor ID: ");
                                while (!int.TryParse(Console.ReadLine(), out doctorId))
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid Doctor ID.");
                                }

                                Console.Write("Enter Patient NHS Number: ");
                                string patientNHSNumber; // Change to string
                                patientNHSNumber = Console.ReadLine(); // Read directly as string
                                while (string.IsNullOrWhiteSpace(patientNHSNumber)) // Check for empty input
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid Patient NHS Number.");
                                    patientNHSNumber = Console.ReadLine();
                                }

                                Console.Write("Enter Start Time (yyyy-MM-dd HH:mm): ");
                                DateTime startTime;
                                while (!DateTime.TryParse(Console.ReadLine(), out startTime))
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid date and time.");
                                }

                                Console.Write("Enter End Time (yyyy-MM-dd HH:mm): ");
                                DateTime endTime;
                                while (!DateTime.TryParse(Console.ReadLine(), out endTime))
                                {
                                    Console.WriteLine("Invalid input. Please enter a valid date and time.");
                                }

                                appointmentManager.AddAppointment(doctorId, patientNHSNumber, startTime, endTime);
                                Console.WriteLine("Appointment scheduled successfully.");
                                break;

                            case "4": // List Appointments
                                var AppointmentManager = new AppointmentManager();
                                var appointments = AppointmentManager.ListAppointments();
                                if (appointments.Any())
                                {
                                    Console.WriteLine("Scheduled Appointments:");
                                    foreach (var appointment in appointments)
                                    {
                                        Console.WriteLine($"Appointment ID: {appointment.AppointmentId}, Doctor ID: {appointment.DoctorID}, Patient NHS Number: {appointment.PatientNHSNumber}");
                                        Console.WriteLine($"Start Time: {appointment.StartTime}, End Time: {appointment.EndTime}");
                                        Console.WriteLine(new string('-', 30));
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No appointments scheduled.");
                                }
                                Console.WriteLine("Press any key to return to the menu.");
                                Console.ReadKey();
                                break;

                            case "5": // Cancel Appointment
                                Console.Write("Enter Appointment ID to cancel: ");
                                if (int.TryParse(Console.ReadLine(), out int appointmentId))
                                {
                                    try
                                    {
                                        appointmentManager.CancelAppointment(appointmentId);
                                        Console.WriteLine("Appointment canceled successfully.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error: {ex.Message}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Appointment ID.");
                                }
                                Console.WriteLine("Press any key to return to the menu.");
                                Console.ReadKey();
                                break;

                            case "6": // Exit
                                Console.WriteLine("Exiting Appointment Scheduling System.");
                                break;

                            default:
                                Console.WriteLine("Invalid choice. Please select a valid option.");
                                break;
                        }
                        break;

                    case "5":
                        Console.WriteLine("Exiting Doctor Menu.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }

            return response;
        }



        internal string GetAdminMenuResponse()
        {
            string response = string.Empty;

            Console.WriteLine("ADMIN MAIN MENU");
            Console.WriteLine("1. List All Staff Account");
            Console.WriteLine("2. Add New User");
            Console.WriteLine("3. Update User");
            Console.WriteLine("4. Search Patient");
            Console.WriteLine("5. Enable/Disable User");
            Console.WriteLine("6. Appointment Details");
            Console.WriteLine("7. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine(); // Capture user input

            AdminMenu(choice);



            static void AdminMenu(string choice)
            {
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("LIST OF ALL STAFF ACCOUNTS");
                        Console.WriteLine(new string('-', 50));

                        string jsonFilePath = "users.json";
                        UserManager userManager = new UserManager(jsonFilePath);

                        // List all staff members
                        var staffMembers = UserManager.ListAllStaff();
                        //foreach (var user in users)
                        //{
                        //Console.WriteLine($"Email: {user.Email}, UserType: {user.UserType}, IsEnabled: {user.IsEnabled}");
                        //}
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("ADD A NEW USER");
                        Console.WriteLine(new string('-', 50));
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("UPDATE USER DETAILS");
                        Console.WriteLine(new string('-', 50));
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("SEARCH PATIENT");
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("\nSearch from the following: ");
                        Console.WriteLine("1. Full Name and Date of Birth");
                        Console.WriteLine("2. NHS Number");
                        Console.WriteLine("3. Hospital Number");
                        Console.Write("Enter your choice: ");
                        string searchChoice = Console.ReadLine();

                        switch (searchChoice)
                        {
                            case "1":
                                Console.Write("Enter Full Name (FirstName LastName): ");
                                string fullName = Console.ReadLine();

                                Console.Write("Enter Date of Birth (yyyy-mm-dd): ");
                                if (!DateTime.TryParse(Console.ReadLine(), out DateTime dob))
                                {
                                    Console.WriteLine("Invalid date format.");
                                    break;
                                }

                                var foundPatients = PatientManager.SearchPatientByDobAndName(dob, fullName);

                                if (foundPatients.Any())
                                {
                                    foreach (var patient in foundPatients)
                                    {
                                        DisplayPatientInfo(patient);
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No patient found with the given details.");
                                }
                                break;

                            case "2":
                                Console.Write("Enter NHS Number: ");
                                string nhsNum = Console.ReadLine();

                                var patientByNHS = PatientManager.SearchPatientByNHSNumber(nhsNum);

                                if (patientByNHS != null)
                                {
                                    DisplayPatientInfo(patientByNHS);
                                }
                                else
                                {
                                    Console.WriteLine("No patient found with the given NHS Number.");
                                }
                                break;

                            case "3":
                                Console.Write("Enter Hospital Number: ");
                                string hospitalNum = Console.ReadLine();

                                var patientByHospital = PatientManager.SearchPatientByHospitalNumber(hospitalNum);

                                if (patientByHospital != null)
                                {
                                    DisplayPatientInfo(patientByHospital);
                                }
                                else
                                {
                                    Console.WriteLine("No patient found with the given Hospital Number.");
                                }
                                break;
                        }
                        break;
                    case "5":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("ENABLE/DISABLE USER ACCOUNTS");
                        Console.WriteLine(new string('-', 50));

                        static void ListAllUsers(UserManager userManager)
                        {
                            var addUser = new List<User>();
                            var users = userManager.ListAllUsers(addUser);
                            foreach (var user in users)
                            {
                                Console.WriteLine($"Email: {user.Email}, UserType: {user.UserType}, IsEnabled: {user.IsEnabled}");
                            }
                        }

                        static void EnableUserAccount(UserManager userManager)
                        {
                            Console.Write("Enter the email of the user to enable: ");
                            string email = Console.ReadLine();
                            bool result = userManager.EnableUser(email);
                            if (result)
                                Console.WriteLine("User account enabled successfully.");
                            else
                                Console.WriteLine("User not found.");
                        }

                        static void DisableUserAccount(UserManager userManager, string adminEmail)
                        {
                            Console.Write("Enter the email of the user to disable: ");
                            string email = Console.ReadLine();
                            bool result = userManager.DisableUser(email, adminEmail);
                            if (result)
                                Console.WriteLine("User account disabled successfully.");
                            else
                                Console.WriteLine("User not found or cannot disable your own account.");
                        }
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("LIST APPOINTMENTS");
                        Console.WriteLine(new string('-', 50));
                        var appointmentManager = new AppointmentManager();
                        var appointments = appointmentManager.ListAppointments();
                        if (appointments.Any())
                        {
                            Console.WriteLine("Scheduled Appointments:");
                            foreach (var appointment in appointments)
                            {
                                Console.WriteLine($"Appointment ID: {appointment.AppointmentId}, Doctor ID: {appointment.DoctorID}");
                                Console.WriteLine($"Start Time: {appointment.StartTime}, End Time: {appointment.EndTime}");
                                Console.WriteLine(new string('-', 30));
                            }
                        }
                        else
                        {
                            Console.WriteLine("No appointments scheduled.");
                        }
                        Console.WriteLine("Press any key to return to the menu.");
                        Console.ReadKey();
                        break;

                    case "7":
                        Console.WriteLine("Exiting Doctor Menu.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }

            }

            return response;
        }



        internal static void DisplayPatientInfo(Patient patient)
        {
            Console.WriteLine("\nPatient Information:");
            Console.WriteLine($"Name: {patient.FirstName} {patient.LastName}");
            Console.WriteLine($"Date of Birth: {patient.DateOfBirth:yyyy-MM-dd}");
            Console.WriteLine($"Contact Details: {patient.ContactDetails}");
            Console.WriteLine($"NHS Number: {patient.NHSNumber}");
            Console.WriteLine($"Hospital Number: {patient.HospitalNumber}");
            Console.WriteLine(new string('-', 30));

        }

        static string PromptForNHSNumber()
        {
            Console.Write("Enter NHS Number: ");
            return Console.ReadLine();
        }

        static DateTime PromptForDate(string promptMessage)
        {
            DateTime date = default;
            Console.Write(promptMessage);
            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("Invalid date format. Please try again.");
                Console.Write(promptMessage);
            }
            return date;
        }
    }
}
