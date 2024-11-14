using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class IncomeTransactionDAO
    {

        public static void CreateIncomeTransaction(IncomeTransaction transaction)
        {
            using var context = new FinanceManagementApplicationContext();
            context.IncomeTransactions.Add(transaction);
            context.SaveChanges();
        }
        public static IEnumerable<IncomeTransaction> GetAllIncomeTransaction(int UserId)
        {
            using var context = new FinanceManagementApplicationContext();
            return context.IncomeTransactions
                .Where(transaction => transaction.UserId == UserId)
                .ToList();
        }
    }
}
