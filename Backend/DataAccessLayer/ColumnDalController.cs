using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class ColumnDalController : DalController
    {
        private const string ColumnTableName = "Columns";

        public ColumnDalController() : base(ColumnTableName)
        {

        }
        // <summary>
        /// convert SQLiteDataReader to object .
        /// </summary>
        /// <param name="reader">the SQL data reader</param>
        /// <returns>returns new board DTO. </returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            ColumnDTO columnDTO = new ColumnDTO(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4));
            return columnDTO;
        }
        /// <summary>
        /// validate if allow to insert and insert the column.
        /// </summary>
        /// <param name="user">a user </param>
        /// <returns>returns true or false accordingly to insert. </returns>
        public bool Insert(ColumnDTO columnDTO)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} ({DTO.IDColumnName},{ColumnDTO.ColumnNameColumnName},{ColumnDTO.ColumnOrdinalColumnName},{ColumnDTO.ColumnLimitationColumnName},{ColumnDTO.ColumnBoardIdColumnName}) " +
                        $"VALUES (@idVal,@columnNameVal,@columnOrdinalVal,@columnLimitationVal,@boardIdVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", columnDTO.Id);
                    SQLiteParameter columnNameParam = new SQLiteParameter(@"columnNameVal", columnDTO.ColumnName);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinalVal", columnDTO.Ordinal);
                    SQLiteParameter columnLimitationParam = new SQLiteParameter(@"columnLimitationVal", columnDTO.Limitation);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", columnDTO.BoardId);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(columnNameParam);
                    command.Parameters.Add(columnOrdinalParam);
                    command.Parameters.Add(columnLimitationParam);
                    command.Parameters.Add(boardIdParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    log.Error($"the DB inserted column {columnDTO.ColumnName} successfully");
                }
                catch (Exception e)
                {
                    log.Error($"the DB could not add column {columnDTO.ColumnName} because {e.Message}");
                    throw new Exception($"the DB could not add column {columnDTO.ColumnName} because {e.Message}");
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
        /// return all the column DTO.
        /// </summary>
        /// <returns>returns a list of all DTO users. </returns>
        public List<ColumnDTO> SelectAll()
        {
            List<ColumnDTO> result = Select().Cast<ColumnDTO>().ToList();
            return result;
        }

        /// <summary>
        /// select tasks.
        /// </summary>
        /// <param name="boardDTO">spesific board that we take the tasks from it.</param>
        /// <returns>returns a list of all DTO tasks. </returns>
        public List<ColumnDTO> SelectColumns(BoardDTO boardDTO)
        {
            List<ColumnDTO> results = new List<ColumnDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {ColumnTableName} where BoardId={boardDTO.Id};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add((ColumnDTO)ConvertReaderToObject(dataReader));
                    }
                    log.Debug("the DB did select function successfully");
                }
                catch (Exception e)
                {
                    log.Error($"the DB could not do select function because {e.Message}");
                    throw new Exception($"the DB could not do select function because {e.Message}");
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
    }
}
