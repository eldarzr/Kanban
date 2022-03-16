using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class BoardDirector
    {
        private IBoardBuilder _builder;

        public IBoardBuilder Builder
        {
            set { _builder = value; }
        }

        public void BuildNewBoard(int id, string boardName, string emailCreator, int columnId)
        {
            this._builder.BuildMinimalBoard(id, boardName, emailCreator);
            this._builder.BuildNewBoardColumns(columnId);
            this._builder.InsertBoardToDB();
        }

        public void LoadBoard(BoardDTO boardDTO)
        {
            this._builder.BuildExistBoard(boardDTO);
            this._builder.BuildExistMembers();
            this._builder.BuildExistColumns();
        }
    }
}
