using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.BusinessLayer.Objects;
using log4net;
using log4net.Config;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestProject")]
   


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Board
    {
        private String _name;
        private String _creatorEmail;
        private int _id;
        private readonly int minimalColumns = 2;
        private SortedList<int, Column> _columns;
        private List<String> _boardMembers;
        private BoardDTO _boardDTO;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public String Name { get { return _name; } }
        public String CreatorEmail { get { return _creatorEmail; } }
        public int Id { get { return _id; } }
        public BoardDTO boardDTO { get { return _boardDTO; } }
        public SortedList<int, Column> columns { get { return _columns; } }
        public List<String> boardMembers { get { return _boardMembers; } }

        /// <summary>
        /// import all members of this board from DTO.
        /// </summary>
        public void ImportMembers()
        {
            _boardMembers = new List<string>();
            foreach (BoardMemberDTO boardMemberDTO in boardDTO.Members)
                _boardMembers.Add(boardMemberDTO.Member);
        }

        /// <summary>
        /// create Array for all the columns: backlog, inprogress, done.
        /// </summary>
        public void InitialzieColums(int id)
        {
            _columns = new SortedList<int, Column>();
            _columns[0] = new Column("backlog", 0, id ,Id);
            _columns[1] = new Column("in progress", 1, id+1, Id);
            _columns[2] = new Column("done", 2, id+2 ,Id);
        }
        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="id">Id of the task</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date of the new task</param>
        public Task AddTask(int id, DateTime dueDate, String title, String description, String userEmail)
        {
            ValidateMember(userEmail);
            
            columns[0].AddTask(id, dueDate, title, description, userEmail);
            Task task = columns[0].GetTask(id, userEmail);
            return task;
        }
       /* public Task AddTask(ITask task)
        {
           

            columns[0].AddTask(task.id, task.dueDate, task.title, task.description, task.userEmail);
            Task task1 = columns[0].GetTask(id, userEmail);
            return task;
        }*/
        /// <summary>
        /// adding user to a board.
        /// </summary>
        /// <param name="user">The specific user that want to join to a board</param>
        public void JoinBoard(string user)
        {
            if (boardMembers.Contains(user))
                throw new Exception($"user {user} is allready join to {Name} board.");
            boardDTO.AddMember(user);
            boardMembers.Add(user);
        }


        public void AddColumn(string userEmail, int columnOrdinal, string columnName, int columnID)
        {
            ValidateMember(userEmail);
            int size = _columns.Count();
            if (columnOrdinal > size)
                throw new Exception($"There are only {size} columns, column cannot be add to the {columnOrdinal} place");
            _columns[size] = new Column(columnName,size,columnID,Id);
            shiftColumn(-1*(size - columnOrdinal), size);
    
        }

        public void RemoveColumn(string userEmail, int columnOrdinal)
        {
            ValidateMember(userEmail);
            ValidateColumn(columnOrdinal);

            if (_columns.Count() == minimalColumns)
            {
                throw new Exception($"Column cannot be removed - there are only 2 columns in the board");
                log.Debug("a try to remove column from a board with 2 columns only have been made");
            }

            if (columnOrdinal == 0)
            {
                shiftTaks(columnOrdinal,1);
            }
            else {
                shiftTaks(columnOrdinal,-1); 
            }
            int numberOfColumns = _columns.Count()-1;
            shiftColumn(numberOfColumns - columnOrdinal, columnOrdinal);
            _columns[numberOfColumns].DeleteFromDB();
            _columns.Remove(numberOfColumns);
        }

        private void shiftTaks(int columnOrdinal,int v)
        {
            int spaceLeft = _columns[columnOrdinal + v].Limitation - _columns[columnOrdinal + v].getNumberOfTasks();
            if (columns[columnOrdinal+v].Limitation!= -1 & _columns[columnOrdinal].getNumberOfTasks() > spaceLeft)
                throw new Exception("There is not enough place in the next column for the tasks,therfore the column cannot be removed");
            foreach (var task in _columns[columnOrdinal].Tasks.Values)
            {
                _columns[columnOrdinal + v].AddTask(task);
            }
        }

        internal List<Task> getInProgress(string email)
        {
            ValidateMember(email);
            List<Task> list = new List<Task>();
            for (int i = 1; i <= getNumOfColumns() - 2; i++)
            {
                foreach (Task task in getColumn(email, i))
                {
                    if (task.Assignee.Equals(email))
                        list.Add(task);
                }

            }
            return list;
        }

        internal int getNumOfColumns()
        {
            return _columns.Count();
        }

        public void RenameColumn(string userEmail, int columnOrdinal, string newColumnName)
        {
            ValidateMember(userEmail);
            columns[columnOrdinal].RenameColumn(newColumnName);
        }

        public void MoveColumn(string userEmail , int columnOrdinal, int shiftSize)
        {
            ValidateMember(userEmail);
            if (!columns[columnOrdinal].isEmpty())
            {
                log.Error("cannot move column with tasks!");
                throw new Exception("cannot move column with tasks!");
            }

            shiftColumn(shiftSize, columnOrdinal);

        }

        

        private void shiftColumn(int shiftSize, int columnOrdinal)
        {
            if((columnOrdinal+shiftSize)<0 || (columnOrdinal + shiftSize) >= _columns.Count())
            throw new Exception("shift Size number is invalid - bigger than the number of the columns that can be moved");

            if (shiftSize > 0)
                for (int i = 0; i < shiftSize; i++)
                {
                    swapColumn(columnOrdinal + i, columnOrdinal + i + 1);
                }
            else
                for (int i = 0; i < Math.Abs(shiftSize); i++)
                {
                    swapColumn(columnOrdinal - i, columnOrdinal - i - 1);
                }
        }


        private void swapColumn(int ordinal1, int ordinal2)
        {
            Column column1 = columns[ordinal1];
            Column column2 = columns[ordinal2];
            column1.changeOrdinal(ordinal2);
            column2.changeOrdinal(ordinal1);
            columns[ordinal1] = column2;
            columns[ordinal2] = column1;
            log.Debug("move column succesfully");
        }

        /// <summary>
        /// limit the size of the column.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="size">size of the column</param>
        public bool LimitColumn(String userEmail,int columnOrdinal ,int size)
        {
            ValidateColumn(columnOrdinal);
            ValidateMember(userEmail);
            columns[columnOrdinal].LimitColumn(size);

            return true;
            
        }

         /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task ID</param>
        public void MoveTask(String userEmail,int columnOrdinal, int taskId)
        {
            ValidateMember(userEmail);
            CheckDoneColumn(columnOrdinal);

            Task task = columns[columnOrdinal].GetTask(taskId, userEmail);
            columns[columnOrdinal + 1].AddTask(task);
            columns[columnOrdinal].RemoveTask(taskId);


        }
        /// <summary>
        /// returns the columns limit.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>returns the column limit</returns>
        public int GetColumnLimit(String userEmail,int columnOrdinal) {
            ValidateMember(userEmail);
            ValidateColumn(columnOrdinal);

            return columns[columnOrdinal].Limitation;
        }

        /// <summary>
        /// assign a task for user .
        /// </summary>
        /// <param name="userEmail">email of the user </param>
        /// <param name="columnOrdinal">The column name</param>
        /// <param name="taskId">The task id</param>
        /// <param name="emailAssignee">The email of the assignee of this task.</param>

        public void AssignTask(string userEmail, int columnOrdinal, int taskId, string emailAssignee)
        {
            CheckDoneColumn(columnOrdinal);
            ValidateMember(userEmail);
            ValidateMember(emailAssignee);
            columns[columnOrdinal].UpdateTaskAssignee(taskId,emailAssignee);            
        }
        /// <summary>
        /// checking if the user is a member of the board.
        /// <param name="member">email of the user </param>
        /// </summary>
        private void ValidateMember(String member)
        {
            if (!_boardMembers.Contains(member))
            {
                log.Error("User that is not a member of the board has enterd");
                throw new Exception($"{member}user is not member of this board!");
            }
        }
        /// <summary>
        /// update the title of the task.
        /// </summary>
        /// <param name="userEmail">The email of the user that update task title</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task ID</param>
        /// <param name="title">The new title of the task</param>
        public void UpdateTaskTitle(String userEmail , int columnOrdinal, int taskId, string title)
        {

            CheckDoneColumn(columnOrdinal);
            ValidateMember(userEmail);
            columns[columnOrdinal].UpdateTaskTitle(taskId, userEmail, title);

        }


        /// <summary>
        /// update the description of the task.
        /// </summary>
        /// <param name="userEmail">The email of the user that update task description</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task ID</param>
        /// <param name="description">The new description of the task</param>
        public void UpdateTaskDescription(String userEmail, int columnOrdinal, int taskId, string description)
        {

            CheckDoneColumn(columnOrdinal);
            ValidateMember(userEmail);
            columns[columnOrdinal].UpdateTaskDescription(taskId, userEmail, description);

        }
        /// <summary>
        /// update the due date of the task.
        /// </summary>
        /// <param name="userEmail">The email of the user that update task dueDate</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task ID</param>
        /// <param name="dueDate">The new dueDate of the task</param>
        public void UpdateTaskDueDate(String userEmail , int columnOrdinal, int taskId, DateTime dueDate)
        {
            CheckDoneColumn(columnOrdinal);
            ValidateMember(userEmail);
            columns[columnOrdinal].UpdateTaskDueDate(taskId, userEmail, dueDate);

        }
        /// <summary>
        /// returns the column name.
        /// </summary>
        /// <param name="userEmail">The email of the user that ask the column name</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>returns the column limit</returns>
        internal string ColumnName(int columnOrdinal, string userEmail)
        {
            ValidateColumn(columnOrdinal);
            ValidateMember(userEmail);
            return columns[columnOrdinal].Name;
        }

        /// <summary>
        /// return the relevant column.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        ///<param name = "userEmail" >the user that doing the action</param>
        /// <returns>returns the tasks list of the column</returns>
        public IList<Task> getColumn(string userEmail,int columnOrdinal) {

            ValidateColumn(columnOrdinal);
            IList<Task> column = new List<Task>();
            ValidateMember(userEmail);
            foreach (var t in columns[columnOrdinal].Tasks)
                    column.Add(t.Value);

            return column;

        }

        /// <summary>
        /// validate if the column is done.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        private void CheckDoneColumn(int columnOrdinal)
        {
            ValidateColumn(columnOrdinal);
 
            if (columnOrdinal >= getNumOfColumns()-1 | columnOrdinal<0)
            {
                log.Error("someone tried to update task in 'done' column");
                throw new Exception("a Task in 'done' state cannot be updated.");
            }
        }

        /// <summary>
        /// check if the given column ordinal is fit to any of the existence columns.
        /// </summary>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        private void ValidateColumn(int columnOrdinal)
        {
            if (!_columns.ContainsKey(columnOrdinal))
            {
                throw new Exception($"There is no such column in the {columnOrdinal} place");
                log.Error("a try to remove column that doesnt exist has been made");
            }
        }

        /// <summary>
        /// import all the tasks from the database.
        /// </summary>
        public void ImportColumnsFromDB()
        {
            foreach (ColumnDTO columnDTO in boardDTO.ImportBoardColumns())
            {
                Column column = new Column(columnDTO);
                int columnOrdinal = columnDTO.Ordinal;
                columns[columnOrdinal] = column;
            }
        }
        /// <summary>
        /// delete all the boards from the database.
        /// </summary>
        public void DeleteFromDB()
        {
            foreach (Column column in columns.Values)
                column.DeleteFromDB();
            boardDTO.Delete();
        }

        public Board()
        {
            _boardMembers = new List<string>();
            _columns = new SortedList<int, Column>();
        }

        public void BasicBoard(int id, string name, string email)
        {
            this._name = name;
            this._creatorEmail = email;
            this._id = id;
            boardMembers.Add(email);
        }

        public void InsertBoard()
        {
            _boardDTO = new BoardDTO(Id, CreatorEmail, Name);
            boardDTO.Insert();
            boardDTO.AddMember(CreatorEmail);
        }

        public void LoadBoard(BoardDTO boardDTO)
        {
            _boardDTO = boardDTO;
            this._name = boardDTO.BoardName;
            this._creatorEmail = boardDTO.EmailCreator;
            this._id = boardDTO.Id;
        }

    }
}
