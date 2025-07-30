using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLista
{
    public interface IUserService
    {
        List<User> GetUsers();
        void SaveUsers(List<User> users);
        void RegisterUser(User user);
        User Login(string username, string password);

    }
}
