using Microsoft.VisualStudio.TestTools.UnitTesting;
using LibraryApp_ClassLibrary_Database;
using LibraryApp_ClassLibrary_General;
using System.Collections.Generic;
using System;

namespace LibraryApp_UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            UserData data = new UserData();
            string connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            int expected = 0;
            int actual = 0;

            Dictionary<int, string> current_roles = data.ReadRoles(connString);
            int numRoles = current_roles.Count;
            expected = 4;
            actual = numRoles;

            Assert.AreEqual(expected, actual);

            data.CreateRole(6, "Security Guard", connString);
            current_roles = data.ReadRoles(connString);
            numRoles = current_roles.Count;

            expected = 5;
            actual = numRoles;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod2()
        {
            UserData data = new UserData();
            string connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            int expected = 0;
            int actual = 0;

            Dictionary<int, string> current_roles = data.ReadRoles(connString);
            int numRoles = current_roles.Count;
            expected = 5;
            actual = numRoles;

            Assert.AreEqual(expected, actual);

            data.DeleteRole(6, connString);
            current_roles = data.ReadRoles(connString);
            numRoles = current_roles.Count;

            expected = 4;
            actual = numRoles;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod3()
        {
            UserData data = new UserData();
            string connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            int expected = 0;
            int actual = 0;

            List<User> current_users = data.ReadUsers(connString);
            int numUsers = current_users.Count;
            expected = 3;
            actual = numUsers;

            Assert.AreEqual(expected, actual);


            data.CreateNewUser(3, "John", "Doe", new System.DateTime(2000, 1, 1), Role.Patron, "jd1999", "password123", connString);
            current_users = data.ReadUsers(connString);
            numUsers = current_users.Count;

            expected = 4;
            actual = numUsers;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMethod4()
        {
            UserData data = new UserData();
            string connString = @"Data Source=DESKTOP-9V9K394;Database=LibraryApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            int expected = 0;
            int actual = 0;

            List<User> current_users = data.ReadUsers(connString);
            int numUsers = current_users.Count;
            expected = 4;
            actual = numUsers;

            Assert.AreEqual(expected, actual);

            /*int UserID = -1;
            for(int i = 0; i < current_users.Count; i++)
            {
                if (current_users[i].Username == "jd1999")
                    UserID = current_users[i].UserID;
                else
                    continue;
            }*/

            data.DeleteUser(3, connString);
            current_users = data.ReadUsers(connString);
            numUsers = current_users.Count;

            expected = 3;
            actual = numUsers;

            Assert.AreEqual(expected, actual);
        }

        

    }
}