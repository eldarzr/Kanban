using System;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class TaskDalController: DalController
    {
        private const string TaskTableName = "Tasks";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public TaskDalController() : base(TaskTableName)
        {

        }

        /// <summary>
        /// return all the tasks DTO.
        /// </summary>
        /// <returns>returns a list of all DTO tasks. </returns>
        public List<TaskDTO> SelectAll()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();
            return result;
        }

        /// <summary>
        /// select tasks.
        /// </summary>
        /// <param name="boardDTO">spesific board that we take the tasks from it.</param>
        /// <returns>returns a list of all DTO tasks. </returns>
        public List<TaskDTO> SelectTasks(ColumnDTO columnDTO)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {TaskTableName} where ColumnId={columnDTO.Id};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add((TaskDTO)ConvertReaderToObject(dataReader));
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
        /// validate if allow to insertand insert the task.
        /// </summary>
        /// <param name="task">a task</param>
        /// <returns>returns true or false accordingly to insert. </returns>
        public bool Insert(TaskDTO task)
 
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({DTO.IDColumnName},{TaskDTO.TaskCreationTimeColumnName},{TaskDTO.TaskTitleColumnName},{TaskDTO.TaskAssigneeColumnName},{TaskDTO.TaskDueDateColumnName},{TaskDTO.TaskDescriptionColumnName},{TaskDTO.TaskColumnIdColumnName}) " +
                        $"VALUES (@idVal,@creationTimeVal,@titleVal,@assigneeVal,@dueDateVal,@descriptionVal,@columnIdVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.Id);
                    SQLiteParameter columnIdParam = new SQLiteParameter(@"columnIdVal", task.ColumnId);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTimeVal", task.CreationTime);
                    SQLiteParameter assigneeParam = new SQLiteParameter(@"assigneeVal", task.Assignee);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", task.Title);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", task.DueDate);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", task.Description);
                    //SQLiteParameter columnParam = new SQLiteParameter(@"columnVal", task.Column);



                    command.Parameters.Add(idParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(assigneeParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(columnIdParam);
                    //command.Parameters.Add(columnParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Debug($"inserted task {task.Title} successfully");
                }
                catch (Exception e)
                {
                    log.Error($"could not insert task {task.Title}");
                    throw new Exception($"could not insert task {task.Title}");
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
        /// delete board.
        /// </summary>
        /// <param name="boardDTO">a board DTO</param>
        /// <returns>returns true or false accordingly to delete. </returns>
        public bool DeleteBoardTasks(ColumnDTO columnDTO)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {TaskTableName} where ColumnId={columnDTO.Id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Debug("deleted board tasks succesfully");
                }
                catch
                {
                    log.Error("could not delete tasks");
                    throw new Exception($"could not delete tasks");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }
        // <summary>
        /// convert SQLiteDataReader to object .
        /// </summary>
        /// <param name="reader">the SQL data reader</param>
        /// <returns>returns new board DTO. </returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO result = new TaskDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6));
            return result;

        }
    }
}
