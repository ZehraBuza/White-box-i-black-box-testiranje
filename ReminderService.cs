using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace ToDoLista
{

    public class ReminderService
    {
        private List<Reminder> reminders;

        public ReminderService()
        {
            reminders = new List<Reminder>();
        }

        // Kreiranje podsjetnika
        public void CreateReminder(DateTime vrijemePodsjetnika, Task zadatak)
        {
            try
            {
                var reminder = new Reminder(vrijemePodsjetnika, zadatak);
                reminders.Add(reminder);
                Console.WriteLine("Podsjetnik uspješno kreiran.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri kreiranju podsjetnika: {ex.Message}");
            }
        }

        // Provjera i ispis podsjetnika
        public void CheckReminders()
        {
            foreach (var reminder in reminders)
            {
                reminder.ProvjeriPodsjetnik();
            }
        }


        public void PrintAllReminders()
        {
            if (reminders.Count == 0)
            {
                Console.WriteLine("Nema podsjetnika.");
                return;
            }

            Console.WriteLine("Lista svih podsjetnika:");
            foreach (var reminder in reminders)
            {
                string status = reminder.PodsjetnikIzvrsen ? "Izvršen" : "Nije izvršen";
                Console.WriteLine($"Podsjetnik za zadatak '{reminder.Zadatak.naziv}' - Rok: {reminder.VrijemePodsjetnika}, Status: {status}");
            }
        }

        public List<Reminder> GetReminders()
        {
            return reminders;
        }

        // Brisanje završeno zadatka
        public void CompleteTask(string taskName)
            {
                var reminder = reminders.FirstOrDefault(r => r.Zadatak.naziv == taskName);
                if (reminder != null)
                {
                    // Korišćenje refleksije za postavljanje privatnog svojstva
                    var taskType = reminder.Zadatak.GetType();
                    var zavrsenProperty = taskType.GetProperty("zavrsen", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    if (zavrsenProperty != null)
                    {
                        // Postavljanje vrijednosti svojstva zavrsen na true
                        zavrsenProperty.SetValue(reminder.Zadatak, true);
                        Console.WriteLine($"Zadatak '{taskName}' je završen i podsjetnik je obrisan.");
                        reminders.Remove(reminder); // Brisanje podsjetnika
                    }
                    else
                    {
                        Console.WriteLine($"Svojstvo 'zavrsen' nije pronađeno u zadatku '{taskName}'.");
                    }
                }
                else
                {
                    Console.WriteLine($"Zadatak '{taskName}' nije pronađen.");
                }
            }
        }
    }
