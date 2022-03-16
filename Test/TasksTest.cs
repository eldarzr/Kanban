using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
/*    class TasksTest
    {


        public static void BoardsCreations(Service service)
        {
            String[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            String[] names = { "BoardA", "BoardB", "BoardC", "BoardD", "BoardE" };

            for (int i = 0; i < emails.Length; i++)
            {
                for (int j = 0; j < names.Length; j++)
                {
                    BoardTest.AddBoard(service, emails[i], names[j]);
                    BoardTest.AddTask(service, emails[i], names[j]);
                    service.AdvanceTask(emails[i], names[j], 0, i + j);
                }
            }

        }
        public static void GetColumn(Service service)
        {
            String[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            String[] names = { "BoardA", "BoardB", "BoardC", "BoardD", "BoardE" };

            foreach (string email in emails)
            {
                foreach (string name in names)
                {
                    var respons = service.GetColumn(email, name, 1);
                    if (!respons.ErrorOccured)
                        foreach (IntroSE.Kanban.Backend.ServiceLayer.Task task in respons.Value)
                        {
                            Console.WriteLine(
                                task.Id + " " + task.Title + " " + task.Description + " " + task.CreationTime
                                + " " + task.DueDate);
                        }
                }
            }

        }
        public static void InProgress(Service service)
        {
            String[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            String[] names = { "BoardA", "BoardB", "BoardC", "BoardD", "BoardE" };

            foreach (string email in emails)
            {
                var respons = service.InProgressTasks(email);
                if (!respons.ErrorOccured)
                    foreach (IntroSE.Kanban.Backend.ServiceLayer.Task task in respons.Value)
                    {
                        Console.WriteLine(
                            task.Id + " " + task.Title + " " + task.Description + " " + task.CreationTime
                            + " " + task.DueDate);
                    }
            }

        }

        public static void AddTask(Service service, string email, string board)
        {
            String[] titles = { "Task A", "Task B", "Task C" };
            DateTime[] dateTimes = { new DateTime(2042, 10, 10), new DateTime(2032, 9, 9), new DateTime(2005, 12, 10) };
            int i = 0;
            foreach (String title in titles)
            {
                var response = service.AddTask(email, email, board, title, "bla bla bla", dateTimes[i]).Value;
            }
        }

    }*/


}
