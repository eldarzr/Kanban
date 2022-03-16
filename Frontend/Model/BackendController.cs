using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class BackendController
    {
        private Service service;

        public BackendController()
        {
            service = new Service();
            service.LoadData();
            /*            service.Login("eldar@gmail.com", "Eldar123");
                        service.AddTask("eldar@gmail.com", "naor@gmail.com", "BoardA", "Task1", "", new DateTime(2022, 2, 1));
                        service.AddTask("eldar@gmail.com", "naor@gmail.com", "BoardA", "Task2", "", new DateTime(2022, 2, 1));
                        service.AddTask("eldar@gmail.com", "naor@gmail.com", "BoardA", "Task3", "", new DateTime(2022, 2, 1));*/
        }
        ///<summary>This method registers a new user to the system.</summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
  
        internal void Register(string email, string password)
        {
            Response res = service.Register(email, password);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// /// <returns>A User</returns>
        internal UserModel Login(string email, string password)
        {
            Response<User> res = service.Login(email, password);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return new UserModel(this, res.Value.Email);
        }
        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        
        internal void Logout(string email)
        {
            Response res = service.Logout(email);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>
        internal void AddBoard(string email, string boardName)
        {
            Response res = service.AddBoard(email, boardName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user that create the board. </param>
        internal void AddColumn(string email, string emailCreator, string boardName, string addColumnName, int addColumnOrdinal)
        {
            Response res = service.AddColumn(email, emailCreator, boardName, addColumnOrdinal, addColumnName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        //  user.Email, Board.EmailCreator, Board.BoardName, AddColumnOrdinal

        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user that create the board. </param>
        internal void RemoveColumn(string email, string emailCreator, string boardName, int addColumnOrdinal)
        {
            Response res = service.RemoveColumn(email, emailCreator, boardName, addColumnOrdinal);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Join to an exist board.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user that create the board. </param>
        /// <param name="boardName">The name of the board to join</param>
        internal void JoinBoard(string email, string emailCreator, string boardName)
        {
            Response res = service.JoinBoard(email, emailCreator, boardName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="user"> the user. Must be logged in</param>
        /// <returns>returns list of all tasks that in progress by a specific user.
        internal IList<TaskModel> getAllInProgress(UserModel user)
        {
            IList<TaskModel> list = new List<TaskModel>();
            foreach (var task in service.InProgressTasks(user.Email).Value)
                list.Add(new TaskModel(this, task.CreationTime, task.Title, task.Description, task.DueDate, task.emailAssignee, task.Id, user));

            return list;
        }

        // <summary>
        /// remove a board.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board to remove</param>
        internal void RemoveBoard(string email, string emailCreator, string boardName)
        {
            Response res = service.RemoveBoard(email, emailCreator, boardName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Returns all the board of a specific user.
        /// </summary>
        /// <param name="user"> the user. Must be logged in</param>
        /// <returns>returns list of all boards that is assign to the user.
        internal IList<(String, String)> GetAllUserBoards(string email)
        {
            Response<IList<(String, String)>> res = service.GetBoardNamesCreators(email);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.Value;
        }
        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user that create the board. </param>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> return a list of all  the task in a specific column.
        internal IList<TaskModel> GetColumn(UserModel userEmail, string creatorEmail, string boardName, int columnOrdinal)
        { 

            IList<TaskModel> list = new List<TaskModel>();
            foreach (var task in service.GetColumn(userEmail.Email, creatorEmail, boardName, columnOrdinal).Value)
                list.Add(new TaskModel(this, task.CreationTime, task.Title, task.Description, task.DueDate, task.emailAssignee, task.Id,userEmail));
            return list;
        }

        /// <summary>
        /// Get the name of a specific column
        /// </summary>

        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="emailCreator">Email of the user that create the board. </param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="ordinallity">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The name of the column.</returns>
        internal String GetColumnName(string userEmail,string boardName,string emailCreator,int ordinallity)
        {
            Response<String> res = service.GetColumnName(userEmail, emailCreator, boardName, ordinallity);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.Value;
        }
        /// <summary>
        /// Get the number of columns in board.
        /// </summary>
        /// <param name="boardName">The name of the board</param>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="emailCreator">Email of the user that create the board. </param>
        /// /// <returns>The number ofcolumns.</returns>
        internal int GetNumberOfColumns(string userEmail, string boardName, string emailCreator)
        {
            Response<int> res = service.getNumberOfColumns(userEmail, emailCreator, boardName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            return res.Value;
        }
        /// <summary>
        /// move column .
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="emailCreator">Email of the user that create the board</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="ordinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="shiftSize">The number of times to move the column, relativly to its current location. Negative values are allowed</param>
        internal void MoveColumn(string email, string emailCreator, string boardName, int ordinal, int shiftSize)
        {
            Response res = service.MoveColumn(email, emailCreator, boardName, ordinal,shiftSize);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// assign a task for user .
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task id</param>
        /// <param name="emailAssignee">The email of the assignee of this task.</param>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="emailCreator">Email of the user that create the board. </param>
        internal void AssignTask(string userEmail, string emailCreator, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            Response res = service.AssignTask(userEmail, emailCreator, boardName, columnOrdinal, taskId, emailAssignee);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }

        }
        /// <summary>
        /// change the size of the column.
        /// </summary>
        /// <param name="newLimitation">the size of column</param>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user that create the board. </param>
        /// <param name="boardName">The name of the board to remove</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        internal void ChangeLimit(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int newLimitation)
        {
            Response res = service.LimitColumn(userEmail, creatorEmail, boardName, columnOrdinal, newLimitation);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }

        }
        // <summary>
        /// rename column.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board to remove</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="newColumnName">The new name of the column</param>

        internal void ChangeColumnName(string email, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            Response res = service.RenameColumn(email, creatorEmail, boardName, columnOrdinal, newColumnName);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user that create the board. </param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        internal void AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            Response res = service.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user that create the board. </param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        internal void UpdateTaskTitle(string userEmail, string creatorEmail,string boardName, int columnOrdinal, int taskId, string title)
        {
            Response res = service.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskId, title);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user that create the board. </param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        internal void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName,int columnOrdinal,int taskId,string description)
        {
            Response res = service.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskId, description);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }
        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user that create the board. </param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        internal void UpdateTaskDuedate(string userEmail, string creatorEmail, string boardName, int columnOrdinal,int taskId, DateTime duedate)
        {
            Response res = service.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskId, duedate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }


        /// <summary>
        /// update the assignee of the task.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="emailCreator">Email of the user that create the board. </param>
        /// <param name="assignee">the user that take the task-Assignee</param>
        /// <param name="taskId">The task ID</param>
        /// <param name="title">The new title of the task</param>
        internal void UpdateTaskAssigne(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string assigne)
        {
            Response res = service.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, assigne);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }


        /// Add a new task.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="creatorEmail"></param>
        /// <param name="boardName"></param>
        /// <param name="columnOrdinal"></param>
        /// <param name="taskId"></param>
        /// <param name="assigne"></param>
        /// 
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="emailCreator">Email of the user that create the board. </param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        internal TaskModel AddTask(UserModel user, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            Response<Task> res = service.AddTask(user.Email, creatorEmail, boardName, title, description, dueDate);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
            Task task = res.Value;
            TaskModel taskModel = new TaskModel(this, task.CreationTime, task.Title, task.Description, task.DueDate, task.emailAssignee, task.Id , user);
            return taskModel;
        }

    }
}
