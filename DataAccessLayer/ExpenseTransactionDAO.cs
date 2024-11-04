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
        public static List<ExpenseTransaction> GetExpenseTransactions()
        {
            var ExpenseTransactions = new List<ExpenseTransaction>();
            try
            {
                using var context = new FinanceManagementApplicationContext();
                return ExpenseTransactions = context.ExpenseTransactions
                                            .Include(e => e.Budget)
                                            .Include(e => e.User)
                                            .ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewTransaction(ExpenseTransaction transaction)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
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
                var context = new FinanceManagementApplicationContext();
                var TransactionToUpdate = context.ExpenseTransactions.FirstOrDefault(t => t.UserId == transaction.UserId);
                if (TransactionToUpdate != null)
                {
                    context.ExpenseTransactions.Update(TransactionToUpdate);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Transaction does not exist");
                }
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
                var context = new FinanceManagementApplicationContext();
                var TransactionToDelete = context.ExpenseTransactions.FirstOrDefault(t => t.UserId == transaction.UserId);
                if (TransactionToDelete != null)
                {
                    context.ExpenseTransactions.Remove(TransactionToDelete);
                    context.SaveChanges();
                }
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
