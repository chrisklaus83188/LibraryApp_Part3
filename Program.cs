// See https://aka.ms/new-console-template for more information
using System.Data.SqlClient;
//using TestDatabaseAccess_ClassLibrary;
using LibraryApp_ClassLibrary_Database;
using LibraryApp_ClassLibrary_General;

string connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

//ClassDb obj = new ClassDb();
UserData data = new UserData();

data.P.MainMenu();

string input = "";
while (input != "e")
{
    Console.WriteLine(" ");
    input = Console.ReadLine();
    if (!string.IsNullOrEmpty(input))
    {
        switch (input)
        {
            case "g":
                data.U.LogInAsGuest(data.Data);
                break;
            case "r":
                data.U.RegisterUser(data.Data);
                break;
            case "l":
                data.U.LogInAsRegisteredUser(data.Data);
                break;
            case "o":
                data.U.LogOut(data.Data);
                break;
            case "pp":
                Console.WriteLine("Please type in a username");
                string username = Console.ReadLine();
                if (!string.IsNullOrEmpty(username))
                {
                    User u = data.ReturnUser(username);
                    data.P.PrintUserProfile(u);
                }
                else
                    Console.WriteLine("User not found");
                break;
            case "pr":
                data.P.PrintRoles();
                break;
            case "pu":
                Console.WriteLine("Here are all of the users currently in the library system:");
                for (int i = 0; i < data.Data.Count; i++)
                    data.P.PrintUser(data.Data[i]);
                break;
            case "add role":
                data.UI_AddRole(connString);
                break;
            case "delete role":
                data.UI_DeleteRole(connString); 
                break;
            case "update role":
                data.UI_UpdateRole(connString);
                break;
            case "view roles":
                data.UI_ReadRoles(connString);
                break;
            case "add user":
                data.UI_AddUser(connString);
                break;
            case "delete user":
                data.UI_DeleteUser(connString);
                break;
            case "update user general":
                data.UI_UpdateUser(connString);
                break;
            case "update user login":
                data.UI_UpdateUserLoggedIn(connString);
                break;
            case "view users":
                data.UI_ReadUsers(connString);
                break;
            case "add book":
                data.UI_AddBook(connString);
                break;
            case "delete book":
                data.UI_DeleteBook(connString);
                break;
            case "update book checkout":
                data.UI_UpdateBookCheckedOut(connString);
                break;
            case "update book":
                data.UI_UpdateBook(connString);
                break;
            case "view books":
                data.UI_ReadBooks(connString);
                break;
            case "m":
                data.P.MainMenu();
                break;
            case "e":
                data.P.ExitMessage();
                break;
            default:
                data.P.BadInput();
                break;
        }
    }
    else
        data.P.BadInput();
}



































 /*   Console.WriteLine(data.UpdateRole(0,"Something Else", connString));
Console.WriteLine(data.UpdateUserLoggedIn(false,"chrisklaus83188",connString));

Console.WriteLine(data.CreateRole(5, "Janitor", connString));
Console.WriteLine(data.DeleteRole(5, connString));

Dictionary<int,string> Roles = data.ReadRoles(connString);

Console.WriteLine(Roles[0]);
Console.WriteLine(Roles[1]);
Console.WriteLine(Roles[2]);
Console.WriteLine(Roles[3]);

List<User> current_users = data.ReadUsers(connString);
Console.WriteLine(current_users[0].Username);
Console.WriteLine(current_users[1].Username);
Console.WriteLine(current_users[2].Username);

Console.WriteLine("Hello, World!");*/
