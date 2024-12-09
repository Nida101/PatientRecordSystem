using PatientRecordSystem;
using System;

class Program
{
    static void Main(string[] args)
    {
        Login();
        //Console.WriteLine("Logged out. Goodbye...");
    }

    static void Login()
    {
        do
        {
            Console.Write("Enter your username > ");
            string username = Console.ReadLine();
            string password = GetPasswordHiddenByChar();

            try
            {
                AuthenticatorManager.AttemptLogin(username, password);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                continue; // Optionally continue to the next iteration
            }

            if (AuthenticatorManager.CurrentUser == null)
            {
                Console.WriteLine("Invalid username or password");
            }
            else
            {
                Console.WriteLine("\n\nLogin successful");
                MenuSystemController menuSystemController = new MenuSystemController();
                string response = menuSystemController.GetMenuResponse();

                Console.WriteLine($"You selected {response}.");

                // Check if the user wants to log out
                if (Logout())
                {
                    // If the user logged out, break the loop
                    break;
                }
                else
                {
                    // If the user did not log out, continue the loop
                    Console.WriteLine("Continuing session...");
                }
            }

        } while (AuthenticatorManager.CurrentUser == null);
    }

    static bool Logout()
    {
        Console.Write("Do you want to logout? (yes/no) --> ");
        string input = Console.ReadLine().ToLower();

        if (input == "yes")
        {
            AuthenticatorManager.Logout(); // Assuming this clears the session
            Console.WriteLine("You have been logged out.");
            return true; // Indicate that the user has logged out
        }
        else
        {
            Console.WriteLine("You are still logged in.");
            return false; // Indicate that the user has not logged out
        }
    }

    static string GetPasswordHiddenByChar()
    {
        string password = string.Empty;
        ConsoleKeyInfo keyInfo;

        Console.Write("Enter your password: ");
        do
        {
            keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Backspace)
            {
                password += keyInfo.KeyChar;
                Console.Write("*");
            }
            else if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password[0..^1];
                Console.Write("\b \b");
            }
        } while (keyInfo.Key != ConsoleKey.Enter);

        Console.WriteLine(); // Move to the next line after password entry
        return password;
    }
}