using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        void CreateUser(User user);
        void DeleteUser(int id);
        void UpdateUser(User user);
        User GetUser(string userName, string password);
        void CreateNewFinanceRecord(FinanceRecord financeRecord);
        void UpdateFinanceRecord(FinanceRecord financeRecord);
        void DeleteFinanceRecord(FinanceRecord financeRecord);
        List<FinanceRecord> GetFinanceRecords(int userId);
        FinanceRecord GetFinanceRecordByUserId(int id);
        User GetUser(string userName);
    }
}
