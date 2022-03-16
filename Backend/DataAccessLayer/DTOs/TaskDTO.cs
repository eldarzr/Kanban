using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
   internal class TaskDTO : DTO
    {
        public const string TaskColumnIdColumnName = "ColumnId";
        public const string TaskCreationTimeColumnName = "CreationTime";
        public const string TaskAssigneeColumnName = "Assignee";
        public const string TaskTitleColumnName = "Title";
        public const string TaskDueDateColumnName = "DueDate";
        public const string TaskColumnColumnName = "Column";
        public const string TaskDescriptionColumnName = "Description";

        private int _columnId;
        public int ColumnId { get => _columnId; set { _columnId = value; _controller.Update(Id, TaskColumnIdColumnName, value); } }
        private string _creationTime;
        public string CreationTime { get => _creationTime; set { _creationTime = value; _controller.Update(Id, TaskCreationTimeColumnName, value); } }

        private string _assignee;
        public string Assignee { get => _assignee; set { _assignee = value; _controller.Update(Id, TaskAssigneeColumnName, value); } }
        private string _title;
        public string Title { get => _title; set { _title = value; _controller.Update(Id, TaskTitleColumnName, value); } }
        
        private string _dueDate;
        public string DueDate { get => _dueDate; set { _dueDate = value; _controller.Update(Id, TaskDueDateColumnName, value); } }
        private string _description;
        public string Description { get => _description; set { _description = value; _controller.Update(Id, TaskDescriptionColumnName, value); } }
        public TaskDTO(int ID, string CreationTime, string Title, string Assignee, string DueDate, string Description, int columnId) : base(new TaskDalController())
        {
            Id = ID;
            _creationTime = CreationTime;
            _assignee = Assignee;
            _title = Title;
            _dueDate = DueDate;
            _description = Description;
            _columnId = columnId;
        }

        public override bool Insert()
        {
            return ((TaskDalController)_controller).Insert(this);
        }
    }
}
