using System.Collections.Generic;

namespace ToDoLista
{
    public interface ITaskService
    {
        List<Task> getList();
        void addTask(Task task);
        void deleteTask(int id);
        void checkTask(int id);
        public List<Task> Sort(List<Task> taskovi, int opcija);
        Task SearchById(List<Task> taskovi, int id);
        Task SearchByName(List<Task> taskovi, string naziv);
    }
}
