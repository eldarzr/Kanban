using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class ServiceBoard
    {
        BoardController boardController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ServiceBoard(BoardController boardController)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            this.boardController = boardController;

        }

        internal void LoadData()
        {
            boardController.ImportBoardsFromDB();
        }


        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumn(String userEmail, string email, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                boardController.LimitColumn(userEmail, email, boardName, columnOrdinal, limit);
                log.Info($"user {email} limited {boardName} column number {columnOrdinal} by {limit}");
                return new Response();
            }

            catch (Exception e)
            {
                log.Debug($"an attempt by user {email}  to limite {boardName} , column number {columnOrdinal} failed");
                return new Response(e.Message);
            }
        }

        internal void DeleteData()
        {
            boardController.DeleteData();
        }

        /// <summary>
        /// Get the limit of a specific column
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The limit of the column.</returns>
        public Response<int> GetColumnLimit(string userEmail, string email, string boardName, int columnOrdinal)
        {
            try
            {
                int columnLimit = boardController.GetColumnLimit(userEmail, email, boardName, columnOrdinal);
                log.Info($"an attempt by user {email} to get {boardName} , column number {columnOrdinal} limit size succeeded");
                return Response<int>.FromValue(columnLimit);
            }

            catch (Exception e)
            {
                log.Error($"an attempt to get a Column Limit size has been failed");
                return Response<int>.FromError(e.Message);
            }
        }
        /// <summary>
        /// Get the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The name of the column.</returns>
        public Response<string> GetColumnName(string userEmail, string email, string boardName, int columnOrdinal)
        {
            try
            {
                String columnName = boardController.GetColumnName(userEmail, email, boardName, columnOrdinal);
                log.Debug($"an attempt by user {email} in board {boardName} to get a columnName has been succeeded");
                return Response<string>.FromValue(columnName);
            }

            catch (Exception e)
            {
                log.Debug($"an attempt by user {email} in board {boardName} to get a columnName has been failed");
                return Response<string>.FromError(e.Message);
            }
        }
        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. </param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>
        public Response<Task> AddTask(string userEmail, string emailCreator, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                Backend.BusinessLayer.Task task = boardController.AddTask(userEmail, emailCreator, boardName, title, description, dueDate);
                Task t1 = new Task(task.Id, task.CreationTime, task.Title, task.Description, task.DueDate, task.Assignee);
                log.Debug($"an attempt by user {emailCreator} in board {boardName} to get add a new task is succeeded");
                return Response<Task>.FromValue(t1);
            }

            catch (Exception e)
            {
                log.Debug($"an attempt by user {emailCreator} in board {boardName} to get add a new task is failed");
                return Response<Task>.FromError(e.Message);
            }
        }

        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(string userEmail, string email, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                boardController.AdvanceTask(userEmail, email, boardName, columnOrdinal, taskId);
                log.Debug($"an attempt by user {email} in board {boardName} to advance a task is succeeded");
                return new Response();
            }

            catch (Exception e)
            {
                log.Debug($"an attempt by user {email} in board {boardName} to advance a task is failed");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(string userEmail, string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {

                boardController.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate, userEmail);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(String userEmail, string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {

                boardController.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title, userEmail);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(string userEmail, string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {

                boardController.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description, userEmail);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="email">Email of the user.</param>
        /// <returns>A response object with a value set to the list of tasks, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> InProgressTasks(string email)
        {

            try
            {
                IList<BusinessLayer.Task> bussList = boardController.InProgressTasks(email);
                IList<Task> list = new List<Task>();
                foreach (BusinessLayer.Task t in bussList)
                {
                    Task task = new Task(t.Id, t.CreationTime, t.Title, t.Description, t.DueDate, t.Assignee);
                    list.Add(task);
                }
                return Response<IList<Task>>.FromValue(list);
            }
            catch (Exception e)
            {
                return Response<IList<Task>>.FromError(e.Message);
            }

        }
        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error messa
        public Response<IList<Task>> GetColumn(string userEmail, string email, string boardName, int columnOrdinal)
        {
            try
            {
                IList<BusinessLayer.Task> bussList = boardController.GetColumn(userEmail, email, boardName, columnOrdinal);
                IList<Task> list = new List<Task>();
                foreach (BusinessLayer.Task t in bussList)
                {
                    Task task = new Task(t.Id, t.CreationTime, t.Title, t.Description, t.DueDate, t.Assignee);
                    list.Add(task);
                }

                return Response<IList<Task>>.FromValue(list);
            }

            catch (Exception e)
            {
                return Response<IList<Task>>.FromError(e.Message);
            }
        }

        public Response AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            try
            {

                boardController.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, emailAssignee);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<IList<string>> getBoardNames(string userEmail)
        {
            try
            {

                IList<String> boardNames = boardController.GetBoardNames(userEmail);
                return Response<IList<String>>.FromValue(boardNames);
            }
            catch (Exception e)
            {
                return Response<IList<String>>.FromError(e.Message);
            }
        }

        public Response<IList<(String, String)>> getBoardNamesCreators(string userEmail)
        {
            try
            {

                IList<(String, String)> boardNames = boardController.GetBoardNamesCreators(userEmail);
                return Response<IList<(String, String)>>.FromValue(boardNames);
            }
            catch (Exception e)
            {
                return Response<IList<(String, String)>>.FromError(e.Message);
            }
        }

        /// <summary>
        /// Renames a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <param name="newColumnName">The new column name</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            try
            {
                boardController.RenameColumn(userEmail, creatorEmail, boardName, columnOrdinal, newColumnName);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Moves a column shiftSize times to the right. If shiftSize is negative, the column moves to the left
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <param name="shiftSize">The number of times to move the column, relativly to its current location. Negative values are allowed</param>  
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {

            try
            {
                boardController.MoveColumn(userEmail, creatorEmail, boardName, columnOrdinal, shiftSize);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Adds a new column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The location of the new column. Location for old columns with index>=columnOrdinal is increased by 1 (moved right). The first column is identified by 0, the location increases by 1 for each column.</param>
        /// <param name="columnName">The name for the new columns</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            try
            {
                boardController.AddColumn(userEmail, creatorEmail, boardName, columnOrdinal, columnName);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Removes a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                boardController.RemoveColumn(userEmail, creatorEmail, boardName, columnOrdinal);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response<int> getNumOfColumns(string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                int amountOfColumns = boardController.getNumOfColumns(userEmail, creatorEmail, boardName);
                return Response<int>.FromValue(amountOfColumns);
            }
            catch (Exception e)
            {
                return Response<int>.FromError(e.Message);
            }
        }

    }
}


