using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    class ColumnDTO : DTO
    {
        public const string ColumnBoardIdColumnName = "BoardId";
        public const string ColumnOrdinalColumnName = "Ordinal";
        public const string ColumnNameColumnName = "ColumnName";
        public const string ColumnLimitationColumnName = "Limitation";

        private int _boardId;
        public int BoardId { get => _boardId; set { _boardId = value; _controller.Update(Id, ColumnBoardIdColumnName, value); } }
        private int _ordinal;
        public int Ordinal { get => _ordinal; set { _ordinal = value; _controller.Update(Id, ColumnOrdinalColumnName, value); } }

        private string _columnName;
        public string ColumnName { get => _columnName; set { _columnName = value; _controller.Update(Id, ColumnNameColumnName, value); } }
        private int _limitation;
        private TaskDalController taskDalController;
        public int Limitation { get => _limitation; set { _limitation = value; _controller.Update(Id, ColumnLimitationColumnName, value); } }
       
        public ColumnDTO(int id, string columnName, int ordinal, int limitation, int boardId) : base(new ColumnDalController())
        {
            this.Id = id;
            _columnName = columnName;
            _ordinal = ordinal;
            _limitation = limitation;
            _boardId = boardId;
            taskDalController = new TaskDalController();
        }

        public bool Delete()
        {
            taskDalController.DeleteBoardTasks(this);
            return ((ColumnDalController)_controller).Delete(this);
        }

        public override bool Insert()
        {
            return ((ColumnDalController)_controller).Insert(this);
        }

        /// <summary>
        /// Import the tasks of the column from the DB.
        /// </summary>
        /// <returns>list of the tasks.</returns>
        public List<TaskDTO> ImportTasks()
        {
            return taskDalController.SelectTasks(this);
        }

        /// <summary>
        /// Add task to the DB.
        /// </summary>
        /// <param name="taskDTO">task we want to save in the DB</param>
        /// <returns>returns if the insertion succssed. </returns>
        public bool AddTask(TaskDTO taskDTO)
        {
            return taskDTO.Insert();
        }
    }
}
