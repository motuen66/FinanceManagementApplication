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
        public static List<IncomeTransaction> GetIncomeTransactions()
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                var IncomeTransactions = context.IncomeTransactions
                                        .Include(t => t.User)
                                        .ToList();
                return IncomeTransactions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewIncomeTransaction(IncomeTransaction incomeTransaction)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                context.IncomeTransactions.Add(incomeTransaction);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateIncomeTransaction(IncomeTransaction incomeTransaction)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                var incomeTransactionToUpdate = context.IncomeTransactions.FirstOrDefault(
                                                t => t.UserId == incomeTransaction.UserId);
                if (incomeTransactionToUpdate != null)
                {
                    context.IncomeTransactions.Update(incomeTransactionToUpdate);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Income transaction does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteIncomeTransaction(IncomeTransaction incomeTransaction)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                var incomeTransactionToDelete = context.IncomeTransactions.FirstOrDefault(
                                                t => t.UserId == incomeTransaction.UserId);
                if (incomeTransactionToDelete != null)
                {
                    context.IncomeTransactions.Remove(incomeTransactionToDelete);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Income transaction does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static IncomeTransaction GetIncomeTransactionById(int id)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                return context.IncomeTransactions.FirstOrDefault(t => t.UserId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
