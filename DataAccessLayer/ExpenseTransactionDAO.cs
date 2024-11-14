using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class ExpenseTransactionDAO
    {
        public static void CreateExpenseTransaction(ExpenseTransaction transaction)
        {
            using var context = new FinanceManagementApplicationContext();
            context.ExpenseTransactions.Add(transaction);
            context.SaveChanges();
        }

        public static void DeleteExpenseTransaction(int id)
        {
            using var context = new FinanceManagementApplicationContext();
            var transaction = context.ExpenseTransactions.FirstOrDefault(t => t.Id == id);
            if (transaction != null)
            {
                context.ExpenseTransactions.Remove(transaction);
                context.SaveChanges();
            }
        }
        public static void UpdateExpenseTransaction(ExpenseTransaction transaction)
        {
            using var context = new FinanceManagementApplicationContext();
            var existingTransaction = context.ExpenseTransactions.FirstOrDefault(t => t.UserId == transaction.UserId);
            if (existingTransaction != null)
            {
                existingTransaction.BudgetId = transaction.BudgetId;
                existingTransaction.Note = transaction.Note;
                existingTransaction.Amount = transaction.Amount;
                existingTransaction.Date = transaction.Date;

                context.SaveChanges();
            }
        }

        public static ExpenseTransaction? GetExpenseTransaction(int id)
        {
            using var context = new FinanceManagementApplicationContext();
            return context.ExpenseTransactions.FirstOrDefault(t => t.UserId == id);
        }

        public static IEnumerable<ExpenseTransaction> GetAllExpenseTransactions()
        {
            using var context = new FinanceManagementApplicationContext();
            return context.ExpenseTransactions
                         .Include(t => t.Budget)
                         .ToList();
        }

        public static List<ExpenseTransaction> GetAllExpenseTransactionsById(int userId)
        {
            using var context = new FinanceManagementApplicationContext();
            return context.ExpenseTransactions
                         .Where(t => t.UserId == userId)
                         .Select(t => new ExpenseTransaction
                         {
                             Id = t.Id,
                             BudgetId = t.BudgetId,
                             Amount = t.Amount,
                             Budget = new BudgetItem
                             {
                                 BudgetName = t.Budget.BudgetName
                             },
                             Date = t.Date,
                             Note = t.Note
                         })
                         .ToList();
        }
    }
}
