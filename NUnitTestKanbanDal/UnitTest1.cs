using NUnit.Framework;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace NUnitTestKanbanDal
{
    public class Tests
    {
        Service service;        

        [SetUp]
        public void Setup()
        {
            service = new Service();
/*            string[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            string[] passwords = { "12vfGsdvfdvfd3", "E12vfdvfdvdfvfd34", "E1d23" };
            for (int i = 0; i < emails.Length; i++)
            {
                Response response = service.Login(emails[i], passwords[i]);
            }*/
        }

        [Test]
        public void Test1()
        {
            string[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            string[] passwords = { "12vfGsdvfdvfd3", "E12vfdvfdvdfvfd34", "E1d23" };
            for (int i = 0; i < emails.Length; i++)
            {
                Response response = service.Register(emails[i], passwords[i]);
                Assert.AreEqual(false, response.ErrorOccured);
            }

        }
        [Test]
        public void Test2()
        {
            service.LoadData();
            string[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            string[] passwords = { "12vfGsdvfdvfd3", "E12vfdvfdvdfvfd34", "E1d23" };
            for (int i = 0; i < emails.Length; i++)
            {
                Response response = service.Login(emails[i], passwords[i]);
                Assert.AreEqual(false, response.ErrorOccured);
            }

        }

        [Test]
        public void Test3()
        {
            service.LoadData();
            string[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            string[] passwords = { "12vfGsdvfdvfd3", "E12vfdvfdvdfvfd34", "E1d23" };
            string[] names = { "BoardA", "BoardB", "BoardC", "BoardD", "BoardE" };

            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
                for (int j = 0; j < names.Length; j++)
                {
                    Response response = service.AddBoard(emails[i], names[j]);
                    Assert.AreEqual(false, response.ErrorOccured);
                }
            }

        }
        [Test]
        public void Test4()
        {
            service.LoadData();
            string[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            string[] passwords = { "12vfGsdvfdvfd3", "E12vfdvfdvdfvfd34", "E1d23" };
            string[] names = { "BoardA", "BoardB", "BoardC", "BoardD", "BoardE" };

            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
                for (int j = 0; j < names.Length; j++)
                {
                    Response response = service.AddBoard(emails[i], names[j]);
                    Assert.AreEqual(true, response.ErrorOccured);
                }
            }

        }
        [Test]
        public void Test5()
        {
            service.LoadData();
            string[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            string[] passwords = { "12vfGsdvfdvfd3", "E12vfdvfdvdfvfd34", "E1d23" };
            string[] names = { "BoardA", "BoardB", "BoardC", "BoardD", "BoardE" };

            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
                for (int j = 0; j < names.Length; j++)
                {
                    Response response = service.JoinBoard(emails[i], emails[(i+1)%3], names[j]);
                    Assert.AreEqual(false, response.ErrorOccured);
                }
            }

        }
        [Test]
        public void Test6()
        {
            service.LoadData();
            string[] emails = { "eldar@gmail.com", "naor@gmail.com", "shani@gmail.com" };
            string[] passwords = { "12vfGsdvfdvfd3", "E12vfdvfdvdfvfd34", "E1d23" };
            string[] names = { "BoardA", "BoardB", "BoardC", "BoardD", "BoardE" };
            service.LoadData();
            for (int i = 0; i < emails.Length; i++)
            {
                service.Login(emails[i], passwords[i]);
                for (int j = 0; j < names.Length; j++)
                {
                    Response response = service.JoinBoard(emails[i], emails[(i + 1) % 3], names[j]);
                    Assert.AreEqual(true, response.ErrorOccured);
                }
            }

        }

    }
}