using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IExpenseService
    {
        //Function for BudgetItems
        void CreateNewBudget(BudgetItem budget);
        void UpdateBudget(BudgetItem budget);
        void DeleteBudget(BudgetItem budget);
        List<BudgetItem> GetBudgets();
        BudgetItem GetBudgetItemById(int id);

        //Function for Expense Transaction
        void CreateNewExpenseTransaction(ExpenseTransaction expenseTransaction);
        void UpdateExpenseTransaction(ExpenseTransaction expenseTransaction);
        void DeleteExpenseTransaction(ExpenseTransaction expenseTransaction);
        List<ExpenseTransaction> GetExpenseTransactions();
        ExpenseTransaction GetExpenseTransactionById(int id);
    }
}
