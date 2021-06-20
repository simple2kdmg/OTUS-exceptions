using System;
using System.Diagnostics;
using System.Globalization;

namespace ToDoText
{
    public class ToDoTask
    {
        public static readonly DateTime MinDate = new DateTime(2000, 1, 1);
        public static readonly DateTime MaxDate = new DateTime(2100, 1, 1);

        public ToDoTask(string name, DateTime date)
        {
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name can not be empty or whitespace.");
            if (date < MinDate || date > MaxDate)
                throw new ArgumentException("Date should be in range: 2000-01-01 to 2100-01-01");
            Name = name;
            Date = date;
        }

        public static ToDoTask Parse(string input)
        {
            if (String.IsNullOrWhiteSpace(input)) throw new ArgumentNullException();
            var split = input.Split('\t');
            if (!DateTime.TryParseExact(split[0], "dd/MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var date)) throw new ArgumentException("Invalid date.");
            var name = split[1];
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name can not be empty or whitespace.");
            return new ToDoTask(name, date);
        }

        public static bool TryParse(string input, out ToDoTask toDoTask)
        {
            try
            {
                toDoTask = Parse(input);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                toDoTask = null;
                return false;
            }
        }

        public DateTime Date { get; }
        public string Name { get; }
    }
}
