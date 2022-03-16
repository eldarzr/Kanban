using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
  

        static void Main(string[] args)
        {
            Service service = new Service();

            string eldar = "eldar@gmail.com";
            string naor = "naor@gmail.com";
            string shani = "shani@gmail.com";

            service.LoadData();
            service.Register(shani, "E1d23");
            service.Login(shani, "E1d23");
            service.AddBoard(shani, "BoardA");
            service.AddBoard(shani, "BoardB");
            service.LimitColumn(shani, shani, "BoardA", 0, 30);
            service.LimitColumn(shani, shani, "BoardA", 1, 40);
            service.LimitColumn(shani, shani, "BoardA", 2, 50);
            service.RenameColumn(shani, shani, "BoardA", 0, "column0");
            service.RenameColumn(shani, shani, "BoardA", 2, "column2");
            service.RenameColumn(shani, shani, "BoardA", 1, "column1");
            service.MoveColumn(shani, shani, "BoardA", 0, 2);
            service.MoveColumn(shani, shani, "BoardA", 0, 2);
            service.MoveColumn(shani, shani, "BoardA", 2, -2);

            service.AddTask(shani, shani, "BoardA", "a", "b", new DateTime(2022, 10, 10));
            service.AdvanceTask(shani, shani, "BoardA", 0, 1);
            service.AddColumn(shani, shani, "BoardA", 2, "Cool Column");
            service.AddColumn(shani, shani, "BoardA", 2, "Ha-Column Shel Eldar");
            service.RemoveColumn(shani, shani, "BoardA", 1);
            service.RemoveColumn(shani, shani, "BoardA", 2);


            //service.DeleteData();


            /*            RegLogForTest(service, eldar, "Ee123");
                        RegLogForTest(service, naor, "Ee123");

                        BoardsCreations(service);

                        service.AddBoard(eldar, "aaa");
                        service.AddBoard(naor, "bbb");
                        service.AddBoard(naor, "ccc");
                        service.JoinBoard(naor, eldar, "aaa");
            service.AddTask(eldar, eldar, "aaa", "a", "b", new DateTime(2022, 10, 10));
            service.AddTask(naor, eldar, "aaa", "c", "d", new DateTime(2022, 10, 10));
            service.AddTask(naor, naor, "bbb", "e", "f", new DateTime(2022, 10, 10));
            service.UpdateTaskDueDate(eldar, eldar, "aaa", 0, 1, new DateTime(2056, 10, 10));
            service.UpdateTaskDescription(eldar, eldar, "aaa", 0, 1, "hello world");
            service.UpdateTaskTitle(eldar, eldar, "aaa", 0, 1, "title");
            service.AssignTask(eldar, eldar, "aaa", 0, 1, naor);
            service.AdvanceTask(naor, eldar, "aaa", 0, 1);

            service.RemoveBoard(naor, eldar, "aaa");

            service.DeleteData();*/



            /*             RegLogForTest(service, "heldar@gmail.com", "A 1 x ");
                        RegLogForTest(service, "keldar@gmail.com", ".@1Ab");
                        RegLogForTest(service, "teldar@gmail.com", "aBa2");
                        RegLogForTest(service, "meldar@gmail.com", "aaaa33A");*/
            /*
                        BoardTest.AddBoard(service, email, "AAA");


                        RegLogForTest(service, "naor@gmail.com", "Ee123");
                        JoinBoards(service);
                        RemoveBoards(service);*/




            /*
                        BoardTest.AddBoard(service, email, "123");
                        BoardTest.RemoveBoard(service, email);
                        BoardTest.AddTask(service, email, "123");*/

            //TasksTest.BoardsCreations(service);
            //  TasksTest.GetColumn(service);
            //TasksTest.InProgress(service);



        }
        public static void BoardsCreations(Service service)
        {
            String[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            String[] names = { "BoardA", "BoardA", "BoardB", "BoardC", "BoardD", "BoardE" };

            for (int i = 0; i < emails.Length; i++)
            {
                for (int j = 0; j < names.Length; j++)
                {
                    BoardTest.AddBoard(service, emails[i], names[j]);
                    //                    BoardTest.AddTask(service, emails[i], names[j]);
                }
            }
            /*
            /*
               BoardTest.UpdateTaskTitle(service, "eldar@gmail.com", "BoardB",0,1,"NEW TITLE");
               BoardTest.UpdateTaskTitle(service, "eldar@gmail.com", "BoardB", 0, 1, "NEW TITLE!!!!!!!!!!!");
               BoardTest.UpdateTaskTitle(service, "eldar@gmail.com", "BoardB", 0, 6, "NEW TITLE");
              */
            /*
                        BoardTest.AddTask(service, "eldar@gmail.com", "BoardC");
                        BoardTest.LimitColumn(service, "eldar@gmail.com", "BoardC");
                        BoardTest.AddTask(service, "eldar@gmail.com", "BoardC");
                        BoardTest.GetColumnLimit(service,"eldar@gmail.com", "BoardC",0);*/


            //BoardTest.LimitColumn(service, "eldar@gmail.com", "AAAA");
            //BoardTest.LimitColumn(service, "eldar@gmail.com", "BoardB");

            /*  *****ADVANCETASK TESTS *****
            BoardTest.AdvanceTask(service, "eldar@gmail.com", "BoardA", 0, 0);
            BoardTest.AdvanceTask(service, "eldar@gmail.com", "BoardA", 0, 1);
            BoardTest.AdvanceTask(service, "eldar@gmail.com", "BoardA", 1, 1);
            BoardTest.AdvanceTask(service, "eldar@gmail.com", "BoardA", 2, 1);
            BoardTest.AdvanceTask(service, "eldar@gmail.com", "BoardA", 3, 1); */



        }
        public static void UpdatesCheck(Service service)
        {

            String[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            String[] names = { "BoardA", "BoardB", "BoardC", "BoardD", "BoardE" };

        }

        public static void userRegister(Service service)
        {
            String[] passwords = { "123", "1234", "E123", "Eldar", "El123", "El123", "EL122333333333333333333" };
            foreach (String pass in passwords)
            {
                Response response = service.Register(pass, pass);
                if (response.ErrorOccured)
                    Console.WriteLine(response.ErrorMessage + "on password: " + pass);
                else
                    Console.WriteLine("registered succesfuly");
            }
        }
        public static void userLogin(Service service)
        {
            String[] emails = { "Eldar", "El123", "El123", "EL122333333333333333333" };
            foreach (String email in emails)
            {
                Response<User> response = service.Login(email, email);
                if (response.ErrorOccured)
                    Console.WriteLine(response.ErrorMessage + "on user: " + email);
                else
                    Console.WriteLine($"logged in succesfuly: {response.Value.Email}");
            }
        }
        public static void userLogout(Service service)
        {
            String[] emails = { "Eldar", "El123", "El123", "EL122333333333333333333" };
            foreach (String email in emails)
            {
                Response response = service.Logout(email);
                if (response.ErrorOccured)
                    Console.WriteLine(response.ErrorMessage + "on user: " + email);
                else
                    Console.WriteLine($"logged out succesfuly: {email}");
            }
        }

        public static void RegLogForTest(Service service, string email, String pass)
        {
            Console.WriteLine(service.Register(email, pass).ErrorMessage);
            Console.WriteLine(service.Login(email, pass).ErrorMessage);
        }

        public static void JoinBoards(Service service)
        {
            String[] emails = { "eldar@gmail.com", "naor@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            String[] names = { "BoardA", "BoardB", "BoardC", "BoardD", "BoardE" };

            for (int i = 0; i < names.Length; i++)
                BoardTest.AddBoard(service, emails[0], names[i]);
            for (int i = 0; i < emails.Length; i++)
                for (int j = 0; j < names.Length; j++)
                {
                    BoardTest.JoinBoard(service, emails[i], emails[0], names[j]);
                }
        }


        public static void RemoveBoards(Service service)
        {
            string boardName = "BoardA";
            String[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };

            for (int i = 0; i < emails.Length; i++)
            {
                for (int j = 0; j < emails.Length; j++)
                {
                    BoardTest.RemoveBoard(service, emails[i], emails[j], boardName);
                }
            }
        }
    }
}
