using System;
using ToDoLista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoLista;

public class Statistic
{
    private List<Task> tasks;

    public Statistic(List<Task> tasks)
    {
        this.tasks = tasks;
    }


    public int GetCompletedTasksCount()
    {
        return tasks.Count(task => task.zavrsen);
    }

    public double ProcenatZavrsenih()
    {
        if (tasks.Count() == 0) return 0;
        return (tasks.Count(task => task.zavrsen) / tasks.Count()) * 100;
    }

    public double GetPendingTasksCount()
    {
        return tasks.Count(task => !task.zavrsen);
    }

    public int ProcenatNezavrsenih()
    {
        if (tasks.Count() == 0) return 0;
        return (tasks.Count(task => !task.zavrsen) / tasks.Count()) * 100;
    }



    public Kategorija? GetCategoryWithMostCompletedTasks()
    {

        var categoryWithMost = tasks.Where(task => task.zavrsen)
                    .GroupBy(task => task.kategorija)
                    .OrderByDescending(group => group.Count())
                    .FirstOrDefault();
        return categoryWithMost?.Key;
    }

    public Kategorija? GetCategoryWithLeastCompletedTasks()
    {
        var categoryWithLeast = tasks.Where(task => !task.zavrsen)
                                 .GroupBy(task => task.kategorija)
                                 .OrderByDescending(group => group.Count())
                                 .FirstOrDefault();

        return categoryWithLeast?.Key;
    }

    public Dictionary<string, int> AnalyzeTasksByCategoryAndStatus(bool status)
    {
        const string NoTasksMessage = "Nema zadataka koji odgovaraju zadatom statusu.";
        const string SingleCategoryMessage = "Postoji samo jedna kategorija sa odgovarajućim zadacima.";
        const string MultipleCategoriesMessage = "Postoji više kategorija sa odgovarajućim zadacima.";

        var categoryCounts = GetCategoryCountsByStatus(status);

        DisplayStatusMessage(categoryCounts.Count);

        return categoryCounts;
    }

    private Dictionary<string, int> GetCategoryCountsByStatus(bool status)
    {
        var categoryCounts = new Dictionary<string, int>();

        foreach (var task in tasks.Where(task => task.zavrsen == status))
        {
            string taskCategory = task.kategorija.ToString();

            if (categoryCounts.TryGetValue(taskCategory, out int currentCount))
            {
                categoryCounts[taskCategory] = currentCount + 1;
            }
            else
            {
                categoryCounts[taskCategory] = 1;
            }
        }

        return categoryCounts;
    }

    private void DisplayStatusMessage(int categoryCount)
    {
        const string NoTasksMessage = "Nema zadataka koji odgovaraju zadatom statusu.";
        const string SingleCategoryMessage = "Postoji samo jedna kategorija sa odgovarajućim zadacima.";
        const string MultipleCategoriesMessage = "Postoji više kategorija sa odgovarajućim zadacima.";

        if (categoryCount == 0)
        {
            Console.WriteLine(NoTasksMessage);
        }
        else if (categoryCount == 1)
        {
            Console.WriteLine(SingleCategoryMessage);
        }
        else
        {
            Console.WriteLine(MultipleCategoriesMessage);
        }
    }


}
