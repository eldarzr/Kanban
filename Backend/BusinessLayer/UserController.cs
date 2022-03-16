using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestProject")]
namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserController
    {
        private Dictionary<String, User> _users;
        private Dictionary<String, User> _login_users;
        private UserDalController userDalController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserController()
        {
            log.Debug("usercontroller is up");
            _users = new Dictionary<string, User>();
            _login_users = new Dictionary<string, User>();
            userDalController = new UserDalController();
        }

        /// <summary>
        /// register new user
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="password">The password of the user</param>
        public void register(String email, String password)
        {
            uniqueEmail(email);
            User user = new User(email, password);
            _users[email] = user;
            log.Debug($"user {email} registered succesfully");
        }

        /// <summary>check if email exist in the system</summary>
        /// <param name="email">The email address of the user</param>
        private void uniqueEmail(String email)
        {
            if (_users.ContainsKey(email))
                throw new Exception("email is already exist");
        }

        /// <summary>login user to the system. user must be registered</summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="password">The password of the user</param>
        public void login(String email, String password)
        {
            if (!_users.ContainsKey(email))
                throw new Exception("could not found this user in the system");
            if (_login_users.ContainsKey(email))
                throw new Exception("user allready logged in to the system");
            User user = _users[email];
            if (user.VerifyPasswords(password))
                _login_users[email] = user;
            log.Debug($"user {email} logged in succesfully");
        }

        /// <summary>logout user from the system. user must be registered</summary>
        /// <param name="email">The email address of the user</param>
        public void logout(String email)
        {
            if (!_users.ContainsKey(email))
                throw new Exception("could not found this user in the system");
            if (!_login_users.ContainsKey(email))
                throw new Exception("user is not logged in to the system");
            _login_users.Remove(email);
            log.Debug($"user {email} logged out succesfully");
        }

        /// <summary>check if user is logged in allready</summary>
        /// <param name="email">The email address of the user</param>
        public void loginValidate(String email)
        {
            if (email == null)
                throw new Exception("a user cannot be null");

            if (!_login_users.ContainsKey(email))
                throw new Exception("user is not logged in to the system");
        }

        public void ImportUsersFromDB()
        { // a.
            foreach (UserDTO userDTO in userDalController.SelectAll())
            {
                User user = new User(userDTO);
                _users[user.email] = user;
            }
        }

        public void DeleteData()
        {
            userDalController.DeleteData();
        }

    }
}
