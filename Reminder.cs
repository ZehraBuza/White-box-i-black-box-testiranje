using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLista
{
    public class Reminder
    {
        public DateTime VrijemePodsjetnika { get; private set; }
        public Task Zadatak { get; private set; }
        public bool PodsjetnikIzvrsen { get; private set; }

        public Reminder(DateTime vrijemePodsjetnika, Task zadatak)
        {
            ValidacijaPodataka(vrijemePodsjetnika, zadatak);

            VrijemePodsjetnika = vrijemePodsjetnika;
            Zadatak = zadatak;
            PodsjetnikIzvrsen = false;
        }



        private void ValidacijaPodataka(DateTime vrijemePodsjetnika, Task zadatak)
        {
            if (zadatak == null)
                throw new ArgumentException("Reminder mora biti vezan za validan zadatak!");

            if (zadatak.zavrsen)
                throw new ArgumentException("Ne možete kreirati podsjetnik za već završen zadatak!");

            if (vrijemePodsjetnika <= DateTime.Now)
                throw new ArgumentException("Vrijeme podsjetnika mora biti u budućnosti!");
        }



        // Provjera da li je podsjetnik za sutra
        public bool IsDueTomorrow()
        {
            var tomorrow = DateTime.Now.AddDays(1).Date;
            return VrijemePodsjetnika.Date == tomorrow;
        }


        public void ProvjeriPodsjetnik()
        {
            if (!PodsjetnikIzvrsen && VrijemePodsjetnika <= DateTime.Now)
            {
                PodsjetnikIzvrsen = true;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.ResetColor();
            }
        }
    }
}
