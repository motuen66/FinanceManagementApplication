using BusinessObjects;
using BusinessObjects.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SavingService : ISavingService
    {
        public SavingService() { }

        public void CreateSavingGoal(SavingGoal savingGoal)
            => SavingGoalDAO.CreateNewSavingGoal(savingGoal);

        public void CreateSavingTransaction(SavingTransaction savingTransaction)
            => SavingTransactionDAO.CreateNewSavingTransaction(savingTransaction);

        public void DeleteSavingGoal(SavingGoal savingGoal)
            => SavingGoalDAO.DeleteSavingGoal(savingGoal);

        public void DeleteSavingTransaction(SavingTransaction savingTransaction)
            => SavingTransactionDAO.DeleteSavingTransaction(savingTransaction);

        public SavingGoal GetSavingGoalById(int id)
            => SavingGoalDAO.GetSavingGoalById(id);

        public List<SavingGoal> GetSavingGoals(int userId)
            => SavingGoalDAO.GetSavingGoals(userId);

        public SavingTransaction GetSavingTransactionById(int id)
            => SavingTransactionDAO.GetSavingTransactionById(id);

        public List<SavingTransaction> GetSavingTransactions(int userId)
            => SavingTransactionDAO.GetSavingTransactions(userId);

        public void UpdateSavingGoal(SavingGoal savingGoal)
            => SavingGoalDAO.UpdateSavingGoal(savingGoal);

        public void UpdateSavingTransaction(SavingTransaction savingTransaction)
            => SavingTransactionDAO.UpdateSavingTransaction(savingTransaction);
    }
}
