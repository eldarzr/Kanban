using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class BoardTest
    {
        Service service;

        public BoardTest(Service service)
        {
            this.service = service;
        }

        public static void AddBoard(Service service, string email, String name)
        {

            Response response = service.AddBoard(email, name);
            if (response.ErrorOccured)
                Console.WriteLine(response.ErrorMessage + "on board: " + name);
            else
                Console.WriteLine("add board succesfuly");
        }
        public static void JoinBoard(Service service, string userEmail, String creatorEmail, String boardName)
        {

            Response response = service.JoinBoard(userEmail, creatorEmail, boardName);
            if (response.ErrorOccured)
                Console.WriteLine(response.ErrorMessage + "on board: " + boardName);
            else
                Console.WriteLine("add board succesfuly");
        }
                public static void RemoveBoard(Service service, string userEmail, string creatorEmail, string boardName)
                {
                    Response response = service.RemoveBoard(userEmail, creatorEmail, boardName);
                    if (response.ErrorOccured)
                        Console.WriteLine($"{response.ErrorMessage} on board: {boardName} on user: {userEmail}");
                    else
                        Console.WriteLine("remove board succesfuly");
            }
        /*

                public static void LimitColumn(Service service,String email,String boardName)
                {

                    int[] lsize = {3,4};
                    //tring [] boardNames = {"BoardB","AAAA
                    for (int i = 0; i < lsize.Length; i++)
                    {
                        Response response = service.LimitColumn(email, boardName, 0 , lsize[i]);
                        if (response.ErrorOccured)
                            Console.WriteLine(response.ErrorMessage + "on limit size: " + lsize[i]);
                        else
                            Console.WriteLine("Limit the column successufly");
                    }
                }

                public static void AdvanceTask(Service service, string email, string board, int columnOrdinal, int taskId) {

                    Response response = service.AdvanceTask(email, board, columnOrdinal, taskId);
                    if (response.ErrorOccured)
                        Console.WriteLine(response.ErrorMessage + "FAILED ADVANCE THE TASK ");
                    else
                        Console.WriteLine("AdvanceTask the column successfully");
                }

                public static void GetColumnName(Service service, string email, string board, int columnOrdinal)
                {

                    Response response = service.GetColumnName(email, board, columnOrdinal);
                    if (response.ErrorOccured)
                        Console.WriteLine(response.ErrorMessage + "FAILED RECEIVED COLUMN NAME");
                    else
                        Console.WriteLine("AdvanceTask the column successfully");
                }
                    public static void GetColumnLimit(Service service, string email, string board, int columnOrdinal)
                    {

                        Response response = service.GetColumnLimit(email, board, columnOrdinal);
                        if (response.ErrorOccured)
                            Console.WriteLine(response.ErrorMessage + "GET COLUMN LIMIT FAIL");
                        else
                            Console.WriteLine("AdvanceTask the column successfully");
                    }

                public static void AddTask(Service service, string email, string board)
                {
                    String[] titles = { "Task A", "Task B"};
                    DateTime[] dateTimes = { new DateTime(2042, 10, 10), new DateTime(2032, 9, 9)};
                    int i = 0;
                    foreach (String title in titles)
                    {
                        Response response = service.AddTask(email, board, title, "bla bla bla", dateTimes[i]);
                        i++;
                        if (response.ErrorOccured)
                            Console.WriteLine(response.ErrorMessage + "on board: " + title);
                        else
                            Console.WriteLine("add task succesfuly");
                    }
                }

                public static void UpdateTaskTitle(Service service, string email, string board, int column, int id, String title)
                {
                    Response response = service.UpdateTaskTitle(email, board, column, id, title);
                    if (response.ErrorOccured)
                        Console.WriteLine(response.ErrorMessage + "on board: " + board + "by email" + email);
                    else
                        Console.WriteLine("add task succesfuly");
                }


                public static void UpdateTaskDescription(Service service, string email, string board, int column, int id, String descript)
                {
                    Response response = service.UpdateTaskDescription(email, board, column, id, descript);
                    if (response.ErrorOccured)
                        Console.WriteLine(response.ErrorMessage + "on board: " + board + "by email" + email);
                    else
                        Console.WriteLine("add task succesfuly");
                }


                public static void UpdateTaskDueDate(Service service, string email, string board, int column, int id, DateTime time)
                {
                    Response response = service.UpdateTaskDueDate(email, board, column, id, time);
                    if (response.ErrorOccured)
                        Console.WriteLine(response.ErrorMessage + "on board: " + board + "by email" + email);
                    else
                        Console.WriteLine("add task succesfuly");
                }
        */





    }
}