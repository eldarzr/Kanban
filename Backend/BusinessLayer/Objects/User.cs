using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestProject")]

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class User
    {
        private string _email;
        private Password password;
        private UserDTO _userDTO;

        public string email { get { return _email; } }
        public UserDTO userDTO { get { return _userDTO; } }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public User(String email, String password)
        {
            VerifyEmail(email);
            _email = email;
            this.password = new Password(password);
            _userDTO = new UserDTO(email, password);
            userDTO.Insert();
//            InsertToDB();
            log.Debug($"user {email} created succesfully");
        }
        public User(UserDTO userDTO)
        {
            _email = userDTO.Email;
            this.password = new Password(userDTO.Pass);
            _userDTO = userDTO;
            log.Debug($"user {email} created succesfully");
        }

        /// <summary>
        /// Return if the password are identical
        /// </summary>
        /// <param name="pass">password to check</param>
        /// <returns>if the passwords equals</returns>
        public bool VerifyPasswords(String pass)
        {
            if (this.password.Equals(pass))
                return true;

            throw new Exception("password not match to the user password");
            
        }

        /// <summary>
        /// check if the email is legal
        /// </summary>
        /// <param name="email">password to check</param>
        public void VerifyEmail(String email)
        {
            if (email == null)
                throw new Exception("email can not be null");
            if (!Regex.IsMatch(email, @"\A(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z"))
                throw new Exception("email is illegal");
            if (Regex.IsMatch(email, @"[א,ב,ג,ד,ה,ו,ז,ח,ט,י,כ,ל,מ,נ,ס,ע,פ,צ,ק,ר,ש,ת]"))
                throw new Exception("email is illegal");
            log.Debug("email is legal");
        }

    }
}
