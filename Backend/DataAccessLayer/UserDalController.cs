using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
//using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class UserDalController : DalController
    {
        private const string UserTableName = "Users";

        public UserDalController() : base(UserTableName)
        {

        }
        // <summary>
        /// convert SQLiteDataReader to object .
        /// </summary>
        /// <param name="reader">the SQL data reader</param>
        /// <returns>returns new board DTO. </returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO userDTO = new UserDTO(reader.GetString(0), reader.GetString(1));
            return userDTO;
        }
        /// <summary>
        /// validate if allow to insert and insert the user.
        /// </summary>
        /// <param name="user">a user </param>
        /// <returns>returns true or false accordingly to insert. </returns>
        public bool Insert(UserDTO user)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserTableName} ({UserDTO.UserEmailColumnName},{UserDTO.UserPasswordColumnName}) " +
                        $"VALUES (@emailVal,@passwordVal);";

                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Pass);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    log.Error($"the DB inserted user {user.Email} successfully");
                }
                catch (Exception e)
                {
                    log.Error($"the DB could not add user {user.Email} because {e.Message}");
                    throw new Exception($"the DB could not add user {user.Email} because {e.Message}");
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
        /// return all the users DTO.
        /// </summary>
        /// <returns>returns a list of all DTO users. </returns>
        public List<UserDTO> SelectAll()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();
            return result;
        }

    }
}
