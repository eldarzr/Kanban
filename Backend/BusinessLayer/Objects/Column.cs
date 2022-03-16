using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestProject")]

namespace IntroSE.Kanban.Backend.BusinessLayer.Objects
{
    internal class Column
    {
        private string name;
        private Dictionary<int, Task> tasks;
        private int limitation;
        private int _ordinal;
        private int _id;
        private ColumnDTO _columnDTO;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string Name { get => name;  }
        public Dictionary<int, Task> Tasks { get => tasks; }
        public int Limitation { get => limitation; set { LimitColumn(value); } }
        public int Ordinal { get => _ordinal; }
        public int ID { get => _id; }
        public ColumnDTO columnDTO { get => _columnDTO; }


        public Column(string name, int ordinal, int id , int boardID)
        {
            this.name = name;
            tasks = new Dictionary<int, Task>();
            Limitation = -1;
            _ordinal = ordinal;
            _id = id;
            _columnDTO = new ColumnDTO(ID, name, ordinal, limitation, boardID);
            columnDTO.Insert();
            log.Debug($"Column {name} created succesfully");
        }
        public Column(ColumnDTO columnDTO)
        {
            this.name = columnDTO.ColumnName;
            tasks = new Dictionary<int, Task>();
            Limitation = columnDTO.Limitation;
            _ordinal = columnDTO.Ordinal;
            _columnDTO = columnDTO;
            _id = columnDTO.Id;
            ImportTasksFromDB();
            log.Debug($"Column {name} created succesfully");
        }

        public bool isEmpty()
        {
            return (tasks.Count() == 0);
        }


        public void RenameColumn(string newColumnName)
        {
            if (newColumnName == null)
            {
                throw new Exception("column name cant be null");
                log.Error("column name cant be null");
            }
            columnDTO.ColumnName = newColumnName;
            name = newColumnName;
        }


        public void changeOrdinal(int columnOrdinal)
        {
            columnDTO.Ordinal = columnOrdinal;
            _ordinal = columnOrdinal;
        }


        /// <summary>
        /// limit the size of the column.
        /// </summary>
        /// <param name="value">the size of column</param>
        /// 
        public void LimitColumn(int value)
        {
            if (value < -1)
                throw new Exception($"can not limit column to be a negative number");

          else if (value != -1 && Tasks.Count > value)
                throw new Exception($"can not limit column to {value} because there are {Tasks.Count} tasks");

            if(columnDTO!=null)
                columnDTO.Limitation = value;
            limitation = value;
        }
        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="taskId">Id of the task</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date of the new task</param>
        /// <param name="userEmail">The user that adding the task</param>
        /// <param name="boardId">Id of the relevant board</param>
        public void AddTask(int taskId, DateTime dueDate, String title, String description, string userEmail)
        {
            CheckLimitation();
            Task task = new Task(taskId, dueDate, title, description, userEmail, ID);
            columnDTO.AddTask(task.taskDTO);
            Tasks[taskId] = task;
        }

        /// <summary>
        /// add a new task.
        /// </summary>
        /// <param name="task">the task id</param>
        public void AddTask(Task task)
        {
            CheckLimitation();
            task.taskDTO.ColumnId = ID;
            Tasks[task.Id] = task;
        }
        /// <summary>
        /// get task.
        /// </summary>
        /// <param name="id">the id of a spesific task</param>
        /// <param name="userEmail">the user that take the task -Assingee </param>
        /// <returns>returns the task id</returns>
        public Task GetTask(int id, string userEmail)
        {
            IdAssingeeValidation(userEmail, id);
            return Tasks[id];
        }
        /// <summary>
        /// remove task.
        /// </summary>
        /// <param name="id">the id of a spesific task</param>
        /// <returns>returns the task id</returns>
        public Task RemoveTask(int id)
        {
            Task task = Tasks[id];
            Tasks.Remove(id);
            return task;
        }


        public int getNumberOfTasks()
        {
            return tasks.Count();
        }


        /// <summary>
        /// check that the column is not exceed the limit.
        /// </summary>
        private void CheckLimitation()
        {
            if (Limitation != -1 & Tasks.Count >= limitation)
                throw new Exception($"this column has came to the limitation");
        }

        /// <summary>
        /// check if the user is the assignee and also if the task is exist.
        /// </summary>
        /// <param name="id">the id of a spesific task</param>
        /// <param name="userEmail">the user that take the task -Assingee </param>
       
        private void IdAssingeeValidation(string userEmail, int id)
        {
            if (!tasks.ContainsKey(id))
                throw new Exception($"there is no such task with the id : {id}");
            if (!userEmail.Equals(tasks[id].Assignee))
                throw new Exception($"user {userEmail} is not the assignee!");
        }
        /// <summary>
        /// update the description of the task.
        /// </summary>
        /// <param name="taskId">The task ID</param>
        /// <param name="userEmail">the user that take tha task-Assignee</param>
        /// <param name="description">The new description of the task</param>
        internal void UpdateTaskDescription(int taskId, string userEmail, string description)
        {
            IdAssingeeValidation(userEmail, taskId);
            tasks[taskId].UpdateTaskDescription(description);
        }
        /// <summary>
        /// update the due date of the task.
        /// </summary>
        /// <param name="userEmail">the user that take tha task-Assignee</param>
        /// <param name="taskId">The task ID</param>
        /// <param name="dueDate">The new dueDate of the task</param>
        internal void UpdateTaskDueDate(int taskId, string userEmail, DateTime dueDate)
        {
            IdAssingeeValidation(userEmail, taskId);
            tasks[taskId].UpdateTaskDueDate(dueDate);
        }
        /// <summary>
        /// update the assignee of the task.
        /// </summary>
        /// <param name="assignee">the user that take the task-Assignee</param>
        /// <param name="taskId">The task ID</param>
        /// <param name="title">The new title of the task</param>
        internal void UpdateTaskAssignee(int taskId, string assignee)
        {
            if (!tasks.ContainsKey(taskId))
                throw new Exception($"there is no such task in id {taskId}");

            tasks[taskId].UpdateTaskAssignee(assignee);
        }
        /// <summary>
        /// update the title of the task.
        /// </summary>
        /// <param name="userEmail">the user that take tha task-Assignee</param>
        /// <param name="taskId">The task ID</param>
        /// <param name="title">The new title of the task</param>
        internal void UpdateTaskTitle(int taskId, string userEmail, string title)
        {
            IdAssingeeValidation(userEmail, taskId);
            tasks[taskId].UpdateTaskTitle(title);
        }


        /// <summary>
        /// import all the column tasks from the database.
        /// </summary>
        private void ImportTasksFromDB()
        {
            foreach (TaskDTO taskDTO in columnDTO.ImportTasks())
            {
                Task task = new Task(taskDTO);
                tasks[taskDTO.Id] = task;
            }
        }

        /// <summary>
        /// delete all the Columns and Tasks from the database.
        /// </summary>
        public void DeleteFromDB()
        {
            columnDTO.Delete();
        }
    }

}
