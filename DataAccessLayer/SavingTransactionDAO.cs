using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SavingTransactionDAO
    {
        public static List<SavingTransaction> GetSavingTransactions()
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var transaction = context.SavingTransactions
                                    .Include(st => st.SavingGoal)
                                    .ToList();
                return transaction;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewSavingTransaction(SavingTransaction savingTransaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.SavingTransactions.Add(savingTransaction);
                context.SaveChanges();
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateSavingTransaction(SavingTransaction savingTransaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var savingTransactionToUpdate = context.SavingTransactions.FirstOrDefault(
                                                st => st.SavingGoalId == savingTransaction.SavingGoalId);
                if (savingTransactionToUpdate != null)
                {
                    context.SavingTransactions.Update(savingTransactionToUpdate);
                    context.SaveChanges();
                } else
                {
                    throw new Exception("Saving transaction already exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteSavingTransaction(SavingTransaction savingTransaction)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var savingTransactionToDelete = context.SavingTransactions.FirstOrDefault(
                                                st => st.SavingGoalId == savingTransaction.SavingGoalId);
                if (savingTransactionToDelete != null)
                {
                    context.SavingTransactions.Remove(savingTransactionToDelete);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Saving transaction already exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static SavingTransaction GetSavingTransactionById(int id)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                return context.SavingTransactions.FirstOrDefault(st => st.SavingGoalId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
