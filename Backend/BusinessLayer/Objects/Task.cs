using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestProject")]

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Task
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int _id;
        private DateTime creationTime;
        private DateTime dueDate;
        private string title;
        private string assignee;
        private string description;
        private int _columnId;
        private TaskDTO _taskDTO = null;
        public int Id { get { return _id; } }
        public DateTime CreationTime { get { return creationTime; } }
        public DateTime DueDate { get { return dueDate; } }
        public string Title { get { return title; } }
        public string Assignee { get { return assignee; } }
        internal TaskDTO taskDTO { get { return _taskDTO; } }
        public string Description { get { return description; } }

        private static int MAX_TITLE_LENGTH = 50;
        private static int MAX_DESCRIPTION_LENGTH = 300;

        public Task(int id, DateTime dueDate, string title, string description, string assignee, int columnId)
        {
            this._id = id;
            this.creationTime = DateTime.Now;
            UpdateTaskDueDate(dueDate);
            UpdateTaskTitle(title);
            UpdateTaskDescription(description);
            UpdateTaskAssignee(assignee);
            _columnId = columnId;
            _taskDTO = new TaskDTO(Id, CreationTime.ToString(), Title, Assignee, DueDate.ToString(), Description, columnId);
             log.Debug("new Task created");
        }
        internal Task(TaskDTO taskDTO)
        {
            this._id = taskDTO.Id;
            this.creationTime = DateTime.Parse(taskDTO.CreationTime);
            this.dueDate = DateTime.Parse(taskDTO.DueDate);
            this.title = taskDTO.Title;
            this.description = taskDTO.Description;
            this.assignee = taskDTO.Assignee;
            _taskDTO = taskDTO;
            log.Debug("new Task created");
        }

        /// <summary>
        /// check the limitations and Update task due date
        /// </summary>
        /// <param name="dueDate">New due date for the task</param>
        public void UpdateTaskDueDate(DateTime dueDate)
        {
            if (dueDate == null)
                throw new Exception("due tile is null");
            if (dueDate < DateTime.Now)
            {
                throw new Exception("the dueDate over");
            }
            if (taskDTO != null)
                taskDTO.DueDate = dueDate.ToString();
            this.dueDate = dueDate;
        }
        /// <summary>
        /// check the limitations and Update task title
        /// </summary>
        /// <param name="title">New title for the task</param>
        public void UpdateTaskTitle(string title)
        {
            if (title == null)
                throw new Exception("title is null");
            if (title.Length == 0)
            {
                throw new Exception("the title can not be empty");
            }
            if (title.Length > MAX_TITLE_LENGTH)
            {
                throw new Exception($"the length can not be over {MAX_TITLE_LENGTH} chracters");
            }
            if (taskDTO != null)
                taskDTO.Title = title;
            this.title = title;
            log.Debug("the title of the Task has been update");
        }
        /// <summary>
        /// check the limitations and Update task description
        /// </summary>
        /// <param name="description">New description for the task</param>
        public void UpdateTaskDescription(string description)
        {
            if (description == null)
            {
                throw new Exception("the description can not be null");
            }
            if (description.Length > MAX_DESCRIPTION_LENGTH)
            {
                throw new Exception($"the length can not be over {MAX_DESCRIPTION_LENGTH} chracters");
            }
            if (taskDTO != null)
                taskDTO.Description = description;
            this.description = description;
            log.Debug("the description of the Task has been update");
        }
        /// <summary>
        /// check the limitations and Update task assignee
        /// </summary>
        /// <param name="assignee">New assignee for the task</param>
        public void UpdateTaskAssignee(string assignee)
        {

            if (taskDTO != null)
                taskDTO.Assignee = assignee;

            this.assignee = assignee;
            log.Debug("the assignee of the Task has been update");
        }

    }


}

