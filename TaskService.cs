using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace ToDoLista
{
    public class TaskService : ITaskService
    {
        private List<Task> listaTaskova = new List<Task>();
        private ITaskArchive taskArchive;

        public void setTaskArchive(ITaskArchive t){taskArchive=t;}
        public ITaskArchive getTaskArchive(){return taskArchive;}
        public List<Task> getList() { return listaTaskova ; }
        public void addTask(Task task) {
            if (task == null) throw new ArgumentException("Neispravan task.");
            listaTaskova.Add(task);
        }

        public void deleteTask(int id)
        {
               if (id < 0 || id >= listaTaskova.Count) throw new ArgumentException("ID ne postoji.");
                listaTaskova.RemoveAt(id);
                Console.WriteLine("Task uspješno obrisan.");
            
        }


        public void checkTask(int id)
        { 
                if (id < 0 || id >= listaTaskova.Count) throw new ArgumentException("ID ne postoji.");
                listaTaskova[id].zavrsen = true;
        }



        public List<Task> Sort(List<Task> taskovi, int opcija)
        {
            if (taskovi == null) throw new ArgumentException("Neispravna lista.");
            if (taskovi.Count == 0) return taskovi;
            switch (opcija)
            {
                case 1:
                    InsertionSortDatum(taskovi);
                    break;
                case 2:
                    InsertionSortVaznost(taskovi);
                    break;
                default:
                    throw new InvalidOperationException("Odabrana opcija ne postoji");
            }

            return taskovi;
        }

        private void InsertionSortDatum(List<Task> taskovi)
        {

            for (int i = 1; i < taskovi.Count; i++)
            {
                Task trenutni = taskovi[i];
                int j = i - 1;

                while (j >= 0 && taskovi[j].datum > trenutni.datum)
                {
                    taskovi[j + 1] = taskovi[j];
                    j--;
                }
                taskovi[j + 1] = trenutni;
            }
        }

      


        public Task SearchById(List<Task> taskovi, int id)
        {
            var task = taskovi.FirstOrDefault(t => t.ID == id);
            if (task == null)
            {
                throw new InvalidDataException("Task nije pronađen");
            }
            return task;
        }


        public Task SearchByName(List<Task> taskovi, string naziv)
        {
            var task = taskovi.FirstOrDefault(t => t.naziv.Equals(naziv, StringComparison.OrdinalIgnoreCase));
            if (task == null)
            {
                throw new InvalidDataException($"Task sa nazivom '{naziv}' nije pronadjen");
            }
            return task;
        }

        public void InsertionSortVaznost(List<Task> taskovi)
        {
            if (taskovi == null || taskovi.Count <= 1)
            {
                return;
            }
            foreach (var task in taskovi)
            {
                if (task.vaznost < 1 || task.vaznost > 5)
                {
                    throw new ArgumentOutOfRangeException("Vaznost zadatka mora biti između 1 i 5.");
                }
            }

            for (int i = 1; i < taskovi.Count; i++)
            {
                Task trenutni = taskovi[i];
                int j = i - 1;

                while (j >= 0 && taskovi[j].vaznost > trenutni.vaznost)
                {
                    taskovi[j + 1] = taskovi[j];
                    j--;
                }
                taskovi[j + 1] = trenutni;

                if (trenutni.zavrsen == true)
                {
                    continue;
                }

            }

        }

    }
}
