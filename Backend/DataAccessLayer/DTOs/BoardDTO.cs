using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    class BoardDTO : DTO
    {
        public const string BoardNameColumnName = "BoardName";
        public const string EmailCreatorColumnName = "EmailCreator";


        private string _boardName;
        private string _emailCreator;

        private BoardMembersDalController boardMembersDalController;
        private ColumnDalController columnDalController;
        private List<BoardMemberDTO> _members;
        public string BoardName { get => _boardName; set { _boardName = value; _controller.Update(Id, BoardNameColumnName, value); } }
        public string EmailCreator { get => _emailCreator; set { _emailCreator = value; _controller.Update(Id, EmailCreatorColumnName, value); } }
    
        public IList Members { get => _members; }
        public BoardDTO(int ID, string email, string boardName) : base(new BoardDalController())
        {
            Id = ID;
            _boardName = boardName;
            _emailCreator = email;

            _members = new List<BoardMemberDTO>();
            boardMembersDalController = new BoardMembersDalController();
            columnDalController = new ColumnDalController();
        }

        public override bool Insert()
        {
            return ((BoardDalController)_controller).Insert(this);
        }

        /// <summary>
        /// Delete the board with his tasks and members from the DB.
        /// </summary>
        /// <returns>returns if the deletion succssed. </returns>
        public bool Delete()
        {
            boardMembersDalController.DeleteBoardMembers(this);
            return ((BoardDalController)_controller).Delete(this);
        }

        /// <summary>
        /// Add member to the board in the DB.
        /// </summary>
        /// <param name="member">member email</param>
        /// <returns>returns if the insertion succssed. </returns>
        public bool AddMember(string member)
        {
            BoardMemberDTO boardMemberDTO = new BoardMemberDTO(Id, member);
            Members.Add(boardMemberDTO);
            return boardMemberDTO.Insert();
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

        /// <summary>
        /// Import the members of the board from the DB.
        /// </summary>
        public void ImportBoardMembers()
        {
            _members = boardMembersDalController.SelectBoardMember(Id);
        }

        /// <summary>
        /// Import the tasks of the board from the DB.
        /// </summary>
        /// <returns>list of the tasks.</returns>
        public List<ColumnDTO> ImportBoardColumns()
        {
            return columnDalController.SelectColumns(this);
        }



    }
}
