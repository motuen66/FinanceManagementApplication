using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class IncomeTransactionService : IIncomeTransactionService
    {
        public IEnumerable<IncomeTransaction> GetAllIncomeTransaction( int UserId)
        {
            return IncomeTransactionDAO.GetAllIncomeTransaction(UserId);
        }

        public void CreateIncomeTransaction(IncomeTransaction incomeTransaction)
        {
            IncomeTransactionDAO.CreateIncomeTransaction(incomeTransaction);
        }
    }
}
