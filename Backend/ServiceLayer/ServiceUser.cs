using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    internal class ServiceUser
    {
        public UserController userController;
        public BoardController boardController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal ServiceUser()
        {
            log.Debug("service user is up");
            boardController = new BoardController();
            userController = boardController.userController;

        }

        ///<summary>This method registers a new user to the system.</summary>
        ///<param name="email">the user e-mail address, used as the username for logging the system.</param>
        ///<param name="password">the user password.</param>
        ///<returns cref="Response">The response of the action</returns>
        internal Response Register(string email, string password)
        {
            try
            {
                userController.register(email, password);
                return new Response();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response(e.Message);
            }

        }

        internal void LoadData()
        {
            userController.ImportUsersFromDB();
        }
        internal void DeleteData()
        {
            userController.DeleteData();
        }

        /// <summary>
        /// Log in an existing user
        /// </summary>
        /// <param name="email">The email address of the user to login</param>
        /// <param name="password">The password of the user to login</param>
        /// <returns>A response object with a value set to the user, instead the response should contain a error message in case of an error</returns>
        internal Response<User> Login(string email, string password)
        {
            try
            {
                userController.login(email, password);
                User user = new User(email);
                return Response<User>.FromValue(user);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return Response<User>.FromError(e.Message);
            }
        }

        private void ValidateUserLoggin(string email)
        {
            userController.loginValidate(email);
        }

        /// <summary>        
        /// Log out an logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response Logout(string email)
        {
            try
            {
                userController.logout(email);
                return new Response();
            }
            catch(Exception e)
            {
                log.Error(e.Message);
                return new Response(e.Message);
            }

        }

        /// <summary>
        /// Removes a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">The name of the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddBoard(string email, string name)
        {
            try
            {
                boardController.AddBoard(email, name);
                return new Response();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Removes a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">The name of the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                boardController.JoinBoard(userEmail, creatorEmail, boardName);
                return new Response();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response object with a value set to the list of tasks, The response should contain a error message in case of an error</returns>
        public Response RemoveBoard(string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                ValidateUserLoggin(userEmail);
                boardController.RemoveBoard(userEmail, creatorEmail, boardName);
                return new Response();
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                return new Response(e.Message);
            }
        }


    }
}
