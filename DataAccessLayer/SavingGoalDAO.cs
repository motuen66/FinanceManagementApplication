using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class SavingGoalDAO
    {
        public static List<SavingGoal> GetSavingGoals()
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                var savingGoals = context.SavingGoals
                                .Include(sg => sg.User)
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
                var context = new FinanceManagementApplicationContext();
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
                var context = new FinanceManagementApplicationContext();
                var SavingGoalToUpdate = context.SavingGoals.FirstOrDefault(
                                        sg => sg.Id == savingGoal.Id);
                if (SavingGoalToUpdate != null)
                {
                    context.SavingGoals.Update(SavingGoalToUpdate);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Saving Goal is not exist");
                }
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
                var context = new FinanceManagementApplicationContext();
                var SavingGoalToRemove = context.SavingGoals.FirstOrDefault(
                                        sg => sg.Id == savingGoal.Id);
                if (SavingGoalToRemove != null)
                {
                    context.SavingGoals.Remove(SavingGoalToRemove);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Saving Goal is not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static SavingGoal GetSavingGoalById(int id)
        {
            try
            {
                var context = new FinanceManagementApplicationContext();
                var SavingGoal = context.SavingGoals.FirstOrDefault(
                                sg => sg.Id == id);

                return SavingGoal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
