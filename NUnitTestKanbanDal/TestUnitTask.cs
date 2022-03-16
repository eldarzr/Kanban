using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;
using System;

namespace NUnitTestKanbanDal
{
    class TestUnitTask
    {
        Service service;
        string[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
        string[] passwords = { "12vfGsdvfdvfd3", "E12vfdvfdvdfvfd34", "E1d23" };
        string[] boardNames = { "BoardA", "BoardB", "BoardC", "BoardD", "BoardE" };
        string[] taskNames = { "TaskA", "TaskB", "TaskC", "TaskD", "TaskE" };
        DateTime[] dateTimes = { new DateTime(2022, 10, 12), new DateTime(2025, 4, 23), new DateTime(2024, 9, 8) };

        [SetUp]
        public void Setup()
        {
            service = new Service();

        }

        [Test]
        public void Test1()
        {
            service.LoadData();

            DateTime[] dateTimes = { new DateTime(2022, 10, 12), new DateTime(2025, 4, 23), new DateTime(2024, 9, 8) };
            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
            }
            Response<Task> responseA = service.AddTask("eldar@gmail.com", "eldar@gmail.com", "BoardA", "The Title !", " The Description ! ", dateTimes[0]);
            Response<Task> responseB = service.AddTask("naor@gmail.com", "eldar@gmail.com", "BoardA", "The Amazing Eldar !", " Boooomba ! ", dateTimes[1]);
            Response<Task> responseC = service.AddTask("shani@gmail.com", "eldar@gmail.com", "BoardA", "Miles Morales!", "Black Beldar! ", dateTimes[2]);


            Assert.AreEqual(false, responseA.ErrorOccured);
            Assert.AreEqual(true, responseB.ErrorOccured);
            Assert.AreEqual(false, responseC.ErrorOccured);

            /* DateTime[] dateTimes = { new DateTime(2022, 10, 12), new DateTime(2025, 4, 23), new DateTime(2024, 9, 8) };
             for (int i = 0; i < emails.Length; i++)
             {
                 service.Login(emails[i], passwords[i]);
                 for(int j=0; j<boardNames.Length; j++)
                 {
                     for (int k = 0; k < taskNames.Length; k++)
                     {
                         Response<Task> response = service.AddTask(emails[i], emails[i], boardNames[j], taskNames[k], taskNames[k], dateTimes[i]);
                         Assert.AreEqual(false, response.ErrorOccured);
                     }
                 }
             }*/

        }
        [Test]
        public void Test10()
        {
            service.LoadData();
            service.Login(emails[0], passwords[0]);
            DateTime past = new DateTime(1998, 10, 12);
            //     Response<Task> responseA = service.AddTask("eldar@gmail.com", "eldar@gmail.com", "BoardA", "The Title !", " The Description ! ", dateTimes[0]);
            Response response1 = service.UpdateTaskDescription(emails[0], "eldar@gmail.com", "BoardA", 0, 1, "New Descript");
            Response response2 = service.UpdateTaskDescription(emails[0], "eldar@gmail.com", "BoardA", 0, 1, "");
            Response response3 = service.UpdateTaskDescription(emails[0], "eldar@gmail.com", "BoardA", 0, 1, null);
            Response response4 = service.UpdateTaskTitle(emails[0], "eldar@gmail.com", "BoardA", 0, 1, "New Title");
            Response response5 = service.UpdateTaskTitle(emails[0], "eldar@gmail.com", "BoardA", 0, 1, "");
            Response response6 = service.UpdateTaskTitle(emails[0], "eldar@gmail.com", "BoardA", 0, 1, null);
            Response response7 = service.UpdateTaskDueDate(emails[0], "eldar@gmail.com", "BoardA", 0, 1, dateTimes[2]);
            Response response8 = service.UpdateTaskDueDate(emails[0], "eldar@gmail.com", "BoardA", 0, 1, past);

            Assert.AreEqual(false, response1.ErrorOccured);
            Assert.AreEqual(false, response2.ErrorOccured); // FALSE
            Assert.AreEqual(true, response3.ErrorOccured);
            Assert.AreEqual(false, response4.ErrorOccured);
            Assert.AreEqual(true, response5.ErrorOccured);
            Assert.AreEqual(true, response6.ErrorOccured);
            Assert.AreEqual(false, response7.ErrorOccured);
            Assert.AreEqual(true, response8.ErrorOccured);

        }
        [Test]
        public void Test11()
        {
            service.LoadData();
            service.Login(emails[2], passwords[2]);
            DateTime past = new DateTime(1998, 10, 12);
            //     Response<Task> responseA = service.AddTask("eldar@gmail.com", "eldar@gmail.com", "BoardA", "The Title !", " The Description ! ", dateTimes[0]);
            Response response1 = service.UpdateTaskDescription(emails[2], "eldar@gmail.com", "BoardA", 0, 1, "New Descript");
            Response response2 = service.UpdateTaskDescription(emails[2], "eldar@gmail.com", "BoardA", 0, 1, "");
            Response response3 = service.UpdateTaskDescription(emails[2], "eldar@gmail.com", "BoardA", 0, 1, null);
            Response response4 = service.UpdateTaskTitle(emails[2], "eldar@gmail.com", "BoardA", 0, 1, "New Title");
            Response response5 = service.UpdateTaskTitle(emails[2], "eldar@gmail.com", "BoardA", 0, 1, "");
            Response response6 = service.UpdateTaskTitle(emails[2], "eldar@gmail.com", "BoardA", 0, 1, null);
            Response response7 = service.UpdateTaskDueDate(emails[2], "eldar@gmail.com", "BoardA", 0, 1, dateTimes[2]);
            Response response8 = service.UpdateTaskDueDate(emails[2], "eldar@gmail.com", "BoardA", 0, 1, past);

            Assert.AreEqual(true, response1.ErrorOccured);
            Assert.AreEqual(true, response2.ErrorOccured);
            Assert.AreEqual(true, response3.ErrorOccured);
            Assert.AreEqual(true, response4.ErrorOccured);
            Assert.AreEqual(true, response5.ErrorOccured);
            Assert.AreEqual(true, response6.ErrorOccured);
            Assert.AreEqual(true, response7.ErrorOccured);
            Assert.AreEqual(true, response8.ErrorOccured);

        }

