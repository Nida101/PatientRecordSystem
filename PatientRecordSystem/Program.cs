using PatientRecordSystem;
using System.Runtime.CompilerServices;

//PatientRecordDataManager p = new PatientRecordDataManager();
Login();

Console.WriteLine("Logged out. Goodbye...");

void Login()
{
    do
    {

        Console.Write("Enter your username > ");
        string username = Console.ReadLine();
        string password = GetPasswordHiddenByChar();

        AuthenticatorManager.AttemptLogin(username, password);

        if (AuthenticatorManager.CurrentUser == null)
        {
            Console.WriteLine("Invalid username or password");
        }
        else
        {
            // The next thing

            Console.WriteLine("\n\nLogin successful");
            MenuSystemController menuSystemController = new MenuSystemController();
            string response = menuSystemController.GetMenuResponse();

            Console.WriteLine($"You selected {response}.");

            Logout(username, password);
        }

    } while (AuthenticatorManager.CurrentUser == null);
}


void Logout(string username, string password)
{
    Console.Write("Do you want to logout? --> ");
    if (Console.ReadLine().ToLower() == "no")
    {
        AuthenticatorManager.Logout();
    }
    else
    {
        AuthenticatorManager.AttemptLogin(username, password);
    }
}

void DisplayMenu1()
{
    Console.WriteLine("This is the Menu: ");
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

    return password;
}

