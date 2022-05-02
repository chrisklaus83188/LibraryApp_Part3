using LibraryApp_ClassLibrary_General;
using System.Data.SqlClient;
using System.Data;

namespace LibraryApp_ClassLibrary_Database
{
    public class ClassDb
    {
        public ClassDb() { }
        private static void CreateCommand(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public bool checkDB()
        {
            var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (var con = new SqlConnection(connString))
                {
                    con.Open();
                    string commandText = "UPDATE Roles SET RoleName = @newName "   + "WHERE RoleID = 0;";   //UPDATE Roles SET RoleName = @newName "   + "WHERE RoleID = 0;
                    using (var com = new SqlCommand(commandText, con))
                    {
                        com.Parameters.Add("@newName", SqlDbType.VarChar);
                        com.Parameters["@newName"].Value = "Something Else";
                        // use your query here...
                        Console.WriteLine("The output of the ExecuteNonQuery is " + com.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class UserData
    {
        

        private List<User> _data;
        private Printer _p;
        private Utility _U;

        public List<User> Data { get { return _data; } }
        public Printer P { get { return _p; } }
        public Utility U { get { return _U; } }

        public UserData()
        {
            _data = new List<User>();
            _p = new Printer();
            _U = new Utility();
            _data.Add(new User("Initial", "Admin", new DateTime(), Role.Administrator, "InitialAdmin", "SuperSecretAdminPassword"));
        }
        public void PrintUsers()
        {
            for (int i = 0; i < _data.Count; i++)
                P.PrintUser(_data[i]);
            Console.WriteLine(" ");
        }

        public void DeleteUser(User u)
        {
            if (_data.Contains(u))
                _data.Remove(u);
            else
                Console.WriteLine("User does not exist, deletion failed");
        }

        public void UpdateUsername(User u, string NewUsername)
        {
            int index = U.UsernameIndex(u.Username, this._data);
            if (index >= 0)
            {
                Console.WriteLine("Username updated to " + NewUsername);
                u.Username = NewUsername;
            }
            else
                Console.WriteLine("User not found");
        }

        public void UpdateFirstname(User u, string NewFirstname)
        {
            int index = U.UsernameIndex(u.Username, this._data);
            if (index >= 0)
            {
                Console.WriteLine("Firstname updated to " + NewFirstname);
                u.Firstname = NewFirstname;
            }
            else
                Console.WriteLine("User not found");

        }

        public void UpdateLastname(User u, string NewLastname)
        {
            int index = U.UsernameIndex(u.Username, this._data);
            if (index >= 0)
            {
                Console.WriteLine("Lastname updated to " + NewLastname);
                u.Lastname = NewLastname;
            }
            else
                Console.WriteLine("User not found");

        }

        public void UpdateDOB(User u, DateTime date)
        {
            int index = U.UsernameIndex(u.Username, this._data);
            if (index >= 0)
            {
                Console.WriteLine("DOB updated to " + date);
                u.DOB = date;
            }
            else
                Console.WriteLine("User not found");

        }

        public void UpdateRole(User u, Role r)
        {
            int index = U.UsernameIndex(u.Username, this._data);
            if (index >= 0)
            {
                Console.WriteLine("DOB updated to " + r);
                u.Role = r;
            }
            else
                Console.WriteLine("User not found");
        }

        public User ReturnUser(string username)
        {
            int index = U.UsernameIndex(username, this._data);
            if (index >= 0)
            {
                return this._data[index];
            }
            else
                return null;            //User not found, need to return something
        }

        public User CreateUser(string F, string L, DateTime Dob, Role R, string Username, string Password)
        {
            return new User(F, L, Dob, R, Username, Password);
        }

        public User CreateBlankUser()
        {
            User u = new User();
            Random random = new Random();
            int num = 0;
            string username = "";
            int index = 0;

            while (index >= 0)
            {
                num = random.Next();
                username = "NewUser" + Convert.ToString(num);
                index = U.UsernameIndex(username, this._data);
            }
            return u;
        }




        public bool AddException(DateTime Occurence, string ExceptionType, string FunctionName, string ConnectionString)
        {
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();

                    string commandText = "EXEC AddException @newOccurence = @argOccurence, @newExceptionType = @argExceptionType, @newFunctionName = @argFunctionName;";   //UPDATE Roles SET RoleName = @newName "   + "WHERE RoleID = 0;


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        storedCommand.Parameters.Add("@argOccurence", SqlDbType.DateTime);
                        storedCommand.Parameters["@argOccurence"].Value = Occurence;

                        storedCommand.Parameters.Add("@argExceptionType", SqlDbType.VarChar);
                        storedCommand.Parameters["@argExceptionType"].Value = ExceptionType;

                        storedCommand.Parameters.Add("@argFunctionName", SqlDbType.VarChar);
                        storedCommand.Parameters["@argFunctionName"].Value = FunctionName;

                        // use your query here...
                        Console.WriteLine("The output of the AddException ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }













        public bool UpdateRole(int RoleID, string RoleName, string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();

                    Dictionary<int, string> current_roles = ReadRoles(ConnectionString);
                    foreach (KeyValuePair<int, string> id in current_roles)
                    {
                        if (RoleID == id.Key)
                            throw new IDAlreadyExistsException();
                    }

                    string commandText = "EXEC UpdateRole @roleIDtoUpdate = @argRoleID, @newRoleName = @argRoleName;";   //UPDATE Roles SET RoleName = @newName "   + "WHERE RoleID = 0;


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        storedCommand.Parameters.Add("@roleIDtoUpdate", SqlDbType.Int);
                        storedCommand.Parameters["@roleIDtoUpdate"].Value = RoleID;

                        storedCommand.Parameters.Add("@argRoleName", SqlDbType.VarChar);
                        storedCommand.Parameters["@argRoleName"].Value = RoleName;

                        // use your query here...
                        Console.WriteLine("The output of the UpdateRole ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (IDAlreadyExistsException)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "IDAlreadyExistsException";
                string FunctionName = "CreateRole";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
            catch (Exception)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "Unknown Exception";
                string FunctionName = "CreateRole";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
        }

        public bool UpdateUserGeneral(int UserID, string Firstname, string Lastname, DateTime DOB, Role R, string Username, string Password, bool IsLoggedIn, string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();

                    List<User> current_users = ReadUsers(ConnectionString);
                    bool userFound = false;
                    for (int i = 0; i < current_users.Count; i++)
                    {
                        if (current_users[i].Username == Username)
                        {
                            userFound = true;
                            break;
                        }
                        else
                            continue;
                    }
                    if (!userFound)
                        throw new IDNotFoundException();

                    string commandText = "EXEC UpdateUserGeneral @userIDtoUpdate = @argUserID, @newLastName = @argLastname, @newFirstName = @argFirstname, @newDOB = @argDOB, @newRoleID = @argRoleID, @newUsername = @argUsername, @newPassword = @argPassword, @newIsLoggedIn = @argIsLoggedIn; ";


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        storedCommand.Parameters.Add("@argUserID", SqlDbType.Int);
                        storedCommand.Parameters["@argUserID"].Value = UserID;

                        storedCommand.Parameters.Add("@argFirstname", SqlDbType.VarChar);
                        storedCommand.Parameters["@argFirstname"].Value = Firstname;

                        storedCommand.Parameters.Add("@argLastname", SqlDbType.VarChar);
                        storedCommand.Parameters["@argLastname"].Value = Lastname;

                        storedCommand.Parameters.Add("@argDOB", SqlDbType.DateTime);
                        storedCommand.Parameters["@argDOB"].Value = DOB;

                        storedCommand.Parameters.Add("@argRoleID", SqlDbType.Int);
                        storedCommand.Parameters["@argRoleID"].Value = (int)R;

                        storedCommand.Parameters.Add("@argUsername", SqlDbType.VarChar);
                        storedCommand.Parameters["@argUsername"].Value = Username;

                        storedCommand.Parameters.Add("@argPassword", SqlDbType.VarChar);
                        storedCommand.Parameters["@argPassword"].Value = Password;

                        storedCommand.Parameters.Add("@argIsLoggedIn", SqlDbType.Bit);
                        storedCommand.Parameters["@argIsLoggedIn"].Value = Convert.ToInt32(IsLoggedIn);

                        // use your query here...
                        Console.WriteLine("The output of the UpdateUser ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (IDNotFoundException)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "IDNotFoundException";
                string FunctionName = "UpdateUserGeneral";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
            catch (Exception)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "Unknown Exception";
                string FunctionName = "UpdateUserGeneral";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
        }

        public bool UpdateUserLoggedIn(bool IsLoggedIn, string Username, string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();

                    List<User> current_users = ReadUsers(ConnectionString);
                    bool userFound = false;
                    for (int i = 0; i < current_users.Count; i++)
                    {
                        if (current_users[i].Username == Username)
                        {
                            userFound = true;
                            break;
                        }
                        else
                            continue;
                    }
                    if (!userFound)
                        throw new IDNotFoundException();

                    string commandText = "EXEC UpdateUserLoginStatus @userName = @inputUserName, @LogInStatus = @inputLoginStatus;";   //UPDATE Roles SET RoleName = @newName "   + "WHERE RoleID = 0;


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        storedCommand.Parameters.Add("@inputUserName", SqlDbType.VarChar);
                        storedCommand.Parameters["@inputUserName"].Value = Username;

                        storedCommand.Parameters.Add("@inputLoginStatus", SqlDbType.Bit);
                        storedCommand.Parameters["@inputLoginStatus"].Value = Convert.ToInt32(IsLoggedIn);



                        // use your query here...
                        Console.WriteLine("The output of the UpdateUserLoginStatus ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (IDNotFoundException)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "IDNotFoundException";
                string FunctionName = "UpdateUserLoggedIn";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
            catch (Exception)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "Unknown Exception";
                string FunctionName = "UpdateUserLoggedIn";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
        }

        public bool DeleteRole(int RoleID, string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();


                    string commandText = "EXEC DeleteRole @roleIDtoDelete = @newRoleID;";


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        storedCommand.Parameters.Add("@newRoleID", SqlDbType.Int);
                        storedCommand.Parameters["@newRoleID"].Value = RoleID;

                        // use your query here...
                        Console.WriteLine("The output of the DELETE Role ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateRole(int RoleID, string RoleName, string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();

                    Dictionary<int, string> current_roles = ReadRoles(ConnectionString);
                    foreach(KeyValuePair<int, string> id in current_roles)
                    {
                        if (RoleID == id.Key)
                            throw new IDAlreadyExistsException();
                    }

                    string commandText = "EXEC AddRole @newRoleID = @argRoleID, @newRoleName = @argRoleName;";   //UPDATE Roles SET RoleName = @newName "   + "WHERE RoleID = 0;


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        storedCommand.Parameters.Add("@argRoleID", SqlDbType.Int);
                        storedCommand.Parameters["@argRoleID"].Value = RoleID;

                        storedCommand.Parameters.Add("@argRoleName", SqlDbType.VarChar);
                        storedCommand.Parameters["@argRoleName"].Value = RoleName;

                        // use your query here...
                        Console.WriteLine("The output of the AddRole ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (IDAlreadyExistsException)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "IDAlreadyExistsException";
                string FunctionName = "CreateRole";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
            catch (Exception)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "Unknown Exception";
                string FunctionName = "CreateRole";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
        }

        public bool CreateNewUser(int UserID, string Firstname, string Lastname, DateTime DOB, Role R, string Username, string Password, string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();

                    List<User> current_users = ReadUsers(ConnectionString);
                    for(int i = 0; i < current_users.Count; i++)
                    {
                        if (current_users[i].Username == Username)
                            throw new UsernameAlreadyExistsException();
                        else if (current_users[i].UserID == UserID)
                            throw new IDAlreadyExistsException();
                        else
                            continue;
                    }

                    string commandText = "EXEC AddUser @newUserID = @argUserID, @newLastName = @argLastname, @newFirstName = @argFirstname, @newDOB = @argDOB, @newRoleID = @argRoleID, @newUsername = @argUsername, @newPassword = @argPassword; ";


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        storedCommand.Parameters.Add("@argUserID", SqlDbType.Int);
                        storedCommand.Parameters["@argUserID"].Value = UserID;

                        storedCommand.Parameters.Add("@argLastname", SqlDbType.VarChar);
                        storedCommand.Parameters["@argLastname"].Value = Lastname;

                        storedCommand.Parameters.Add("@argFirstname", SqlDbType.VarChar);
                        storedCommand.Parameters["@argFirstname"].Value = Firstname;

                        storedCommand.Parameters.Add("@argDOB", SqlDbType.DateTime);
                        storedCommand.Parameters["@argDOB"].Value = DOB;

                        storedCommand.Parameters.Add("@argRoleID", SqlDbType.Int);
                        storedCommand.Parameters["@argRoleID"].Value = (int)R;

                        storedCommand.Parameters.Add("@argUsername", SqlDbType.VarChar);
                        storedCommand.Parameters["@argUsername"].Value = Username;

                        storedCommand.Parameters.Add("@argPassword", SqlDbType.VarChar);
                        storedCommand.Parameters["@argPassword"].Value = Password;

                        // use your query here...
                        Console.WriteLine("The output of the AddUser ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (UsernameAlreadyExistsException)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "UsernameAlreadyExistsException";
                string FunctionName = "CreateUser";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
            catch (IDAlreadyExistsException)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "IDAlreadyExistsException";
                string FunctionName = "CreateUser";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
            catch (Exception)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "Unknown Exception";
                string FunctionName = "CreateUser";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
        }

        public bool DeleteUser(int UserID, string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();


                    string commandText = "EXEC DeleteUser @userIDtoDelete = @argUserID;";


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        storedCommand.Parameters.Add("@argUserID", SqlDbType.Int);
                        storedCommand.Parameters["@argUserID"].Value = UserID;

                        // use your query here...
                        Console.WriteLine("The output of the DELETE User ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Dictionary<int,string> ReadRoles(string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            Dictionary<int, string> data = new Dictionary<int, string>();

            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();

                    string commandText = "EXEC SelectAllRoles;";  

                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {

                        // use your query here...
                        using (SqlDataReader reader = storedCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                data.Add((int)reader["RoleID"], reader["RoleName"].ToString());
                            }
                            reader.Close();
                        }
                    }
                }
                return data;
            }
            catch (Exception)
            {
                return data;
            }
        }

        public List<User> ReadUsers(string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            List<User> data = new List<User>();

            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();

                    string commandText = "EXEC SelectAllUsers;";

                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {

                        // use your query here...
                        using (SqlDataReader reader = storedCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                data.Add(new User(reader["FirstName"].ToString(), reader["LastName"].ToString(), (DateTime)reader["DOB"], (Role)reader["RoleID"], reader["Username"].ToString(), reader["Password"].ToString(), (bool)reader["IsLoggedIn"]));
                            }
                            reader.Close();
                        }
                    }
                }
                return data;
            }
            catch (Exception)
            {
                return data;
            }
        }







        public void UI_AddRole(string ConnectionString)
        {
            List<User> list = ReadUsers(ConnectionString);
            if (this.U.AdminApproval(list))
            {
                Console.WriteLine("Please enter a new RoleID");
                int RoleID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Please enter a new RoleName");
                string RoleName = Console.ReadLine();

                if (!String.IsNullOrEmpty(RoleName))
                    CreateRole(RoleID, RoleName, ConnectionString);
            }
            else
                Console.WriteLine("You do not have permission to perform this action");
        }
        public void UI_AddUser(string ConnectionString)
        {
            Console.WriteLine("Please enter a new UserID");
            int UserID = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Please enter a new Firstname");
            string Firstname = Console.ReadLine();
            Console.WriteLine("Please enter a new Lastname");
            string Lastname = Console.ReadLine();
            Console.WriteLine("Please enter a new DOB");
            DateTime DOB = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Please enter a new Role");
            string RoleString = Console.ReadLine();
            Role newRole = Enum.TryParse(RoleString, true, out Role result) ? result : Role.Guest;
            Console.WriteLine("Please enter a new Username");
            string Username = Console.ReadLine();
            Console.WriteLine("Please enter a new Password");
            string Password = Console.ReadLine();

            if(!String.IsNullOrEmpty(Firstname) && !String.IsNullOrEmpty(Lastname) && !String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password))
                CreateNewUser(UserID, Firstname, Lastname, DOB, newRole, Username, Password, ConnectionString);
        }

        public void UI_DeleteRole(string ConnectionString)
        {
            List<User> list = ReadUsers(ConnectionString);
            if (this.U.AdminApproval(list))
            {
                Console.WriteLine("Please enter a RoleID to delete");
                int RoleID = Convert.ToInt32(Console.ReadLine());

                DeleteRole(RoleID, ConnectionString);
            }
            else
                Console.WriteLine("You do not have permission to perform this action");
        }

        public void UI_DeleteUser(string ConnectionString)
        {
            List<User> list = ReadUsers(ConnectionString);
            if (this.U.AdminApproval(list))
            {
                Console.WriteLine("Please enter a UserID to delete");
                int UserID = Convert.ToInt32(Console.ReadLine());

                DeleteUser(UserID, ConnectionString);
            }
            else
                Console.WriteLine("You do not have permission to perform this action");
        }

        public void UI_UpdateRole(string ConnectionString)
        {
            List<User> list = ReadUsers(ConnectionString);
            if (this.U.AdminApproval(list))
            {
                Console.WriteLine("Please enter a RoleID to update");
                int RoleID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Please enter a RoleName to update");
                string RoleName = Console.ReadLine();

                UpdateRole(RoleID, RoleName, ConnectionString);
            }
            else
                Console.WriteLine("You do not have permission to perform this action");
        }
        public void UI_UpdateUser(string ConnectionString)
        {
            List<User> list = ReadUsers(ConnectionString);
            if (this.U.AdminOrLibrarianApproval(list))
            {
                Console.WriteLine("Please enter a new UserID");
                int UserID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Please enter a new Firstname");
                string Firstname = Console.ReadLine();
                Console.WriteLine("Please enter a new Lastname");
                string Lastname = Console.ReadLine();
                Console.WriteLine("Please enter a new DOB");
                DateTime DOB = Convert.ToDateTime(Console.ReadLine());
                Console.WriteLine("Please enter a new Role");
                string RoleString = Console.ReadLine();
                Role newRole = Enum.TryParse(RoleString, true, out Role result) ? result : Role.Guest;
                Console.WriteLine("Please enter a new Username");
                string Username = Console.ReadLine();
                Console.WriteLine("Please enter a new Password");
                string Password = Console.ReadLine();
                Console.WriteLine("Please enter the new login status. Type either True or False");
                string stringIsLoggedIn = Console.ReadLine();
                bool IsLoggedIn = false;
                if (stringIsLoggedIn == "True")
                    IsLoggedIn = true;
                else
                    IsLoggedIn = false;

                if (!String.IsNullOrEmpty(Firstname) && !String.IsNullOrEmpty(Lastname) && !String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password))
                    UpdateUserGeneral(UserID, Firstname, Lastname, DOB, newRole, Username, Password, IsLoggedIn, ConnectionString);
            }
            else
                Console.WriteLine("You do not have permission to perform this action");
        }

        public void UI_UpdateUserLoggedIn(string ConnectionString)
        {
            Console.WriteLine("Please enter a Username to update");
            string Username = Console.ReadLine();
            Console.WriteLine("Please enter the new login status. Type either True or False");
            string stringIsLoggedIn = Console.ReadLine();
            bool IsLoggedIn = false;
            if(stringIsLoggedIn == "True")
                IsLoggedIn = true;  
            else
                IsLoggedIn = false;

            if (!String.IsNullOrEmpty(Username))
                UpdateUserLoggedIn(IsLoggedIn, Username, ConnectionString);
        }

        public void UI_ReadRoles(string ConnectionString)
        {
            Dictionary<int, string> data = ReadRoles(ConnectionString);

            Console.WriteLine("Here are the contents of the Roles table");
            Console.WriteLine("----------------------------------------");
            foreach (KeyValuePair<int, string> row in data)
            {
                Console.WriteLine("RoleID: " + row.Key + ", RoleName: " + row.Value);
            }
        }

        public void UI_ReadUsers(string ConnectionString)
        {
            List<User> data = ReadUsers(ConnectionString);

            Console.WriteLine("Here are the contents of the Users table");
            Console.WriteLine("----------------------------------------");
            for (int i = 0; i < data.Count; ++i)
            {
                Console.WriteLine("Firstname: " + data[i].Firstname + ", Lastname: " + data[i].Lastname + ", DOB: " + data[i].DOB + ", Role: " + data[i].Role + ", Username: " + data[i].Username + ", Password: " + data[i].Password + ", IsLoggedIn: " + data[i].IsLoggedIn);
            }
        }















        public string AddBook(int BookID, string Title, string Genre, string ConnectionString)
        {
            string val = "Failed";
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();
                    val = "Opened the connection";

                    string commandText = "EXEC AddBook @newBookID = @argBookID, @newTitle = @argTitle, @newGenre = @argGenre;";


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        val = "Entered command";

                        storedCommand.Parameters.Add("@argBookID", SqlDbType.Int);
                        storedCommand.Parameters["@argBookID"].Value = BookID;

                        storedCommand.Parameters.Add("@argTitle", SqlDbType.VarChar);
                        storedCommand.Parameters["@argTitle"].Value = Title;

                        storedCommand.Parameters.Add("@argGenre", SqlDbType.VarChar);
                        storedCommand.Parameters["@argGenre"].Value = Genre;

                        // use your query here...
                        Console.WriteLine("The output of the AddBook ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                        val = "Executed the procedure";
                    }
                }
                return "Success";
            }
            catch (Exception)
            {
                return val;
            }
        }

        public bool DeleteBook(int BookID, string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();


                    string commandText = "EXEC DeleteBook @bookIDtoDelete = @argBookID;";


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        storedCommand.Parameters.Add("@argBookID", SqlDbType.Int);
                        storedCommand.Parameters["@argBookID"].Value = BookID;

                        // use your query here...
                        Console.WriteLine("The output of the DeleteBook ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateBook(int BookID, string Title, string Genre, bool IsCheckedOut, int UserID, string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();

                    List<Book> current_books = SelectAllBooks(ConnectionString);
                    bool bookFound = false;
                    int currentUserID = 0;
                    for (int i = 0; i < current_books.Count; i++)
                    {
                        if (current_books[i].BookID == BookID)
                        {
                            bookFound = true;
                            currentUserID = current_books[i].UserID;
                            break;
                        }
                        else
                            continue;
                    }
                    if (!bookFound)
                        throw new IDNotFoundException();

                    int UserIDtoPassToSP = UserID;
                    bool UserArgumentExists = false;
                    for (int i = 0; i < current_books.Count; i++)
                    {
                        if (current_books[i].UserID == UserID)
                        {
                            UserArgumentExists = true;  
                            break;
                        }
                        else
                            continue;
                    }
                    if(!UserArgumentExists)
                        UserIDtoPassToSP = currentUserID;


                    string commandText = "EXEC UpdateBook @bookIDtoUpdate = @argBookID, @newTitle = @argTitle, @newGenre = @argGenre, @newIsCheckedOut = @argIsCheckedOut, @newUserID = @argUserID; ";


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        storedCommand.Parameters.Add("@argBookID", SqlDbType.Int);
                        storedCommand.Parameters["@argBookID"].Value = BookID;

                        storedCommand.Parameters.Add("@argTitle", SqlDbType.VarChar);
                        storedCommand.Parameters["@argTitle"].Value = Title;

                        storedCommand.Parameters.Add("@argGenre", SqlDbType.VarChar);
                        storedCommand.Parameters["@argGenre"].Value = Genre;

                        storedCommand.Parameters.Add("@argIsCheckedOut", SqlDbType.Bit);
                        storedCommand.Parameters["@argIsCheckedOut"].Value = Convert.ToInt32(IsCheckedOut);

                        storedCommand.Parameters.Add("@argUserID", SqlDbType.Int);
                        storedCommand.Parameters["@argUserID"].Value = UserIDtoPassToSP;

                        // use your query here...
                        Console.WriteLine("The output of the AddBook ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (IDNotFoundException)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "IDNotFoundException";
                string FunctionName = "UpdateUserLoggedIn";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
            catch (Exception)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "Unknown Exception";
                string FunctionName = "UpdateUserLoggedIn";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
        }

        public bool UpdateBookCheckedOut(int BookIDtoUpdate, bool IsCheckedOut, int UserID, string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();

                    List<Book> current_books = SelectAllBooks(ConnectionString);
                    bool bookFound = false;
                    int currentUserID = 0;
                    for (int i = 0; i < current_books.Count; i++)
                    {
                        if (current_books[i].BookID == BookIDtoUpdate)
                        {
                            bookFound = true;
                            currentUserID = current_books[i].UserID;
                            break;
                        }
                        else
                            continue;
                    }
                    if (!bookFound)
                        throw new IDNotFoundException();

                    int UserIDtoPassToSP = UserID;
                    bool UserArgumentExists = false;
                    if (IsCheckedOut == false)                  //We're going to check in the book, so the UserID needs to be 0
                    {
                        UserIDtoPassToSP = 0;
                    }
                    else
                    {                                           //We're going to check the book out, so the UserID we pass to the SP needs to already exist
                        for (int i = 0; i < current_books.Count; i++)
                        {
                            if (current_books[i].UserID == UserID)
                            {
                                UserArgumentExists = true;
                                break;
                            }
                            else
                                continue;
                        }
                        if (!UserArgumentExists)
                            throw new UtilizeNonExistentUserException();
                    }

                    string commandText = "EXEC UpdateBookIsCheckedOut @bookIDtoUpdate = @argBookIDtoUpdate, @newIsCheckedOut = @argIsCheckedOut, @newUserID = @argUserID;";   //UPDATE Roles SET RoleName = @newName "   + "WHERE RoleID = 0;


                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {
                        storedCommand.Parameters.Add("@argBookIDtoUpdate", SqlDbType.Int);
                        storedCommand.Parameters["@argBookIDtoUpdate"].Value = BookIDtoUpdate;

                        storedCommand.Parameters.Add("@argIsCheckedOut", SqlDbType.Bit);
                        storedCommand.Parameters["@argIsCheckedOut"].Value = Convert.ToInt32(IsCheckedOut);

                        storedCommand.Parameters.Add("@argUserID", SqlDbType.Int);
                        storedCommand.Parameters["@argUserID"].Value = UserIDtoPassToSP;

                        // use your query here...
                        Console.WriteLine("The output of the UpdateBookIsCheckedOut ExecuteNonQuery is " + storedCommand.ExecuteNonQuery());
                    }
                }
                return true;
            }
            catch (IDNotFoundException)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "IDNotFoundException";
                string FunctionName = "UpdateBookCheckedOut";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
            catch (UtilizeNonExistentUserException)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "UtilizeNonExistentUserException";
                string FunctionName = "UpdateBookCheckedOut";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
            catch (Exception)
            {
                DateTime Occurence = DateTime.Now;
                string ExceptionType = "Unknown Exception";
                string FunctionName = "UpdateBookCheckedOut";
                AddException(Occurence, ExceptionType, FunctionName, ConnectionString);
                return false;
            }
        }

        public List<Book> SelectAllBooks(string ConnectionString)
        {
            //var connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            List<Book> data = new List<Book>();

            try
            {
                using (SqlConnection connectionToDB = new SqlConnection(ConnectionString))
                {
                    connectionToDB.Open();

                    string commandText = "EXEC SelectAllBooks;";

                    using (SqlCommand storedCommand = new SqlCommand(commandText, connectionToDB))
                    {

                        // use your query here...
                        using (SqlDataReader reader = storedCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                data.Add(new Book(reader["Title"].ToString(), reader["Genre"].ToString(), (bool)reader["IsCheckedOut"]));
                            }
                            reader.Close();
                        }
                    }
                }
                return data;
            }
            catch (Exception)
            {
                return data;
            }
        }



  
        public void UI_AddBook(string ConnectionString)
        {
            List<User> list = ReadUsers(ConnectionString);
            if (this.U.AdminApproval(list))
            {
                Console.WriteLine("Please enter a new BookID");
                int BookID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Please enter a new Title");
                string Title = Console.ReadLine();
                Console.WriteLine("Please enter a new Genre");
                string Genre = Console.ReadLine();

                if (!String.IsNullOrEmpty(Title) && !String.IsNullOrEmpty(Genre))
                    AddBook(BookID, Title, Genre, ConnectionString);
            }
            else
                Console.WriteLine("You do not have permission to perform this action");  
        }
        public void UI_DeleteBook(string ConnectionString)
        {
            List<User> list = ReadUsers(ConnectionString);
            if (this.U.AdminApproval(list))
            {
                Console.WriteLine("Please enter a new BookID");
                int BookID = Convert.ToInt32(Console.ReadLine());

                DeleteBook(BookID, ConnectionString);
            }
            else
                Console.WriteLine("You do not have permission to perform this action");
        }
        public void UI_UpdateBook(string ConnectionString)
        {
            List<User> list = ReadUsers(ConnectionString);
            if (this.U.AdminApproval(list))
            {
                Console.WriteLine("Please enter a new BookID");
                int BookID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Please enter a new Title");
                string Title = Console.ReadLine();
                Console.WriteLine("Please enter a new Genre");
                string Genre = Console.ReadLine();
                Console.WriteLine("Please enter a new BookID");
                int UserID = Convert.ToInt32(Console.ReadLine());
                string stringIsCheckedOut = Console.ReadLine();
                bool IsCheckedOut = false;
                if (stringIsCheckedOut == "True")
                    IsCheckedOut = true;
                else
                    IsCheckedOut = false;

                if (!String.IsNullOrEmpty(Title) && !String.IsNullOrEmpty(Genre))
                    UpdateBook(BookID, Title, Genre, IsCheckedOut, UserID, ConnectionString);
            }
            else
                Console.WriteLine("You do not have permission to perform this action");
        }
        public void UI_UpdateBookCheckedOut(string ConnectionString)
        {
            List<User> list = ReadUsers(ConnectionString);
            if (this.U.AdminApproval(list))
            {
                Console.WriteLine("Please enter a BookID");
                int BookIDtoUpdate = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Please enter a UserID");
                int UserID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Please enter the new checkout status. Type either True or False");
                string stringIsCheckedOut = Console.ReadLine();
                bool IsCheckedOut = false;
                if (stringIsCheckedOut == "True")
                    IsCheckedOut = true;
                else
                    IsCheckedOut = false;

                UpdateBookCheckedOut(BookIDtoUpdate, IsCheckedOut, UserID, ConnectionString);
            }
            else
                Console.WriteLine("You do not have permission to perform this action");
        }
        public void UI_ReadBooks(string ConnectionString)
        {
            List<Book> data = SelectAllBooks(ConnectionString);

            Console.WriteLine("Here are the contents of the Books table");
            Console.WriteLine("----------------------------------------");
            for (int i = 0; i < data.Count; ++i)
            {
                Console.WriteLine("Title: " + data[i].Title + ", Genre: " + data[i].Genre + ", IsCheckedOut: " + data[i].IsCheckedOut);
            }
        }


    }

   [Serializable]
   public class IDAlreadyExistsException : Exception
    {
        public IDAlreadyExistsException() { }
        public IDAlreadyExistsException(string message) : base(message) { }
        public IDAlreadyExistsException (string message, Exception innerException) : base(message, innerException) { }  

    }

    [Serializable]
    public class IDNotFoundException : Exception
    {
        public IDNotFoundException() { }
        public IDNotFoundException(string message) : base(message) { }
        public IDNotFoundException(string message, Exception innerException) : base(message, innerException) { }

    }

    [Serializable]
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException() { }
        public UsernameAlreadyExistsException(string message) : base(message) { }
        public UsernameAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }

    }

    [Serializable]
    public class UtilizeNonExistentUserException : Exception
    {
        public UtilizeNonExistentUserException() { }
        public UtilizeNonExistentUserException(string message) : base(message) { }
        public UtilizeNonExistentUserException(string message, Exception innerException) : base(message, innerException) { }

    }
}

    

