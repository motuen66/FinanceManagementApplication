using BusinessObjects;
using DataAccessLayer;

namespace TestDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new FinanceManagementApplicationContext();
            List<User> users = context.Users.ToList();
            foreach (var user in users) { 
                Console.WriteLine(user.Username);
            }
        }
    }
}
