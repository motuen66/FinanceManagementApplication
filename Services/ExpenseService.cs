using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ExpenseService : IExpenseService
    {
        public ExpenseService() { }
        public void CreateNewBudget(BudgetItem budget) 
            => BudgetItemDAO.CreateNewBudget(budget);

        public void CreateNewExpenseTransaction(ExpenseTransaction expenseTransaction) 
            => ExpenseTransactionDAO.CreateNewTransaction(expenseTransaction);

        public void DeleteBudget(BudgetItem budget) 
            => BudgetItemDAO.DeleteBudget(budget);

        public void DeleteExpenseTransaction(ExpenseTransaction expenseTransaction)
            => ExpenseTransactionDAO.DeleteTransaction(expenseTransaction);

        public BudgetItem GetBudgetItemById(int id)
            => BudgetItemDAO.GetBudgetItemById(id);

        public List<BudgetItem> GetBudgets()
            => BudgetItemDAO.GetBudgetItems();

        public ExpenseTransaction GetExpenseTransactionById(int id)
            => ExpenseTransactionDAO.GetTransactionById(id);

        public List<ExpenseTransaction> GetExpenseTransactions()
            => ExpenseTransactionDAO.GetExpenseTransactions();

        public void UpdateBudget(BudgetItem budget)
            => BudgetItemDAO.UpdateBudget(budget);

        public void UpdateExpenseTransaction(ExpenseTransaction expenseTransaction)
            => ExpenseTransactionDAO.UpdateTransaction(expenseTransaction);
    }
}
