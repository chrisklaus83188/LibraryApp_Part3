namespace LibraryApp_ClassLibrary_General
{
    public enum Role { Guest, Administrator, Librarian, Patron }

    public class Utility
    {
        public Utility() { }

        public void RegisterUser(List<User> list)
        {
            bool validName = false;
            bool validDate = false;
            bool validRole = false;
            bool validUsername = false;
            bool validPassword = false;

            User u = new User();

            string input = "";
            string exitString = "exit";
            bool exitStringEntered = false;

            Console.WriteLine("Please enter your first and last name. Please include a space between your first and last names.");
            while (!validName && !exitStringEntered)
            {
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    exitStringEntered = CheckIfExitString(input, exitString);
                    validName = CheckValidName(input, u);   //if user types the exit string, then validName will simply remain false and upone the next while loop iteration the condition will evaluate to false
                }
                else
                {
                    input = "";         //Prevent accessing NULL at the while loop
                    continue;
                }
            }

            Console.WriteLine("Please enter your date of birth. Please enter it in the following format: MM/DD/YYYY");
            while (!validDate && !exitStringEntered)
            {
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    exitStringEntered = CheckIfExitString(input, exitString);
                    validDate = CheckValidDate(input, u);
                }
                else
                {
                    input = "";         //Prevents NULL from getting to the while loop condition
                    continue;
                }
            }

            Console.WriteLine("Please enter your role. Valid selections are Patron, Librarian, or Administrator. ");
            while (!validRole && !exitStringEntered)
            {
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    exitStringEntered = CheckIfExitString(input, exitString);
                    validRole = CheckValidRole(input, u, list);
                }
                else
                {
                    input = "";     //Prevents NULL from getting to the while loop condition
                    continue;
                }
            }

            Console.WriteLine("Please enter your username. Your username must be unique. ");
            while (!validUsername && !exitStringEntered)
            {
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    exitStringEntered = CheckIfExitString(input, exitString);
                    validUsername = CheckValidUsername(input, u, list);
                }
                else
                {
                    input = "";     //Prevents NULL from getting to the while loop condition
                    continue;
                }
            }

            Console.WriteLine("Please enter your password. It can be whatever you'd like. ");
            while (!validPassword && !exitStringEntered)
            {
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    exitStringEntered = CheckIfExitString(input, exitString);
                    validPassword = CheckValidPassword(input, u);
                }
                else
                {
                    input = "";     //Prevents NULL from getting to the while loop condition
                    continue;
                }
            }

            if (validName && validDate && validRole && validUsername && validPassword && !exitStringEntered)
            {
                Console.WriteLine("Registration Complete");
                list.Add(u);
            }
            else
                Console.WriteLine("Registration Aborted");
        }

        public bool CheckIfExitString(string input, string exitString)  //Maybe make this private because you don't want anyone else calling this function unless they've checked that the input string is not NULL
        {
            if (input == exitString)
                return true;
            else
                return false;
        }
        public bool CheckValidName(string input, User u)
        {
            string[] words = input.Split(" ");
            if (words.Length == 2)
            {
                u.Firstname = words[0];
                u.Lastname = words[1];
                return true;
            }
            else
            {
                Console.WriteLine("Invalid name entry. Please enter your first name, followed by a space, followed by your last name.");
                return false;
            }
        }

        public bool CheckValidDate(string input, User u)
        {
            int year = 0;
            int month = 0;
            int day = 0;

            string[] words = input.Split("/");
            if (words.Length == 3)
            {
                month = Convert.ToInt32(words[0]);
                day = Convert.ToInt32(words[1]);
                year = Convert.ToInt32(words[2]);

                if (month >= 1 && month <= 12)
                {
                    if (year >= 1900 && year <= DateTime.Now.Year)
                    {
                        if (day >= 1 && day <= DateTime.DaysInMonth(year, month))
                        {
                            u.DOB = new DateTime(year, month, day);
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid day. Please ensure that the day entered corresponds to an actual day (Ex. February has 28 days, but 29 days for a leap year)");
                            return false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid year. Please enter a year from 1900 to the current year.");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid month. Please ensure that the month is a number between 1 and 12 inclusively.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Invalid date entry. Please enter your date of birth in the following format: MM/DD/YYYY.");
                return false;
            }

        }

        public bool CheckValidRole(string input, User u, List<User> list)
        {

            if (Enum.GetNames(typeof(Role)).Contains(input))
            {
                Role r;
                switch (input)
                {
                    case "Patron":
                        r = Role.Patron;
                        break;
                    case "Administrator":
                        r = Role.Administrator;
                        break;
                    case "Librarian":
                        r = Role.Librarian;
                        break;
                    default:
                        r = Role.Patron;
                        break;
                }
                if (r == Role.Patron)
                {
                    u.Role = Role.Patron;
                    return true;
                }
                else
                {
                    //Need credentials from an administrator
                    Console.WriteLine("Need administrator approval for this action.");
                    if (AdminApproval(list))
                    {
                        if (r == Role.Administrator)
                            u.Role = Role.Administrator;
                        else
                            u.Role = Role.Librarian;
                        return true;
                    }
                    else
                        return false;
                }
            }
            else
            {
                Console.WriteLine("Invalid role entry. Please enter one of Patron, Librarian, or Administrator.");
                return false;
            }

        }

        public bool CheckValidUsername(string input, User u, List<User> list)
        {
            string[] words = input.Split(" ");
            if (words.Length == 1)
            {
                int userIndex = UsernameIndex(words[0], list);
                if (userIndex == -1)             //Username wasn't found in the list, so it's available
                {
                    u.Username = words[0];
                    return true;
                }
                else
                {
                    Console.WriteLine("Username provided is not unique. Try again.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Invalid username entry. Make sure that there are no spaces in the username, and make sure that it is unique.");
                return false;
            }
        }

        public bool CheckValidPassword(string input, User u)
        {
            Console.WriteLine("Please retype your password to confirm");
            string input_retype = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                if (input == input_retype)
                {
                    u.Password = input;
                    return true;
                }
                else
                {
                    Console.WriteLine("Passwords don't match, try again.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Invalid input, try again");
                return false;
            }
        }

        public bool AdminApproval(List<User> list)
        {
            bool approval = false;

            Console.WriteLine("Please type the username for an administrator");
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                int listIndex = UsernameIndex(input, list);
                if (listIndex >= 0)
                {
                    if (list[listIndex].Role == Role.Administrator)
                    {
                        Console.WriteLine("Please type the password for " + input);
                        input = Console.ReadLine();
                        if (!string.IsNullOrEmpty(input))
                        {
                            if (input == list[listIndex].Password)
                            {
                                Console.WriteLine("Approval accepted");
                                approval = true;
                            }
                            else
                                Console.WriteLine("Invalid password, approval denied");
                        }
                        else
                            Console.WriteLine("Invalid input, approval denied");
                    }
                    else
                        Console.WriteLine("Not an administrator, approval denied");
                }
                else
                    Console.WriteLine("Username does not exist, approval denied");
            }
            else
                Console.WriteLine("Invalid input, approval denied.");

            return approval;
        }

        public bool AdminOrLibrarianApproval(List<User> list)
        {
            bool approval = false;

            Console.WriteLine("Please type the username for an administrator");
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                int listIndex = UsernameIndex(input, list);
                if (listIndex >= 0)
                {
                    if (list[listIndex].Role == Role.Administrator || list[listIndex].Role == Role.Librarian)
                    {
                        Console.WriteLine("Please type the password for " + input);
                        input = Console.ReadLine();
                        if (!string.IsNullOrEmpty(input))
                        {
                            if (input == list[listIndex].Password)
                            {
                                Console.WriteLine("Approval accepted");
                                approval = true;
                            }
                            else
                                Console.WriteLine("Invalid password, approval denied");
                        }
                        else
                            Console.WriteLine("Invalid input, approval denied");
                    }
                    else
                        Console.WriteLine("Not an administrator, approval denied");
                }
                else
                    Console.WriteLine("Username does not exist, approval denied");
            }
            else
                Console.WriteLine("Invalid input, approval denied.");

            return approval;
        }

        public int UsernameIndex(string username, List<User> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Username == username)
                    return i;
                else
                    continue;
            }
            return -1;

        }

        public void LogInAsRegisteredUser(List<User> list)
        {
            Console.WriteLine("Please enter your username");
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                int userIndex = UsernameIndex(input, list);
                if (userIndex >= 0)
                {
                    Console.WriteLine("Please enter your password");
                    input = Console.ReadLine();
                    if (!string.IsNullOrEmpty(input))
                    {
                        if (input == list[userIndex].Password)
                        {
                            Console.WriteLine("Welcome to the library. You're logged in now.");
                            list[userIndex].IsLoggedIn = true;
                        }
                        else
                            Console.WriteLine("Incorrect password");

                    }
                    else
                        Console.WriteLine("Invalid input, try again");
                }
                else
                    Console.WriteLine("Username does not exist. Register your username and try again.");
            }
            else
                Console.WriteLine("Invalid input, try again");
        }

        public void LogInAsGuest(List<User> list)
        {
            User Guest = new User();                //The default role is Guest
            string input = "";
            string exitString = "exit";
            bool exitStringEntered = false;
            bool validName = false;

            Console.WriteLine("Please enter your first and last name. Please include a space between your first and last names.");
            while (!validName && !exitStringEntered)
            {
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    exitStringEntered = CheckIfExitString(input, exitString);
                    validName = CheckValidName(input, Guest);   //if user types the exit string, then validName will simply remain false and upone the next while loop iteration the condition will evaluate to false
                }
                else
                {
                    input = "";         //Prevent accessing NULL at the while loop
                    continue;
                }
            }

            if (validName && !exitStringEntered)
            {   //Assign random username
                Utility U = new Utility();
                Random random = new Random();
                int num = 0;
                string username = "";
                int index = 0;

                while (index >= 0)
                {
                    num = random.Next();
                    username = "GuestUser" + Convert.ToString(num);
                    index = U.UsernameIndex(username, list);
                }
                Guest.Username = username;
                Console.WriteLine("Welcome guest! Your username is " + Guest.Username);
                list.Add(Guest);
            }
            else
                Console.WriteLine("Guest login aborted");
        }

        public void LogOut(List<User> list)
        {
            Console.WriteLine("Please enter your username");
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                int userIndex = UsernameIndex(input, list);
                if (userIndex >= 0)
                {
                    Console.WriteLine(list[userIndex].Username + " is now logged out");
                    list[userIndex].IsLoggedIn = false;
                }

                else
                    Console.WriteLine("Username does not exist. Register your username and try again.");
            }
            else
                Console.WriteLine("Invalid input, try again");
        }

        public int GuestIndex(User u, List<User> list)
        {
            if (u.Role == Role.Guest)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    if (list[i].Firstname == u.Firstname && list[i].Lastname == u.Lastname)
                        return i;
                    else
                        continue;
                }
                return -1;
            }
            else
                return -1;
        }

    }
    public class User
    {
        private string _firstname;
        private string _lastname;
        private DateTime _dob;
        private Role _role;
        private bool _isLoggedIn;
        private string _username;
        private string _password;
        private int _userId;

        public string Firstname { get { return _firstname; } set { _firstname = value; } }
        public string Lastname { get { return _lastname; } set { _lastname = value; } }
        public DateTime DOB { get { return _dob; } set { _dob = value; } }
        public Role Role { get { return _role; } set { _role = value; } }
        public string Username { get { return _username; } set { _username = value; } }
        public string Password { get { return _password; } set { _password = value; } }
        public bool IsLoggedIn { get { return _isLoggedIn; } set { _isLoggedIn = value; } }
        public int UserID { get { return _userId; } set { _userId = value; } }

        public User()
        {
            _firstname = "";
            _lastname = "";
            _dob = new DateTime();
            _role = Role.Guest;
            _username = "";
            _password = "";
            _isLoggedIn = false;

            Random ran = new Random();
            _userId = ran.Next(1000000000,2000000000);
        }
        public User(string F, string L, DateTime Dob, Role R)
        {
            _firstname = F;
            _lastname = L;
            _dob = Dob;
            _role = R;
            _isLoggedIn = false;

            Random ran = new Random();
            _userId = ran.Next(1000000000, 2000000000);
        }

        public User(string F, string L, DateTime Dob, Role R, string Username, string Password)
        {
            _firstname = F;
            _lastname = L;
            _dob = Dob;
            _role = R;
            _username = Username;
            _password = Password;
            _isLoggedIn = false;

            Random ran = new Random();
            _userId = ran.Next(1000000000, 2000000000);
        }

        public User(string F, string L, DateTime Dob, Role R, string Username, string Password, bool IsLoggedIn)
        {
            _firstname = F;
            _lastname = L;
            _dob = Dob;
            _role = R;
            _username = Username;
            _password = Password;
            _isLoggedIn = IsLoggedIn;

            Random ran = new Random();
            _userId = ran.Next(1000000000, 2000000000);
        }

    }

    public class Book
    {
        private string _title;
        private string _genre;
        private bool _isCheckedOut;
        private int _bookId;
        private int _userId;

        public string Title { get { return _title; } set { _title = value; } }
        public string Genre { get { return _genre; } set { _genre = value; } }
        public bool IsCheckedOut { get { return _isCheckedOut; } set { _isCheckedOut = value; } }
        public int BookID { get { return _bookId; } set { _bookId = value; } }
        public int UserID { get { return _userId; } set { _userId = value; } }

        public Book()
        {
            _title = "";
            _genre = "";
            _isCheckedOut = false;
            _userId = 0;

            Random ran = new Random();
            _bookId = ran.Next(1000000000, 2000000000);
        }
        public Book(string T, string G)
        {
            _title = T;
            _genre = G;
            _isCheckedOut = false;
            _userId = 0;

            Random ran = new Random();
            _bookId = ran.Next(1000000000, 2000000000);
        }

        public Book(string T, string G, bool IsCheckedOut)
        {
            _title = T;
            _genre = G;
            _isCheckedOut = IsCheckedOut;
            _userId = 0;

            Random ran = new Random();
            _bookId = ran.Next(1000000000, 2000000000);
        }

    }

    public class Printer
    {
        public Printer() { }

        public void MainMenu()
        {
            Console.WriteLine(" ");
            Console.WriteLine("Welcome to the library console application. Listed below are the following menu options:");
            Console.WriteLine("g  -------------------- Log in as guest");
            Console.WriteLine("l  -------------------- Log in as registered user");
            Console.WriteLine("o  -------------------- Log out");
            Console.WriteLine("r  -------------------- Register as a patron");
            Console.WriteLine("pp -------------------- Print profile");
            Console.WriteLine("pr -------------------- Print roles");
            Console.WriteLine("pu -------------------- Print all users");
            Console.WriteLine("add role -------------- Add role to Roles table");
            Console.WriteLine("add user -------------- Add user to Users table");
            Console.WriteLine("add book -------------- Add book to Books table");
            Console.WriteLine("delete role ----------- Delete role in Roles table");
            Console.WriteLine("delete user ----------- Delete user in Users table");
            Console.WriteLine("delete book ----------- Delete book in Books table");
            Console.WriteLine("update role ----------- Update role in Roles table");
            Console.WriteLine("update user ----------- Update all information for a user in Users table");
            Console.WriteLine("update user login ----- Update user login status in Users table");
            Console.WriteLine("update book ----------- Update all information for a book in Books table");
            Console.WriteLine("update book checkout -- Update user login status in Users table");
            Console.WriteLine("view roles ------------ View contents of Roles table");
            Console.WriteLine("view users ------------ View contents of Users table");
            Console.WriteLine("view books ------------ View contents of Books table");


            Console.WriteLine("m  -- Print main menu");
            Console.WriteLine("e  -- Exit");
        }

        public void PrintUser(User u)
        {
            Console.WriteLine("Name: " + u.Firstname + " " + u.Lastname + "     Role: " + u.Role);
        }

        public void PrintUserProfile(User u)
        {
            Console.WriteLine("Here is the complete user profile");
            Console.WriteLine("Name:      " + u.Firstname + " " + u.Lastname);
            Console.WriteLine("DOB:       " + u.DOB);
            Console.WriteLine("Role:      " + u.Role);
            Console.WriteLine("Username:  " + u.Username);
        }

        public void PrintRoles()
        {
            Console.WriteLine("Here are the possible roles for users");
            foreach (var item in Enum.GetNames(typeof(Role)))
            {
                Console.WriteLine(item);
            }
        }

        public void BadInput()
        {
            Console.WriteLine("Invalid input, try again");
        }

        public void ExitMessage()
        {
            Console.WriteLine("Thank you for using the library console application. Goodbye.");
        }
    }

   
}