using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLista
{
    

    public class Task
    {
        public int ID { get; set; }
        public string naziv { get; set; }
        public int vaznost { get; set; }
        public DateTime datum { get; set; }
        public Kategorija kategorija { get; set; }
        public Boolean zavrsen  { get; set; }

        public Task(int iD, string naziv, int vaznost, DateTime datum, Kategorija kategorija, bool zavrsen)
        {
            ID = iD;
            this.naziv = naziv;
            this.vaznost = vaznost;
            this.datum = datum;
            this.kategorija = kategorija;
            this.zavrsen = zavrsen;
        }
      

    }
}
