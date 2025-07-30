using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace ToDoLista
{
    class Program
    {
        static void Main(string[] args)
        {
            UserService userservice = new UserService();
            TaskService taskService = new TaskService();
            Statistic statistic = new Statistic(taskService.getList());
            User loggedInUser = null;
            ReminderService reminderService = new ReminderService();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Opcije:");
                Console.WriteLine("1 - Registracija korisnika");
                Console.WriteLine("2 - Prijava korisnika");
                Console.WriteLine("3 - Rad sa taskovima kao gost");
                Console.WriteLine("4 - Izlaz");
                Console.Write("Izaberite opciju: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": // Registracija korisnika
                        Console.Write("Unesite korisničko ime: ");
                        var usernameTemp = Console.ReadLine();
                        Console.Write("Unesite lozinku: ");
                        var passwordTemp = Console.ReadLine();
                        Console.Write("Unesite email: ");
                        var emailTemp = Console.ReadLine();
                        try
                        {
                            var tempUser = new User(usernameTemp, emailTemp, passwordTemp, new List<Task>());
                            userservice.RegisterUser(tempUser);
                            Console.WriteLine("Korisnik uspešno registrovan!");
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"Greška: {ex.Message}");
                            Console.WriteLine("Pritisnite Enter za povratak na meni.");
                            Console.ReadLine();
                        }
                        break;


                    case "2": // Prijava korisnika
                        Console.Write("Unesite korisničko ime: ");
                        usernameTemp = Console.ReadLine();
                        Console.Write("Unesite lozinku: ");
                        passwordTemp = Console.ReadLine();
                        loggedInUser = userservice.Login(usernameTemp, passwordTemp);
                        if (loggedInUser != null)
                        {
                            Console.WriteLine($"Dobrodošli, {loggedInUser.Username}!");
                            Console.WriteLine("Pritisnite Enter za nastavak.");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Nevalidno korisničko ime ili lozinka.");
                            Console.WriteLine("Pritisnite Enter za povratak na meni.");
                            Console.ReadLine();
                        }
                        break;


                    case "3": // Rad sa taskovima kao gost
                        Console.WriteLine("Radite sa taskovima kao gost. Nema potrebe za prijavom.");
                        break;

                    case "4": // Izlaz
                        return;

                    default:
                        Console.WriteLine("Nevalidan unos, pokušajte ponovo.");
                        continue; // Povratak na osnovni meni
                }

                // Glavni meni aplikacije
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("To-Do List Aplikacija");
                    Console.WriteLine("1. Dodaj task");
                    Console.WriteLine("2. Brisanje taska");
                    Console.WriteLine("3. Čekiraj task");
                    Console.WriteLine("4. Pregled taskova");
                    Console.WriteLine("5. Pretraga taskova");
                    Console.WriteLine("6. Sortiraj taskove");
                    Console.WriteLine("7. Napravi podsjetnik");
                    Console.WriteLine("8. Statistika");
                    Console.WriteLine("9. Izlaz");
                    Console.Write("Izaberite opciju: ");

                    string taskChoice = Console.ReadLine();

                    switch (taskChoice)
                    {
                        case "1": // Dodavanje taska
                            Console.Clear();
                            Console.Write("Unesite naziv taska: ");
                            string naziv = Console.ReadLine();
                            Console.Write("Unesite važnost (1-5): ");
                            int vaznost;
                            while (true)
                            {
                                Console.Write("Unesite važnost (1-5): ");
                                if (int.TryParse(Console.ReadLine(), out vaznost) && vaznost >= 1 && vaznost <= 5)
                                    break;
                                Console.WriteLine("Pogrešan unos. Molimo unesite broj između 1 i 5.");
                            }
                            DateTime datum;
                            while (true)
                            {
                                Console.Write("Unesite datum (yyyy-mm-dd): ");
                                if (DateTime.TryParse(Console.ReadLine(), out datum))
                                    break;
                                Console.WriteLine("Pogrešan unos. Molimo unesite datum u formatu yyyy-mm-dd.");
                            }
                            Kategorija kategorija;
                            while (true)
                            {
                                Console.WriteLine("Izaberite kategoriju (1 - Obrazovanje, 2 - Domacinstvo, 3 - Sport, 4 - Dogadjaji, 5 - Ostalo): ");
                                if (Enum.TryParse(Console.ReadLine(), out kategorija) && Enum.IsDefined(typeof(Kategorija), kategorija))
                                    break;
                                Console.WriteLine("Pogrešan unos. Molimo unesite broj između 1 i 5.");
                            }
                            taskService.addTask(new Task(taskService.getList().Count, naziv, vaznost, datum, kategorija, false));
                            Console.WriteLine("Task je uspešno dodat!");
                            break;
                        case "2":
                            Console.Write("Unesite ID taska za brisanje: ");
                            string input = Console.ReadLine();

                            // Provjeravamo da li je unos validan broj koristeći int.TryParse
                            if (int.TryParse(input, out int id))
                            {
                                try
                                {
                                    // Pokušavamo obrisati task po ID-u
                                    taskService.deleteTask(id);
                                }
                                catch (ArgumentException ex)
                                {
                                    Console.WriteLine($"Greška: {ex.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Uneseni ID nije validan broj!");
                            }
                            break;

                        case "3":
                            Console.Write("Unesite ID taska za čekiranje: ");
                            string checkInput = Console.ReadLine();

                            if (int.TryParse(checkInput, out int checkId))
                            {
                                try
                                {
                                    taskService.checkTask(checkId);
                                    Console.WriteLine($"Task s ID-om {checkId} označen kao završen.");
                                }
                                catch (ArgumentException ex)
                                {
                                    Console.WriteLine($"Greška: {ex.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Uneseni ID nije validan broj!");
                            }
                            break;

                        case "4": // Pregled taskova
                            Console.Clear();
                            Console.WriteLine("Lista svih taskova:");
                            foreach (var task in taskService.getList())
                            {
                                string status = task.zavrsen ? "Završen" : "Nije završen";
                                Console.WriteLine($"ID: {task.ID}, Naziv: {task.naziv}, Važnost: {task.vaznost}, Kategorija: {task.kategorija}, Status: {status}");
                            }
                            break;

                        case "5": // Pretraga taskova
                            Console.Clear();
                            Console.WriteLine("1. Pretraga po ID-u");
                            Console.WriteLine("2. Pretraga po Nazivu");
                            Console.Write("Izaberite opciju: ");
                            string searchChoice = Console.ReadLine();

                            if (searchChoice == "1")
                            {
                                Console.Write("Unesite ID za pretragu: ");
                                input = Console.ReadLine();
                                if (int.TryParse(input, out id))
                                {
                                    try
                                    {
                                        Task foundTask = taskService.SearchById(taskService.getList(), id);
                                        string status = foundTask.zavrsen ? "Završen" : "Nije završen";
                                        Console.WriteLine($"Pronađen task: ID: {foundTask.ID}, Naziv: {foundTask.naziv}, Važnost: {foundTask.vaznost}, Kategorija: {foundTask.kategorija}, Status: {status}");
                                    }
                                    catch (InvalidDataException ex)
                                    {
                                        Console.WriteLine($"Greška: {ex.Message}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Uneseni ID nije validan broj!");
                                }
                            }
                            else if (searchChoice == "2")
                            {
                                Console.Write("Unesite naziv taska: ");
                                try
                                {
                                    string nazivSearch = Console.ReadLine();
                                    Task foundTask = taskService.SearchByName(taskService.getList(), nazivSearch);
                                    string status = foundTask.zavrsen ? "Završen" : "Nije završen";
                                    Console.WriteLine($"Pronađen task: ID: {foundTask.ID}, Naziv: {foundTask.naziv}, Važnost: {foundTask.vaznost}, Kategorija: {foundTask.kategorija}, Status: {status}");
                                }
                                catch (InvalidDataException ex)
                                {
                                    Console.WriteLine($"Greška: {ex.Message}");
                                }
                            }
                            break;

                        case "6": // Sortiranje taskova
                            Console.Clear();
                            Console.WriteLine("1. Sortiraj po datumu");
                            Console.WriteLine("2. Sortiraj po važnosti");
                            Console.Write("Izaberite opciju: ");

                            try
                            {
                                string sortChoice = Console.ReadLine();
                                int opcija = int.Parse(sortChoice);
                                var sorted = taskService.Sort(taskService.getList(), opcija);

                                if (opcija == 1)
                                {
                                    Console.WriteLine("Taskovi sortirani po datumu:");
                                    foreach (var task in sorted)
                                    {
                                        Console.WriteLine($"{task.naziv} - {task.datum.ToShortDateString()}");
                                    }
                                }
                                else if (opcija == 2)
                                {
                                    Console.WriteLine("Taskovi sortirani po važnosti:");
                                    foreach (var task in sorted)
                                    {
                                        Console.WriteLine($"{task.naziv} - Važnost: {task.vaznost}");
                                    }
                                }
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.WriteLine($"Greška: {ex.Message}");
                            }

                            break;

                        case "7":
                            // Provjeriti podsjetnike do sutra
                            bool imaPodsjetnika = false;
                            foreach (var reminder in reminderService.GetReminders())
                            {
                                if (reminder.IsDueTomorrow())
                                {
                                    imaPodsjetnika = true;
                                    Console.WriteLine($"Podsjetnik za zadatak '{reminder.Zadatak.naziv}' je postavljen za sutra.");
                                }
                            }

                            if (!imaPodsjetnika)
                            {
                                Console.WriteLine("Nemate podsjetnika za sutra.");
                            }

                            // Opcije nakon prikaza podsjetnika
                            Console.WriteLine("Odaberite opciju:");
                            Console.WriteLine("1 - Kreiraj novi podsjetnik");
                            Console.WriteLine("2 - Brisanje postojećeg podsjetnika");
                            Console.WriteLine("3 - Pregled podsjetnika");
                            var subOption = Console.ReadLine();
                            switch (subOption)
                            {
                                case "1":
                                    Console.Write("Unesite ID zadatka za podsjetnik: ");
                                    string taskIdInput = Console.ReadLine();

                                    if (int.TryParse(taskIdInput, out int taskId))
                                    {
                                        // Potraži zadatak po ID-u
                                        Task task = taskService.getList().FirstOrDefault(t => t.ID == taskId);

                                        if (task != null)
                                        {
                                            // Ako zadatak postoji, unesi vrijeme podsjetnika
                                            DateTime reminderTime;
                                            while (true)
                                            {
                                                Console.Write("Unesite vrijeme podsjetnika (yyyy-mm-dd hh:mm): ");
                                                if (DateTime.TryParse(Console.ReadLine(), out reminderTime) && reminderTime > DateTime.Now)
                                                {
                                                    reminderService.CreateReminder(reminderTime, task);
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Pogrešan unos. Molimo unesite validno vrijeme u budućnosti.");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Zadatak s tim ID-em nije pronađen.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nevalidan ID zadatka.");
                                    }
                                    break;

                                case "2":
                                    // Brisanje postojećeg podsjetnika
                                    Console.WriteLine("Unesite naziv zadatka za koji želite obrisati podsjetnik:");
                                    naziv = Console.ReadLine();
                                    reminderService.CompleteTask(naziv);
                                    break;

                                case "3":
                                    // Pregled postojećih podsjetnika
                                    reminderService.PrintAllReminders();
                                    break;

                                default:
                                    Console.WriteLine("Nepoznata opcija.");
                                    break;
                            }
                            break;




                        case "8": // Statistika
                            Console.Clear();
                            Console.WriteLine($"Broj završenih taskova: {statistic.GetCompletedTasksCount()} ");
                            Console.WriteLine($"Broj nezavršenih taskova: {statistic.GetPendingTasksCount()} ");
                            Console.WriteLine($"Kategorija s najviše završenih taskova: {statistic.GetCategoryWithMostCompletedTasks()}");
                            Console.WriteLine($"Kategorija s najmanje završenih taskova: {statistic.GetCategoryWithLeastCompletedTasks()}");

                            break;

                     


                        case "9": // Povratak na osnovni meni
                            Console.WriteLine("Povratak na osnovni meni...");
                            goto EndOfInnerLoop;

                        default:
                            Console.WriteLine("Nevažeća opcija, pokušajte ponovo.");
                            break;
                    }

                    Console.WriteLine("\nPritisnite bilo koji taster za povratak u meni...");
                    Console.ReadKey();
                }

            EndOfInnerLoop:
                continue; // Povratak na osnovni meni
            }
        }
    }
}