using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SavingGoalDAO
    {
        public static List<SavingGoal> GetSavingGoals(int userId)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var savingGoals = context.SavingGoals
                                .Include(sg => sg.User)
                                .Where(sg => sg.UserId == userId)
                                .ToList();
                return savingGoals;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void CreateNewSavingGoal(SavingGoal savingGoal)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.SavingGoals.Add(savingGoal);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void UpdateSavingGoal(SavingGoal savingGoal)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.SavingGoals.Update(savingGoal);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteSavingGoal(SavingGoal savingGoal)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                context.SavingGoals.Remove(savingGoal);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static SavingGoal GetSavingGoalById(int userId)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                var SavingGoal = context.SavingGoals.FirstOrDefault(
                                sg => sg.UserId == userId);

                return SavingGoal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
