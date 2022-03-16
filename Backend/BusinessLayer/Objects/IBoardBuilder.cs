using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    interface IBoardBuilder
    {
        //make empty board
        void Reset();

        //make regular board without columns
        void BuildMinimalBoard(int id, string boardName, string emailCreator);

        //add default columns to the board
        public void BuildNewBoardColumns(int columnId);

        //save the board to the DB
        public void InsertBoardToDB();

        //load the board from dto
        void BuildExistBoard(BoardDTO boardDTO);

        //load the columns to the board from dto
        void BuildExistColumns();

        //load the members to the board from dto
        void BuildExistMembers();
        
        public Board GetBoard();
    }
}
