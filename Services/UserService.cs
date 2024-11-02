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

        public void CreateUser(User user) => UserDAO.CreateNewUser(user);

        public void DeleteUser(int id) => UserDAO.DeleteUser(id);


        public User GetUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
