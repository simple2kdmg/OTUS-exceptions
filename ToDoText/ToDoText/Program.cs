using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;



namespace ToDoText
{
    using System;

    class Program
    {
        const string _fileName = "todo.txt";

        static void AddTask(string name, DateTime date)
        {
            if (!File.Exists(_fileName)) Console.WriteLine("File doesn't exist.");
            else
            {
                try
                {
                    var task = new ToDoTask(name, date);
                    File.AppendAllLines(_fileName, new[] { $"{task.Date:dd/MM/yy}\t{task.Name}" });
                }
                catch (ArgumentException ae)
                {
                    Console.WriteLine($"Invalid date or time. {ae}");
                }
            }
        }

        static ToDoTask[] GetTasks()
        {
            if (!File.Exists(_fileName))
            {
                Console.WriteLine("File doesn't exist.");
                return new ToDoTask[0];
            }
            else
            {
                var todoTxtLines = File.ReadAllLines(_fileName);
                if (todoTxtLines.Length == 0) return new ToDoTask[0];
                return todoTxtLines.Select(x =>
                    ToDoTask.TryParse(x, out var result) ? result : null
                ).Where(x => x != null).ToArray();
            }  
        }

        static void ListTodayTasks()
        {
            var tasks = GetTasks();
            if (tasks.Length == 0) Console.WriteLine("No tasks available.");
            else ShowTasks(tasks, DateTime.Today, DateTime.Today);
        }

        static void ListAllTasks()
        {
            var tasks = GetTasks();
            var minDate = tasks.Min(t => t.Date);
            var maxDate = tasks.Max(t => t.Date);
            ShowTasks(tasks, minDate, maxDate);
        }

        static void ShowTasks(ToDoTask[] tasks, DateTime from, DateTime to)
        {
            if (tasks.Length == 0)
            {
                Console.WriteLine("There are no tasks");
                return;
            }
            if (to.Date < from.Date)
            {
                Console.WriteLine("DateTo can not be less than DateFrom");
                return;
            }
            for (var date = from.Date; date < to.Date.AddDays(1); date += TimeSpan.FromDays(1))
            {
                var dateTasks = tasks.Where(t => t.Date >= date && t.Date < date.AddDays(1)).ToArray();
                if (dateTasks.Any())
                {
                    Console.WriteLine($"{date:dd/MM/yy}");
                    foreach (var task in dateTasks)
                    {
                        Console.WriteLine($"\t{task.Name}");
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += HandleGlobalException;

            switch (args[0])
            {
                case "add":
                    if (String.IsNullOrEmpty(args[1]))
                    {
                        Console.WriteLine("Name can not be empty");
                        return;
                    };
                    if (!DateTime.TryParseExact(args[2], "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out var date))
                    {
                        Console.WriteLine("Incorrect date");
                        return;
                    }
                    var name = args[1];
                    AddTask(name, date);
                    break;
                case "today":
                    ListTodayTasks();
                    break;
                case "all":
                    ListAllTasks();
                    break;
            }
        }

        private static void HandleGlobalException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine($"Global exception caught: {e}");
        }
    }
}
