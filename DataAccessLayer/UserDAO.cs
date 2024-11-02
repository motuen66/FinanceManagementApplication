using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserDAO
    {
        public static void CreateNewUser(User newUser)
        {
            using var context = new FinanceManagementApplicationContext();
            context.Users.Add(newUser);
            context.SaveChanges();
        }

        public void UpdateUser(User newUser)
        {

        }

        public static void DeleteUser(int id) 
        {
            using var context = new FinanceManagementApplicationContext();
            User user = context.Users.FirstOrDefault(u => id == u.Id);
            context.Users.Remove(user);
            context.SaveChanges();
        }

        public static User? getUser(string username, string password)
        {
            using var context = new FinanceManagementApplicationContext(); 
            return (context.Users.FirstOrDefault(u => username.Equals(u.Username) && password.Equals(u.Password)));
        }
    }
}
