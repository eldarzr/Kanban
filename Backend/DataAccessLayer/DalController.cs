using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class DalController
    { 
        protected readonly string _connectionString;
        private readonly string _tableName;
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
        }
        /// <summary>
        /// update variables.
        /// </summary>
        /// <param name="id">the id of the variable that will update</param>
        /// <param name="attributeName">the name</param>
        /// <param name="attributeValue">the value</param>
        /// <returns>returns true or false if update has done. </returns>
        public bool Update(int id, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
                };
                try
                {

                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Debug($"field {attributeName} updated successfully to {attributeValue}");
                }
                catch (Exception e)
                {
                    log.Error($"field {attributeName} could not update to {attributeValue}");
                    throw new Exception($"field {attributeName} could not update to {attributeValue}");
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
        /// update variables.
        /// </summary>
        /// <param name="id">the id of the variable that will update</param>
        /// <param name="attributeName">the name</param>
        /// <param name="attributeValue">the value</param>
        /// <returns>returns true or false if update has done. </returns>
        public bool Update(int id, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
                    log.Debug($"field {attributeName} updated successfully to {attributeValue}");
                }
                catch (Exception e)
                {
                    log.Error($"field {attributeName} could not update to {attributeValue}");
                    throw new Exception($"field {attributeName} could not update to {attributeValue}");
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
        /// select data.
        /// </summary>
        /// <returns>returns list of DTOS. </returns>
        protected List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                       results.Add(ConvertReaderToObject(dataReader));
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
        /// <summary>
        /// convert SQLiteDataReader to object .
        /// </summary>
        /// <param name="reader">the SQL data reader</param>
        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);

        /// <summary>
        /// delete DTO.
        /// </summary>
        /// <param name="DTOObj">a DTO</param>
        /// <returns>returns true or false accordingly to delete. </returns>
        public bool Delete(DTO DTOObj)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where id={DTOObj.Id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Debug($"delete from table {_tableName} successfully");
                }
                catch
                {
                    log.Error($"could not delete from table {_tableName}");
                    throw new Exception($"could not delete from table {_tableName}");
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
        /// delete data.
        /// </summary>
        /// <returns>returns true or false accordingly to delete. </returns>
        public bool DeleteData()
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE FROM {_tableName}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Debug($"delete table {_tableName} successfully");
                }
                catch
                {
                    log.Error($"could not delete table {_tableName}");
                    throw new Exception($"could not delete table {_tableName}");
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
        /// find maxid of all tasks from DB.
        /// </summary>
        /// <returns>returns max id of tasks. </returns>
        public int MaxId()
        {
            int maxID = 1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT MAX(ID) FROM {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    maxID = Convert.ToInt32(command.ExecuteScalar()) + 1;
                    log.Error("the DB did max function successfully");
                }
                catch (Exception e)
                {
                    log.Error($"the DB could not do max function because {e.Message}");
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
            return maxID;
        }
    }
}
