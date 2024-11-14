using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IIncomeTransactionService
    {
        IEnumerable<IncomeTransaction> GetAllIncomeTransaction(int UserId);

        void CreateIncomeTransaction(IncomeTransaction transaction);
    }
}
