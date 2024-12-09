using PatientRecordSystem;
using PatientRecordSystem._2._Patient_Record_M;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Newtonsoft.Json;
using static PatientRecordSystem.Patient;
using static PatientRecordSystem.Prescription;

namespace PatientRecordSystem
{
    internal class MenuSystemController
    {
        string filePath = "patients.json";

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
            Console.WriteLine("1. Add Patients");
            Console.WriteLine("2. List All Patients");
            Console.WriteLine("3. Search Patient");
            Console.WriteLine("4. View Full Patient Record");
            Console.WriteLine("5. Manage Appointment");
            Console.WriteLine("6. Exit");

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
                        Console.WriteLine("ADD PATIENT");
                        Console.WriteLine(new string('-', 50));

                        AddPatient patientManager = new AddPatient();

                        while (true)
                        {
                            Console.WriteLine("Enter patient details:");

                            // Prompt for first name
                            Console.Write("First Name: ");
                            string firstName = Console.ReadLine();

                            // Prompt for last name
                            Console.Write("Last Name: ");
                            string lastName = Console.ReadLine();

                            // Prompt for date of birth using the existing method
                            DateTime dateOfBirth = Patient.PromptForDate("Date of Birth (yyyy-MM-dd): ");

                            // Prompt for contact details
                            Console.Write("Contact Details: ");
                            string contactDetails = Console.ReadLine();

                            // Prompt for NHS number using the existing method
                            string nhsNumber = Patient.PromptForNHSNumber();

                            // Add the new patient
                            try
                            {
                                patientManager.AddNewPatient(firstName, lastName, dateOfBirth, contactDetails, nhsNumber);
                            }
                            catch (ArgumentException ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            // Ask if the user wants to add another patient
                            Console.Write("Do you want to add another patient? (y/n): ");
                            string continueInput = Console.ReadLine();
                            if (continueInput?.ToLower() != "y")
                            {
                                break; // Exit the loop if the user does not want to continue
                            }
                        }
                        break;


                    case "2":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("LIST PATIENTS");
                        Console.WriteLine(new string('-', 50));
                        PatientManager.ListAllPatients();
                        break;


                    case "3":
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

                    case "4":
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine(new string('-', 50));
                            Console.WriteLine("PATIENT RECORD");
                            Console.WriteLine(new string('-', 50));
                            Console.WriteLine("\nSelect an option:");
                            Console.WriteLine("1. View/Update Patients Basic Details");
                            Console.WriteLine("2. View/Update Patients Appointment Details");
                            Console.WriteLine("3. View/Update Patients Prescriptions");
                            Console.WriteLine("4. View/Update Patient Notes");
                            Console.WriteLine("5. Exit to Main Menu");
                            Console.Write("Enter your choice: ");
                            string select = Console.ReadLine();

                            switch (select)
                            {
                                case "1":
                                    Console.WriteLine("VIEW/UPDATE PATIENTS BASIC DETAILS");
                                    // Prompt the user to enter the NHS number
                                    Console.Write("Please enter the NHS number of the patient you want to view/update: ");
                                    string nhsNumber = Console.ReadLine();

                                    // Call the method to view and update patient details
                                    PatientManager.ViewUpdatePatientDetails(nhsNumber);
                                    break;

                                case "2":
                                    string FilePath = "patients.json"; // Path to appointments JSON file
                                    Console.WriteLine("VIEW/UPDATE PATIENTS APPOINTMENT DETAILS");
                                    Console.WriteLine("Enter the Appointment ID to update:");
                                    if (int.TryParse(Console.ReadLine(), out int appointmentId))
                                    {
                                        AppointmentManager.ViewUpdateAppointmentDetails(appointmentId, FilePath);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Appointment ID.");
                                    }
                                    break;


                                case "3":
                                    Console.WriteLine("VIEW/UPDATE PATIENTS PRESCRIPTIONS");
                                    PrescriptionManager PrescriptionManager = new PrescriptionManager();
                                    Console.WriteLine("Enter NHS Number:");
                                    string NHSNumber = Console.ReadLine();
                                    PrescriptionManager.ViewUpdatePatientPrescriptions(NHSNumber);

                                    break;

                                case "4":
                                    Console.WriteLine("VIEW/UPDATE PATIENTS NOTES");
                                    Notes.ViewUpdatePatientNotes();
                                    break;

                                case "5":
                                    AuthenticatorManager.Logout();
                                    Console.WriteLine("Logging Out...");
                                    Environment.Exit(0);
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Try again.");
                                    break;
                            }
                        }
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("MANAGE APPOINTMENTS");
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("1. Schedule Appointment");
                        Console.WriteLine("2. List Appointments");
                        Console.WriteLine("3. Cancel Appointment");
                        Console.WriteLine("4. Exit");
                        Console.Write("Select an option: ");

