using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BudgetItemDAO
    {
        public static List<BudgetItem> GetBudgetItems()
        {
            var budgetItems = new List<BudgetItem>();
            try
            {
                using var context = new FinanceManagementApplicationContext();
                return budgetItems = context.BudgetItems.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewBudget(BudgetItem budget)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.BudgetItems.Add(budget);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateBudget(BudgetItem budget)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var BudgetToUpdate = context.BudgetItems.FirstOrDefault(
                    b => b.Id == budget.Id);
                if (BudgetToUpdate != null)
                {
                    context.BudgetItems.Update(BudgetToUpdate);
                    context.SaveChanges();
                } else
                {
                    throw new Exception("Budget is not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteBudget(BudgetItem budget)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var BudgetToDelete = context.BudgetItems.FirstOrDefault(
                    b => b.Id == budget.Id);
                if (BudgetToDelete != null)
                {
                    context.BudgetItems.Remove(BudgetToDelete);
                    context.SaveChanges();
                } else
                {
                    throw new Exception("Budget is not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static BudgetItem GetBudgetItemById(int id)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var budget = context.BudgetItems.FirstOrDefault(b => b.Id == id);

                return budget;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
