using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;



namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    class BoardDalController : DalController
    {
        private const string boardTableName = "Boards";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BoardDalController() : base(boardTableName)
        {

        }


        /*        public List<BoardDTO> SelectAllBoards()
                {
                    List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();

                    return result;
                }*/

        /// <summary>
        /// validate if allow to insert and insert the board.
        /// </summary>
        /// <param name="board">a board</param>
        /// <returns>returns true or false accordingly to insert. </returns>

        public bool Insert(BoardDTO board)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {boardTableName} ({DTO.IDColumnName} ,{BoardDTO.BoardNameColumnName},{BoardDTO.EmailCreatorColumnName}) " +
                        $"VALUES (@idVal,@boardNameVal,@creatorVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.Id);
                    SQLiteParameter bordNameParam = new SQLiteParameter(@"boardNameVal", board.BoardName);
                    SQLiteParameter emailParam = new SQLiteParameter(@"creatorVal", board.EmailCreator);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(bordNameParam);
                    command.Parameters.Add(emailParam);

                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Debug($"inserted board {board.BoardName} succesfully");
                }
                catch
                {
                    log.Error($"could not insert board {board.BoardName}");
                    throw new Exception($"could not insert board {board.BoardName}");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        /// <summary>
        /// convert SQLiteDataReader to object .
        /// </summary>
        /// <param name="reader">the SQL data reader</param>
        /// <returns>returns new board DTO. </returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result = new BoardDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
            return result;
        }
        /// <summary>
        /// return all the boards DTO.
        /// </summary>
        /// <returns>returns a list of all DTO boards. </returns>
        public List<BoardDTO> SelectAll()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            foreach (BoardDTO boardDTO in result)
                boardDTO.ImportBoardMembers();
            return result;
        }
        /// <summary>
        /// delete boards.
        /// </summary>
        /// <param name="boardDTO">a board DTO</param>
        /// <returns>returns true or false accordingly to delete. </returns>
        public bool DeleteBoard(BoardDTO boardDTO)
        {
            BoardMembersDalController boardMembersDalController = new BoardMembersDalController();
            boardMembersDalController.Delete(boardDTO);
            log.Debug($"deleted board {boardDTO.BoardName} succesfully");
            return Delete(boardDTO);
        }
    }

}

