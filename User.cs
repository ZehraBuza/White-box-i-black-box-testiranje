using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ToDoLista
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Task> ToDoLista { get; set; } 

        public User(string username, string email, string password, List<Task> toDoLista)
        {
            ValidateData(username, email, password);
            this.Username = username;
            this.Email = email;
            this.Password = password;
            this.ToDoLista = toDoLista;
     
        }

        private void ValidateData(string username, string email, string password)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");


            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Unesite korisničko ime!");
            }

            if (username.Length < 6)
            {
                throw new ArgumentException("Korisnicko ime mora imati minimalno 6 karaktera.");
            }

            if (!emailRegex.IsMatch(email))
            {
                throw new ArgumentException("Email mora biti u  validnom formatu!");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Morate unijeti password");
            }

            if (password.Length < 8)
            {
                throw new ArgumentException("Password mora imati najmanje 8 karaktera.");
            }
        }

        public override string ToString()
        {
            return $"Username: '{Username}', Password: '{Password}', Email: '{Email}'";
        }

        public void AddTask(Task task)
        {
            ToDoLista.Add(task);
        }

       
    }
}

