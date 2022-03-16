using NUnit.Framework;
using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using Moq;
using IntroSE.Kanban.Backend;

namespace TestProject
{
    public class Tests
    {
        Board b;
        
        [SetUp]
        public void init()
        {
            BoardBuilder boardBuilder = new BoardBuilder();
            BoardDirector boardDirector = new BoardDirector();
            boardDirector.Builder = boardBuilder;
            boardDirector.BuildNewBoard(0, "Name1", "shanihd@gmail.com", 0);
            b = boardBuilder.GetBoard();
           
            
        }
    

        [Test]
        public void AddColumn_success()
        {
            
            try
            {
                b.AddColumn("shanihd@gmail.com", 0, "InProgress", 1);
                Assert.AreEqual(true, true);
            }
            catch (Exception e)
            {
                Assert.AreEqual(true, false);
            }
        }
        [Test]
        public void AddColumn_SizeNegative_fail()
        {
            int columnOrdinal = -1;
            try
            {
                b.AddColumn("shanihd@gmail.com", columnOrdinal, "InProgress", 1);
                Assert.AreEqual(true, false);//fail
            }
            catch (Exception e)
            {
                Assert.AreEqual(false, false);//failed
            }
        }


            [Test]
        public void AddColumn_ValidateMember_Fail()
        {
            try
            {
                b.AddColumn("shanihd99@gmail.com", 1, "InProgress", 1);
                Assert.AreEqual(true, false);
            }
            catch
            {
                Assert.AreEqual(false, false);
            }

        }
        [Test]
        public void AddColumn_exceedSize_Fail()
        {
            int columnOrdinal = 1000;        
            try
            {
                b.AddColumn("shanihd@gmail.com", columnOrdinal, "InProgress", 1);
                Assert.AreEqual(true, false);
            }
            catch
            {
                Assert.AreEqual(false, false);
            }
        }
        [Test]
        public void JoinBoard_userExistes_fail()
        {
            try
            {
                b.JoinBoard("shanihd@gmail.com");
                Assert.AreEqual(true, false);
            }
            catch
            {
                Assert.AreEqual(false, false);
            }
        }
        [Test]
        public void JoinBoard_success()
        {
            try
            {
                b.JoinBoard("shanihd99@gmail.com");
                Assert.AreEqual(true, true);
            }
            catch
            {
                Assert.AreEqual(true, false);
            }
        }

        [Test]
        public void MoveColumn_valiEmail_fail()
        {
            try
            {
                b.MoveColumn("shanihd99@gmail.com",0,1);
                Assert.AreEqual(true, false);
            }
            catch
            {
                Assert.AreEqual(false, false);
            }
        }
        [Test]
        public void MoveColumn_Success()
        {
            try
            {
                b.MoveColumn("shanihd@gmail.com", 0, 1);
                Assert.AreEqual(true, true);
            }
            catch
            {
                Assert.AreEqual(true, false);
            }
        }
        [Test]
        public void MoveColumn_columnWithTask_fail()
        {
            try
            {
                b.columns[0].AddTask(8, new DateTime(2022, 12, 12), "title", "do the task", "shanihd@gmail.com");
                b.MoveColumn("shanihd@gmail.com", 0, 1);
                Assert.AreEqual(true, false);
            }
            catch
            {
                Assert.AreEqual(false, false);
            }
        }

       

        [TearDown]
        public void end()
        {
            b.DeleteFromDB();
        }
      
    }
}