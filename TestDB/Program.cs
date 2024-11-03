using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Client;
using Services;

namespace TestDB
{
    internal class Program
    {
        public readonly IUserService userService;

        static void Main(string[] args)
        {
            using var context = new FinanceManagementApplicationContext();
            //List<User> users = context.Users.ToList();
            //foreach (var user in users) { 
            //    Console.WriteLine(user.Username);
            //}

            //ExpenseTransactionDAO dao = new ExpenseTransactionDAO();
            //List<ExpenseTransaction> expenseTransactions = dao.GetExpenseTransactions();
            //foreach (var expenseTransaction in expenseTransactions)
            //{
            //    Console.WriteLine(expenseTransaction.Amount);
            //}
            var incomeSources = SavingTransactionDAO.GetSavingTransactions();
            foreach (var incomeSource in incomeSources)
            {
                Console.WriteLine(incomeSource.Amount);
            }

        }
    }
}