                        string appointmentChoice = Console.ReadLine();
                        var appointmentManager = new AppointmentManager();
                        string filePath = "appointments.json";

                        // Load existing appointments from file
                        appointmentManager.LoadAppointmentsFromFile(filePath);

                        switch (appointmentChoice)
                        {
                            case "1": // Schedule Appointment
                                Console.Write("Enter Doctor ID: ");
                                int doctorId; 
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

                                appointmentManager.AddAppointment(doctorId, patientNHSNumber, startTime, endTime, filePath);
                                Console.WriteLine("Appointment scheduled successfully.");

                                // Save appointments to file
                                appointmentManager.SaveAppointmentsToFile(filePath);
                                break;


                            case "2": // List Appointments

                                var AppointmentManager = new AppointmentManager();
                                var appointments = AppointmentManager.ListAppointments(filePath);
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

                            case "3": // Cancel Appointment
                                Console.Write("Enter Appointment ID to cancel: ");
                                if (int.TryParse(Console.ReadLine(), out int appointmentId))
                                {
                                    try
                                    {
                                        appointmentManager.CancelAppointment(appointmentId, filePath);
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

                            case "4": // Exit
                                Console.WriteLine("Exiting Appointment Scheduling System.");
                                break;

                            default:
                                Console.WriteLine("Invalid choice. Please select a valid option.");
                                break;
                        }
                        break;


                    case "6":
                        AuthenticatorManager.Logout();
                        Console.WriteLine("Logging Out...");
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
                                AuthenticatorManager.Logout();
                                Console.WriteLine("Logging Out...");
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
                            Console.WriteLine("1. View/Update Patients Basic Details");
                            Console.WriteLine("2. View/Update Patients Appointment Details");
                            Console.WriteLine("3. View/Update Patients Prescriptions");
                            Console.WriteLine("4. View/Update Patient Notes");
                            Console.WriteLine("5. Exit to Main Menu");
                            Console.Write("Enter your choice: ");
                            string subChoice = Console.ReadLine();

                            switch (subChoice)
                            {
                                case "1":
                                    Console.WriteLine("VIEW/UPDATE PATIENTS BASIC DETAILS");
                                    // Prompt the user to enter the NHS number
                                    Console.Write("Please enter the NHS number of the patient you want to view/update: ");
                                    string nhsNumber = Console.ReadLine();

                                    // Call the method to view and update patient details
                                    PatientManager.ViewUpdatePatientDetails(nhsNumber);
                                    break;

                                case "2":
                                    string FilePath = "appointments.json"; // Path yo appointments JSON file
                                    Console.WriteLine("VIEW/UPDATE PATIENTS APPOINTMENT DETAILS");
                                    Console.WriteLine("Enter the Appointment ID to update:");
                                    if (int.TryParse(Console.ReadLine(), out int appointmentId))
                                    {
                                        AppointmentManager.ViewUpdateAppointmentDetails(appointmentId, FilePath);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Appointment ID.");
                                    }
                                    break;

                                case "3":
                                    Console.WriteLine("VIEW/UPDATE PATIENTS PRESCRIPTION DETAILS");
                                    PrescriptionManager PrescriptionManager = new PrescriptionManager();
                                    Console.WriteLine("Enter NHS Number:");
                                    string NHSNumber = Console.ReadLine();
                                    PrescriptionManager.ViewUpdatePatientPrescriptions(NHSNumber);


                                    break;

                                case "4":
                                    Console.WriteLine("VIEW/UPDATE PATIENTS NOTES");
                                    Notes.ViewUpdatePatientNotes();
                                    
                                    break;

                                case "5":
                                    AuthenticatorManager.Logout();
                                    Console.WriteLine("Logging Out...");
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
                        Console.WriteLine("1. Schedule Appointment");
                        Console.WriteLine("2. List Appointments");
                        Console.WriteLine("3. Cancel Appointment");
                        Console.WriteLine("4. Exit");
                        Console.Write("Select an option: ");

                        string appointmentChoice = Console.ReadLine();
                        var appointmentManager = new AppointmentManager();
                        string filePath = "appointments.json";

                        // Load existing appointments from file
                        appointmentManager.LoadAppointmentsFromFile(filePath);

                        switch (appointmentChoice)
                        {
                            case "1": // Add Appointment
                                Console.Write("Enter Doctor ID: ");
                                int doctorId;
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

                                appointmentManager.AddAppointment(doctorId, patientNHSNumber, startTime, endTime, filePath);
                                Console.WriteLine("Appointment scheduled successfully.");
                                break;


                            case "2": // List Appointments
                                var AppointmentManager = new AppointmentManager();
                                var appointments = AppointmentManager.ListAppointments(filePath);
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

                            case "3": // Cancel Appointment
                                Console.Write("Enter Appointment ID to cancel: ");
                                if (int.TryParse(Console.ReadLine(), out int appointmentId))
                                {
                                    try
                                    {
                                        appointmentManager.CancelAppointment(appointmentId, filePath);
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

                            case "4": // Exit
                                Console.WriteLine("Exiting Appointment Scheduling System.");
                                break;

                            default:
                                Console.WriteLine("Invalid choice. Please select a valid option.");
                                break;
                        }
                        break;

                    case "5":
                        AuthenticatorManager.Logout();
                        Console.WriteLine("Logging Out...");
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
            Console.WriteLine("1. Manage Users Record");
            Console.WriteLine("2. List All Staff Accounts");
            Console.WriteLine("3. Manage Patient Record");
            Console.WriteLine("4. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine(); // Capture user input

            AdminMenu(choice);
            return choice;



            static void AdminMenu(string choice)
            {
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("MANAGE USERS RECORD");
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("1. View/Update New Users");
                        Console.WriteLine("2. Enable/Disable User Accounts");
                        Console.WriteLine("3. Exit");
                        Console.Write("Select an option: ");

                        string ManageUsersRecordChoice = Console.ReadLine();

                        switch (ManageUsersRecordChoice)
                        {
                            case "1":
                                Console.WriteLine("VIEW/UPDATE NEW USERS");

                                // Ask admin for action
                                Console.WriteLine("Would you like to add or update a user? (add/update)");
                                string action = Console.ReadLine()?.ToLower();
                                UserManager usermanager = new UserManager("users.json");

                                if (action == "add" || action == "update")
                                {
                                    // Get user details from admin
                                    Console.Write("Enter user email: ");
                                    string email = Console.ReadLine();

                                    Console.Write("Enter user type: ");
                                    string userType = Console.ReadLine();

                                    Console.Write("Enter user password: ");
                                    string password = Console.ReadLine();

                                    // Create a new user object
                                    User user = new User
                                    {
                                        Email = email,
                                        UserType = userType
                                    };

                                    // Add or update the user
                                    if (usermanager.AddOrUpdateUser(user, password))
                                    {
                                        Console.WriteLine("User added/updated successfully.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Failed to add/update user.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid action. Please enter 'add' or 'update'.");
                                }
                                break;

                            case "2":
                                Console.WriteLine(new string('-', 50));
                                Console.WriteLine("ENABLE/DISABLE USER ACCOUNTS");
                                Console.WriteLine(new string('-', 50));
                                Console.WriteLine("1. Enable User Account");
                                Console.WriteLine("2. Disable User Account");
                                Console.WriteLine("3. Exit");
                                Console.Write("Select an option: ");

                                string ManageUserRecordChoice = Console.ReadLine();
                                string jsonFilePath = "users.json"; // Replace with the correct path to your JSON file
                                var usersmanager = new UserManager(jsonFilePath);

                                switch (ManageUserRecordChoice)
                                {
                                    case "1":
                                        Console.WriteLine("ENABLE USER ACCOUNT");
                                        Console.Write("Enter admin email: ");
                                        string adminEmailEnable = Console.ReadLine();
                                        Console.Write("Enter the email of the user to enable: ");
                                        string emailEnable = Console.ReadLine();

                                        bool enableResult = usersmanager.ChangeUserStatus(emailEnable, adminEmailEnable, true);
                                        if (!enableResult)
                                        {
                                            Console.WriteLine("Failed to enable user.");
                                        }
                                        break;

                                    case "2":
                                        Console.WriteLine("DISABLE USER ACCOUNT");
                                        Console.Write("Enter admin email: ");
                                        string adminEmailDisable = Console.ReadLine();
                                        Console.Write("Enter the email of the user to disable: ");
                                        string emailDisable = Console.ReadLine();

                                        bool disableResult = usersmanager.ChangeUserStatus(emailDisable, adminEmailDisable, false);
                                        if (!disableResult)
                                        {
                                            Console.WriteLine("Failed to disable user.");
                                        }
                                        break;

                                    case "3": // Exit
                                        Console.WriteLine("Exiting the program. Goodbye!");
                                        return;

                                    default:
                                        Console.WriteLine("Invalid choice. Please try again.");
                                        break;
                                }
                                break;

                            case "3":
                                AuthenticatorManager.Logout();
                                Console.WriteLine("Logging Out...");
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please select a valid option.");
                                break;
                        }
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("LIST ALL STAFF ACCOUNTS");
                        Console.WriteLine(new string('-', 50));

                        UserManager userManager = new UserManager("users.json");
                        userManager.ListAllUsers();
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("MANAGE PATIENT RECORD");
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine("1. Add a New Patient");
                        Console.WriteLine("2. View Patients Basic Information");
                        Console.WriteLine("3. Search for a Patient");
                        Console.WriteLine("4. Patients Appointment Details");
                        Console.WriteLine("5. Exit");
                        Console.WriteLine("Enter your choice: ");

                        string PatientRecordChoice = Console.ReadLine();
                      
                        switch (PatientRecordChoice)
                        {
                            case "1":
                                Console.WriteLine("ADD NEW PATIENTS");
                                AddPatient patientManager = new AddPatient();

                                while (true)
                                {
                                    Console.WriteLine("Enter patient details:");

                                    // Prompt for first name
                                    Console.Write("First Name: ");
                                    string firstName = Console.ReadLine();

                                    // Prompt for last name
                                    Console.Write("Last Name: ");
                                    string lastName = Console.ReadLine();

                                    // Prompt for date of birth using the existing method
                                    DateTime dateOfBirth = Patient.PromptForDate("Date of Birth (yyyy-MM-dd): ");

                                    // Prompt for contact details
                                    Console.Write("Contact Details: ");
                                    string contactDetails = Console.ReadLine();

                                    // Prompt for NHS number using the existing method
                                    string nhsNumber = Patient.PromptForNHSNumber();

                                    // Add the new patient
                                    try
                                    {
                                        patientManager.AddNewPatient(firstName, lastName, dateOfBirth, contactDetails, nhsNumber);
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }

                                    // Ask if the user wants to add another patient
                                    Console.Write("Do you want to add another patient? (y/n): ");
                                    string continueInput = Console.ReadLine();
                                    if (continueInput?.ToLower() != "y")
                                    {
                                        break; // Exit the loop if the user does not want to continue
                                    }
                                }
                                break;

                            case "2":
                                Console.Clear();
                                Console.WriteLine(new string('-', 50));
                                Console.WriteLine("VIEW PATIENTS BASIC INFORMATION");
                                Console.WriteLine(new string('-', 50));
                                PatientManager.ListAllPatients();
                                break;

                            case "3":
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
                                string SearchPatientChoice = Console.ReadLine();

                                switch (SearchPatientChoice)
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
                                        AuthenticatorManager.Logout();
                                        Console.WriteLine("Logging Out...");
                                        Environment.Exit(0);
                                        break;

                                    default:
                                        Console.WriteLine("Invalid choice. Please select a valid option.");
                                        break;
                                }
                                break;

                            case "4":
                                Console.Clear();
                                Console.WriteLine(new string('-', 50));
                                Console.WriteLine("PATIENTS APPOINTMENT DETAILS");
                                Console.WriteLine(new string('-', 50));
                                string filePath = "appointments.json";

                                var AppointmentManager = new AppointmentManager();
                                var appointments = AppointmentManager.ListAppointments(filePath);
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

                            case "5":
                                AuthenticatorManager.Logout();
                                Console.WriteLine("Logging Out...");
                                Environment.Exit(0);
                                break;
                        }
                        break;

                    case "4":
                        Console.WriteLine("Exiting Admin Menu.");
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
    }
}
