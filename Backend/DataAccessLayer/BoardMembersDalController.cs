using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class BoardMembersDalController : DalController
    {
        private const string membersTableName = "Members";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BoardMembersDalController() : base(membersTableName)
        {

        }


        /// <summary>
        /// validate if allow to insertand insert the board member.
        /// </summary>
        /// <param name="boardMember">a board mamber</param>
        /// <returns>returns true or false accordingly to insert. </returns>

        public bool Insert(BoardMemberDTO boardMember)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {membersTableName} ({DTO.IDColumnName} ,{BoardMemberDTO.MemberColumnName}) " +
                        $"VALUES (@idVal,@memberVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", boardMember.Id);
                    SQLiteParameter boardMemberParam = new SQLiteParameter(@"memberVal", boardMember.Member);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(boardMemberParam);
                    command.Prepare();

                    log.Debug("inserted board members succesfully");
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("could not insert board members");
                    throw new Exception("could not import board members");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        public bool DeleteBoardMembers(BoardDTO boardDTO)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {membersTableName} where Id={boardDTO.Id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Debug("deleted board members succesfully");
                }
                catch
                {
                    log.Error("could not delete tasks");
                    throw new Exception($"could not delete members");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        /// <summary>
        /// select board member.
        /// </summary>
        /// <param name="id">id of a board member</param>
        /// <returns>returns a list of all DTO boards members. </returns>
        public List<BoardMemberDTO> SelectBoardMember(int id)
        {
            List<BoardMemberDTO> results = new List<BoardMemberDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {membersTableName} where id={id};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add((BoardMemberDTO)ConvertReaderToObject(dataReader));
                    }
                    log.Debug("imported board members succesfully");
                }
                catch (Exception e)
                {
                    log.Error("could not import board members");
                    throw new Exception("could not import board members");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }
        /// <summary>
        /// convert SQLiteDataReader to object .
        /// </summary>
        /// <param name="reader">the SQL data reader</param>
        /// <returns>returns new board DTO. </returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardMemberDTO result = new BoardMemberDTO(reader.GetInt32(0), reader.GetString(1));
            return result;

        }
        /// <summary>
        /// return all the boards members DTO.
        /// </summary>
        /// <returns>returns a list of all DTO boards members. </returns>
        public List<BoardMemberDTO> SelectAll()
        {
            List<BoardMemberDTO> result = Select().Cast<BoardMemberDTO>().ToList();
            return result;
        }


    }
}
