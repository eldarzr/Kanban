using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class BoardBuilder : IBoardBuilder
    {

        private Board _board;

        public  BoardBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            _board = new Board();
        }

        public void BuildMinimalBoard(int id, string boardName, string emailCreator)
        {
            _board.BasicBoard(id, boardName, emailCreator);
        }

        public void BuildNewBoardColumns(int columnId)
        {
            _board.InitialzieColums(columnId);
        }


        public void InsertBoardToDB()
        {
            _board.InsertBoard();
        }

        public void BuildExistBoard(BoardDTO boardDTO)
        {
            _board.LoadBoard(boardDTO);
        }

        public void BuildExistColumns()
        {
            _board.ImportColumnsFromDB();
        }

        public void BuildExistMembers()
        {
            _board.ImportMembers();
        }

        public Board GetBoard()
        {
            Board board = _board;
            Reset();
            return board;
        }
    }
}
