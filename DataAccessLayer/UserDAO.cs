using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static void UpdateUser(User newUser)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                User user = context.Users.FirstOrDefault(u => u.Id == newUser.Id);
                if (user != null)
                {
                    context.Users.Update(user);
                    context.SaveChanges();
                }else
                {
                    throw new Exception("User does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
        public static User? getUserById(int id)
        {
            using var context = new FinanceManagementApplicationContext();
            return (context.Users.FirstOrDefault(u => id.Equals(u.Id) ));
        }

        public static void AddBalance(IncomeTransaction transaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var user = context.IncomeTransactions.Add(transaction);

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