        [Test]
        public void Test12()
        {
            service.LoadData();
            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
            }
            DateTime past = new DateTime(1998, 10, 12);

            Response response1 = service.AssignTask(emails[0], emails[0], "BoardA", 0, 1, emails[1]);
            Response response2 = service.AssignTask(emails[1], emails[0], "BoardA", 0, 1, emails[1]);
            Response response3 = service.AssignTask(emails[0], emails[0], "BoardA", 0, 1, emails[2]);
            Response response4 = service.AssignTask(emails[2], emails[0], "BoardA", 0, 1, emails[2]);
            Response response5 = service.AdvanceTask(emails[0], emails[0], "BoardA", 0, 1);
            Response response6 = service.AdvanceTask(emails[1], emails[0], "BoardA", 0, 1);
            Response response7 = service.AdvanceTask(emails[2], emails[0], "BoardA", 0, 1);
            Response response7B = service.AdvanceTask(emails[2], emails[0], "BoardA", 4, 1);
            Response response7C = service.AdvanceTask(emails[2], emails[0], "BoardA", -1, 1);
            //       Response response8 = service.AdvanceTask(emails[2], emails[0], "BoardA", 1, 1);
            Response response9 = service.AdvanceTask(emails[2], emails[0], "BoardA", 2, 1);

            //   Response response8 = service.AdvanceTask(emails[1], emails[0], "BoardA", 0, 1);



            Assert.AreEqual(true, response1.ErrorOccured);
            Assert.AreEqual(true, response2.ErrorOccured);
            Assert.AreEqual(true, response3.ErrorOccured);
            Assert.AreEqual(true, response4.ErrorOccured);
            Assert.AreEqual(true, response5.ErrorOccured);
            Assert.AreEqual(true, response6.ErrorOccured);
            Assert.AreEqual(true, response7.ErrorOccured);
            Assert.AreEqual(true, response7B.ErrorOccured);
            Assert.AreEqual(true, response7C.ErrorOccured);
            //    Assert.AreEqual(false, response8.ErrorOccured);
            Assert.AreEqual(true, response9.ErrorOccured);


        }


        [Test]

        public void Test13()
        {
            service.LoadData();
            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
            }
            DateTime past = new DateTime(1998, 10, 12);

            Response response1 = service.AssignTask(emails[0], emails[0], "BoardA", 0, 1, emails[1]);
            Response response2 = service.AssignTask(emails[1], emails[0], "BoardA", 0, 1, emails[1]);
            Response response3 = service.AssignTask(emails[0], emails[0], "BoardA", 0, 1, emails[2]);
            Response response4 = service.AssignTask(emails[2], emails[0], "BoardA", 0, 1, emails[2]);

