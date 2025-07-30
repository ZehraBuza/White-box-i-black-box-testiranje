using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ToDoLista;

namespace ToDoLista
{
    public class UserService : IUserService
    {
      
        public string FilePath;

        private List<User> users;

        public UserService()
        {
            FilePath = GetTempFilePath();
            
            users = new List<User>();
            users = LoadUsers();
        }

        // Dohvatanje svih korisnika
        public List<User> GetUsers()
        {
            return users;
        }

        // Registracija korisnika
        private const int MinimumPasswordLength = 6;
        private HashSet<string> usernames = new HashSet<string>();
        private HashSet<string> emails = new HashSet<string>();

        public void RegisterUser(User user)
        {
            // Validacija lozinke
            ValidatePassword(user.Password);

            // Provjera duplikata korisničkog imena i emaila
            ValidateDuplicate(user);

            // Dodavanje korisnika
            users.Add(user);
            usernames.Add(user.Username);
            emails.Add(user.Email);

            SaveUsers(users);
        }

        private void ValidatePassword(string password)
        {
            if (password.Length < MinimumPasswordLength)
            {
                throw new ArgumentException("Lozinka mora imati najmanje 6 karaktera.");
            }
        }

        private void ValidateDuplicate(User user)
        {
          
            if (usernames.Contains(user.Username))
            {
                throw new ArgumentException("Korisničko ime već postoji.");
            }

           
            if (emails.Contains(user.Email))
            {
                throw new ArgumentException("Email već postoji.");
            }
        }



        // Prijava korisnika
        public User Login(string username, string password)
        {
            return users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        // Učitavanje korisnika iz CSV fajla
        private List<User> LoadUsers()
        {
            if (!File.Exists(FilePath))
            {
                return new List<User>();
            }

            var userList = new List<User>();
            var lines = File.ReadAllLines(FilePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var parts = line.Split(',');

                if (parts.Length != 3) // CSV format: Username,Email,Password
                {
                    continue; // Preskoči linije koje nemaju tačno 3 dijela
                }

                if (string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]) || string.IsNullOrWhiteSpace(parts[2]))
                {
                    continue; // Preskoči linije sa praznim poljima
                }

                if (userList.Exists(u => u.Username == parts[0]))
                {
                    continue; // Preskoči duplikate korisničkih imena
                }

                userList.Add(new User(parts[0], parts[1], parts[2], new List<Task>()));
            }

            return userList;
        }

        private void LogWarning(string message)
        {
            Console.WriteLine($"Warning: {message}");
        }

        private void LogError(string message)
        {
            Console.WriteLine($"Error: {message}");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Spašavanje korisnika u CSV fajl
        public void SaveUsers(List<User> users)
        {
            var lines = users.Select(u => $"{u.Username},{u.Email},{u.Password}");
            File.WriteAllLines(FilePath, lines);
        }

        // Napredni algoritam filtriranja korisnika po kriterijumu
        public List<User> FilterUsers(string keyword)
        {
            // Učitavanje korisnika iz CSV fajla
            var allUsers = LoadUsers();

            // Filtriranje korisnika prema ključnom pojmu (ako je unesen)
            var filteredUsers = allUsers
                .Where(u => string.IsNullOrEmpty(keyword) ||
                            u.Username.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                            u.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                .OrderBy(u => u.Username) // Sortiranje po korisničkom imenu
                .ToList();

            return filteredUsers;
        }

        // Generisanje jedinstvene putanje za fajl
        public static string GetTempFilePath()
        {

            return Path.Combine(Path.GetTempPath(), $"users_{Guid.NewGuid()}.csv");
        }

    }
}
