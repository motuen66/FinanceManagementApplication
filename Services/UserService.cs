using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        public UserService() { }

        public void CreateNewFinanceRecord(FinanceRecord financeRecord)
            => FinanceRecordDAO.CreateNewFinanceRecord(financeRecord);

        public void CreateUser(User user) 
            => UserDAO.CreateNewUser(user);

        public void DeleteFinanceRecord(FinanceRecord financeRecord)
            => FinanceRecordDAO.DeleteFinanceRecord(financeRecord);

        public void DeleteUser(int id) 
            => UserDAO.DeleteUser(id);

        public FinanceRecord GetFinanceRecordByUserId(int id)
            => FinanceRecordDAO.GetFinanceRecordByUserId(id);

        public List<FinanceRecord> GetFinanceRecords()
            => FinanceRecordDAO.GetFinanceRecords();

        public User GetUser(string userName, string password) 
            => UserDAO.getUser(userName, password);

        public void UpdateFinanceRecord(FinanceRecord financeRecord)
            => FinanceRecordDAO.UpdateFinanceRecord(financeRecord);

        public void UpdateUser(User user) 
            => UserDAO.UpdateUser(user);

        public void AddBalance(IncomeTransaction transaction) 
            => UserDAO.AddBalance(transaction);

        public User GetUserById(int id)
            => UserDAO.getUserById(id);
    }
}
