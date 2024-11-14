using BusinessObjects;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ExpenseTransactionDAO
    {
        public static List<ExpenseTransaction> GetExpenseTransactions(int userId)
        {
            using var context = new FinanceManagementApplicationContext();
            return context.ExpenseTransactions
                          .Include(e => e.Budget)
                          .Include(e => e.User)
                          .Where(e => e.UserId == userId)
                          .ToList();
        }


        public static void CreateNewTransaction(ExpenseTransaction transaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.ExpenseTransactions.Add(transaction);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateTransaction(ExpenseTransaction transaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.ExpenseTransactions.Update(transaction);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteTransaction(ExpenseTransaction transaction) 
        { 
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.ExpenseTransactions.Remove(transaction);
                context.SaveChanges();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static ExpenseTransaction GetTransactionById(int userId)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                return context.ExpenseTransactions.FirstOrDefault(t => t.UserId == userId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
