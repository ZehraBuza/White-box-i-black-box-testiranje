using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLista
{
    public class Fake : ITaskArchive
    {
        public List<Task> ArchivedTasks { get; private set; } = new List<Task>();

        public void ArchiveTask(Task task)
        {
            ArchivedTasks.Add(task);
        }
    }
}
