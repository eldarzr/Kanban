using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestProject")]


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class BoardController
    {
        Dictionary<String, Dictionary<String, Board>> _allBoards;
        Dictionary<String, List<Board>> _boardsByUser;
        private UserController _userController;
        private int nextTaskId;
        private int nextColumnId;
        private int nextId;
        private BoardDirector boardDirector;
        private IBoardBuilder boardBuilder;
        private BoardDalController boardDalController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserController userController { get => _userController; }

        public BoardController()
        {
            boardBuilder = new BoardBuilder();
            boardDirector = new BoardDirector();
            boardDirector.Builder = boardBuilder;
            nextId = 1;
            _allBoards = new Dictionary<String, Dictionary<String, Board>>();
            _userController = new UserController();
            _boardsByUser = new Dictionary<string, List<Board>>();
            boardDalController = new BoardDalController();
            log.Debug("BoardController instance created !");
            nextTaskId = (new TaskDalController()).MaxId();
            nextColumnId = (new ColumnDalController()).MaxId();
        }


        /// <summary>
        /// validate all the required conditions before adding a board.
        /// </summary> 
        /// <param name="emailCreator">password of the user to check</param>
        /// <param name="boardName">password of the user to check</param>
        private void BoardValidate(string userEmail,string emailCreator, string boardName)
        {
            userController.loginValidate(userEmail);
            if (!_allBoards.ContainsKey(emailCreator))
                throw new Exception($"user {emailCreator} does not have boards");
            if (!_allBoards[emailCreator].ContainsKey(boardName))
                throw new Exception($"board {boardName} by {emailCreator} does not exist ");
        }

        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>

        public void AddBoard(String userEmail, String boardName)
        {
            userController.loginValidate(userEmail);
            if (!_allBoards.ContainsKey(userEmail)) {
                _allBoards[userEmail] = new Dictionary<string, Board>();
                _boardsByUser[userEmail] = new List<Board>();
            }

            if (_allBoards[userEmail].ContainsKey(boardName))
                throw new Exception("there is already board in this name");

            //Board board = new Board(boardName, userEmail, nextId, nextColumnId);
            boardDirector.BuildNewBoard(nextId, boardName, userEmail, nextColumnId);
            Board board = boardBuilder.GetBoard();
            nextId++;
            nextColumnId = nextColumnId + 3;
            _allBoards[userEmail][boardName] = board;
            _boardsByUser[userEmail].Add(_allBoards[userEmail][boardName]);
            log.Info($"the created board add to the all boards list successfully");
        }
        /// <summary>
        /// Join to an exist board.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board to join</param>

        public void JoinBoard(String userEmail, String creatorEmail, String boardName)
        {
            BoardValidate(userEmail,creatorEmail, boardName);
  

            _allBoards[creatorEmail][boardName].JoinBoard(userEmail);
            if (!_boardsByUser.ContainsKey(userEmail))
            {
                _boardsByUser[userEmail] = new List<Board>();
            }
            _boardsByUser[userEmail].Add(_allBoards[creatorEmail][boardName]);
            log.Debug($"user {userEmail} join to {boardName} board successfully of the creator {creatorEmail}");
        }

        /// <summary>
        /// remove a board.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board to remove</param>
        public void RemoveBoard(string userEmail, string creatorEmail, string boardName)
        {
            BoardValidate(userEmail, creatorEmail, boardName);

            if (!userEmail.Equals(creatorEmail))
                throw new Exception($"the user {userEmail} is not the creator");

            foreach (String b in _allBoards[creatorEmail][boardName].boardMembers)
                _boardsByUser[b].Remove(_allBoards[creatorEmail][boardName]);

            _allBoards[creatorEmail][boardName].DeleteFromDB();
                 _allBoards[creatorEmail].Remove(boardName);
            
            log.Debug($"board {boardName} that created by {creatorEmail} has been removed");
        }


        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>returns list of all tasks that in progress by a specific user.
        public IList<Task> InProgressTasks(String email)
        {
            userController.loginValidate(email);
            IList<Task> list = new List<Task>();
            foreach (Board board in _boardsByUser[email])
            {
                foreach (Task task in board.getInProgress(email))
                {
                        list.Add(task);
                }

            }
            return list;
        }


        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID.</param>
        /// <param name="taskId">the id of the task</param>
        /// <param name="emailAssignee">Email of the user that take the task</param>
        /// <returns>returns list of all tasks that in progress by a specific user.
        public void AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {

            BoardValidate(userEmail, creatorEmail, boardName);
            _allBoards[creatorEmail][boardName].AssignTask(userEmail,columnOrdinal, taskId,emailAssignee);
            log.Debug($"{emailAssignee} has been set to be the Assignee of {boardName} in the taskId : {taskId}");
        }

        /// <summary>
        /// Returns the names of all boards a user is memeber of.
        /// </summary>
        /// <param name="userEmail">The email of the user. Must be logged-in.</param>
        /// <returns>All the names of the boards the user is created or joined</returns>
        public IList<String> GetBoardNames(String userEmail)
        {

            IList<String> boardNames = new List<String>();
            foreach (Board b in _boardsByUser[userEmail])
                boardNames.Add(b.Name);
            return boardNames;

        }

        /// <summary>
        /// Returns the names of all boards a user is memeber of.
        /// </summary>
        /// <param name="userEmail">The email of the user. Must be logged-in.</param>
        /// <returns>All the names of the boards the user is created or joined</returns>
        public IList<(String, String)> GetBoardNamesCreators(String userEmail)
        {
            IList<(String , String)> boardNames = new List<(String, String)>();
            if(_boardsByUser.ContainsKey(userEmail))
                foreach (Board b in _boardsByUser[userEmail])
                    boardNames.Add((b.CreatorEmail, b.Name));
            return boardNames;

        }

        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. </param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        public void UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate,String userEmail)
        {
            BoardValidate(userEmail, email, boardName);
            _allBoards[email][boardName].UpdateTaskDueDate(userEmail,columnOrdinal, taskId, dueDate);
            log.Debug("the task due date has been update");
        }
        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title, String userEmail)
        {
            BoardValidate(userEmail, email, boardName);
            _allBoards[email][boardName].UpdateTaskTitle(userEmail,columnOrdinal, taskId, title);
            log.Debug("the task title has been update");
        }
         /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user.</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description, String userEmail)
        {
            BoardValidate(userEmail, email, boardName);
            _allBoards[email][boardName].UpdateTaskDescription(userEmail,columnOrdinal, taskId, description);
            log.Debug("the task Description has been update");
        }
          /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. </param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        public void AdvanceTask(string userEmail,string email, string boardName, int columnOrdinal, int taskId) {
            BoardValidate(userEmail, email, boardName);
            userController.loginValidate(userEmail);
            _allBoards[email][boardName].MoveTask(userEmail,columnOrdinal, taskId);
        }
        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value.</param>
        public void LimitColumn(String userEmail,string email, string boardName, int columnOrdinal, int limit)
        {
            BoardValidate(userEmail, email, boardName);
            userController.loginValidate(userEmail);
            _allBoards[email][boardName].LimitColumn(userEmail,columnOrdinal, limit);
        }
        /// <summary>
        /// Get the limit of a specific column
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The limit of the column.</returns>
        public int GetColumnLimit(string userEmail,string email, string boardName, int columnOrdinal)
        {
            BoardValidate(userEmail, email, boardName);
            return _allBoards[email][boardName].GetColumnLimit(userEmail,columnOrdinal);
        }
         /// <summary>
        /// Get the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The name of the column.</returns>
        public String GetColumnName(String userEmail,string email, string boardName, int columnOrdinal) {

            BoardValidate(userEmail, email, boardName);
            return _allBoards[email][boardName].ColumnName(columnOrdinal, userEmail);

        }
        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="email">Email of the user. </param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        public Task AddTask(string userEmail,string emailCreator, string boardName, string title, string description, DateTime dueDate)
        {
            BoardValidate(userEmail, emailCreator, boardName);
            if (description == null)
                description = "";
          Task task= _allBoards[emailCreator][boardName].AddTask(nextTaskId, dueDate, title, description, userEmail);
            nextTaskId++;
            return task;
        }

        public void AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            BoardValidate(userEmail,creatorEmail, boardName);
            _allBoards[creatorEmail][boardName].AddColumn(userEmail, columnOrdinal, columnName,nextColumnId);
            nextColumnId++;

        }

        public void RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            BoardValidate(userEmail, creatorEmail, boardName);
            _allBoards[creatorEmail][boardName].RemoveColumn(userEmail, columnOrdinal);
        }

        public void RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            BoardValidate(userEmail, creatorEmail, boardName);
            _allBoards[creatorEmail][boardName].RenameColumn(userEmail, columnOrdinal, newColumnName);
        }

        public void MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            BoardValidate(userEmail, creatorEmail, boardName);
            _allBoards[creatorEmail][boardName].MoveColumn(userEmail, columnOrdinal, shiftSize);
        }

        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns> return a list of all  the task in a specific column.
        public IList<Task> GetColumn(string userEmail, string email, string boardName, int columnOrdinal)
        {
            BoardValidate(userEmail, email, boardName);
          
            return _allBoards[email][boardName].getColumn(userEmail, columnOrdinal);
        }

        public int getNumOfColumns(string userEmail, string email, string boardName)
        {
            BoardValidate(userEmail, email, boardName);
            return _allBoards[email][boardName].getNumOfColumns();
        }

        /// <summary>import all boards from DB</summary>
        public void ImportBoardsFromDB()
        {
            foreach (BoardDTO boardDTO in boardDalController.SelectAll())
            {
                //Board board = new Board(boardDTO);
                boardDirector.LoadBoard(boardDTO);
                Board board = boardBuilder.GetBoard();
                string creator = board.CreatorEmail;
                if (!_allBoards.ContainsKey(creator))
                    _allBoards[creator] = new Dictionary<string, Board>();
                _allBoards[creator][board.Name] = board;
                foreach(string member in board.boardMembers)
                {
                    if (!_boardsByUser.ContainsKey(member))
                        _boardsByUser[member] = new List<Board>();
                    _boardsByUser[member].Add(board);
                }
                if (board.Id >= nextId)
                    nextId = board.Id + 1;
            }
        }
        /// <summary>delete all boards from DB</summary>
        public void DeleteData()
        {
            boardDalController.DeleteData();
            new TaskDalController().DeleteData();
            new BoardMembersDalController().DeleteData();
            new ColumnDalController().DeleteData();
        }

    }
}
