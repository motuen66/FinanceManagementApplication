using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService
    {
        public UserService() { }
        public List<User> getAllUsers() => UserDAO.getAllUsers();
    }
}