            Assert.AreEqual(true, response1.ErrorOccured);
            Assert.AreEqual(true, response2.ErrorOccured);
            Assert.AreEqual(false, response3.ErrorOccured);
            Assert.AreEqual(false, response4.ErrorOccured);


        }

        [Test]
        public void Test14(){
            service.LoadData();
            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
            }
            Response<Task> responseC1 = service.AddTask("shani@gmail.com", "naor@gmail.com", "BoardA", "Miles Morales!", "Tea! ", dateTimes[2]);
            Response<Task> responseC2 = service.AddTask("shani@gmail.com", "shani@gmail.com", "BoardA", "Files Forales!", "Ice! ", dateTimes[2]);
        Response<Task> responseC3 = service.AddTask("shani@gmail.com", "eldar@gmail.com", "BoardA", "Kiles Korales!", "JuaE! ", dateTimes[2]);
        Response<Task> responseC4 = service.AddTask("shani@gmail.com", "shani@gmail.com", "BoardA", "Tiles Torales!", "TopC! ", dateTimes[1]);

        }



        [Test]
        public void Test15() {
            service.LoadData();
            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
            }
            
            Response<Task> responseC5 = service.AddTask("naor@gmail.com", "shani@gmail.com", "BoardA", "Tyze Shutakes!", "Bloop ", dateTimes[1]);
            Response response0 = service.AssignTask("shani@gmail.com", "shani@gmail.com","BoardA",0,6,"shani@gmail.com");

            Response response7B = service.AdvanceTask(emails[2], emails[1], "BoardA",0, 1);
            Response response7C = service.AdvanceTask(emails[2], emails[2], "BoardA", 4, 1);
            Response response7D = service.AdvanceTask(emails[2], emails[0], "BoardA", 4, 1);
        }
            [Test]
        public void Test2()
        {
            service.LoadData();
            DateTime[] dateTimes = { new DateTime(2022, 10, 12), new DateTime(2025, 4, 23), new DateTime(2024, 9, 8) };
            int id = 1;
            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
                for (int j = 0; j < boardNames.Length; j++)
                {
                    for (int k = 0; k < taskNames.Length; k++)
                    {
                        Response response = service.UpdateTaskTitle(emails[i], emails[i], boardNames[j], 0, id++, "title" +id);
                        Assert.AreEqual(false, response.ErrorOccured);
                    }
                }
            }
        }

        [Test]
        public void Test3()
        {
            service.LoadData();
            DateTime[] dateTimes = { new DateTime(2022, 10, 12), new DateTime(2025, 4, 23), new DateTime(2024, 9, 8) };
            int id = 1;
            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
                for (int j = 0; j < boardNames.Length; j++)
                {
                    for (int k = 0; k < taskNames.Length; k++)
                    {
                        Response response = service.UpdateTaskDescription(emails[i], emails[i], boardNames[j], 0, id++, "desc" + id);
                        Assert.AreEqual(false, response.ErrorOccured);
                    }
                }
            }

        }

        [Test]
        public void Test4()
        {
            service.LoadData();
            DateTime[] dateTimes = { new DateTime(2022, 10, 12), new DateTime(2025, 4, 23), new DateTime(2024, 9, 8) };
            int id = 1;
            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
                for (int j = 0; j < boardNames.Length; j++)
                {
                    for (int k = 0; k < taskNames.Length; k++)
                    {
                        Response response = service.UpdateTaskDueDate(emails[i], emails[i], boardNames[j], 0, id++, new DateTime(2222, 2, 22));
                        Assert.AreEqual(false, response.ErrorOccured);
                    }
                }
            }

        }
        [Test]
        public void Test5()
        {
            service.LoadData();
            DateTime[] dateTimes = { new DateTime(2022, 10, 12), new DateTime(2025, 4, 23), new DateTime(2024, 9, 8) };
            int id = 1;
            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
                for (int j = 0; j < boardNames.Length; j++)
                {
                    for (int k = 0; k < taskNames.Length; k++)
                    {
                        Response response = service.AdvanceTask(emails[i], emails[i], boardNames[j], 0, id++);
                        Assert.AreEqual(false, response.ErrorOccured);
                    }
                }
            }

        }

    }
}