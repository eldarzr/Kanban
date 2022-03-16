using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
[assembly: InternalsVisibleTo("TestProject")]

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class Password
    {
        private string pass;
        private readonly int MIN_PASS_LENGTH = 4;
        private readonly int MAX_PASS_LENGTH = 20;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
         
        public Password(string pass)
        {
            if (pass == null)
                throw new Exception("password can not be null");
            MinLengthValid(pass);
            MaxLengthValid(pass);
            UpperCaseLetterValid(pass);
            LowerCaseLetterValid(pass);
            NumberValid(pass);
            PasswordsValid(pass);
            this.pass = pass;
            log.Debug($"password {pass} created succesfully");

        }

        private void PasswordsValid(string pass)
        {
            
            List<String> illegalPasswords = new List<string> { "123456", "123456789", "1234567890", "qwerty", "password", "password1", "1111111", "12345678", "abc123", "1234567", "12345", "20", "123123", "000000", "iloveyou", "1234", "1q2w3e4r5t", "Qwertyuiop", "123", "Monkey", "Dragon" };

            if (illegalPasswords.Contains(pass))
                throw new Exception("password is illegal!");
        }

        /// <summary>
        /// check if the pass is in min length
        /// </summary>
        /// <param name="pass">password of the user to check</param>
        private void MinLengthValid(string pass)
        {
            if (pass.Length < MIN_PASS_LENGTH)
                throw new Exception("too short password!");
        }

        /// <summary>
        /// check if the pass is in max length
        /// </summary>
        /// <param name="pass">password of the user to check</param>
        private void MaxLengthValid(string pass)
        {
            if (pass.Length > MAX_PASS_LENGTH)
                throw new Exception("too long password!");
        }

        /// <summary>
        /// check if the pass has upercase letter
        /// </summary>
        /// <param name="pass">password of the user to check</param>
        private void UpperCaseLetterValid(string pass)
        {
            string pattern = @"[A-Z]";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(pass);
            if (matches.Count == 0)
                throw new Exception("password does'nt have upper letter");
        }

        /// <summary>
        /// check if the pass has lower case letter
        /// </summary>
        /// <param name="pass">password of the user to check</param>
        private void LowerCaseLetterValid(string pass)
        {
            string pattern = @"[a-z]";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(pass);
            if (matches.Count == 0)
                throw new Exception("password does'nt have lower letter");
        }

        /// <summary>
        /// check if the pass has number
        /// </summary>
        /// <param name="pass">password of the user to check</param>
        private void NumberValid(string pass)
        {
            string pattern = @"[0-9]";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(pass);
            if (matches.Count == 0)
                throw new Exception("password does'nt have number");
        }

        /// <summary>
        /// Return if the password are identical
        /// </summary>
        /// <param name="obj">password to check</param>
        /// <returns>if the passwords equals</returns>
        public override bool Equals(Object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("no password sent!");
            if (obj is String)
            {
                String otherPass = (String)obj;
                return this.pass.Equals(otherPass);
            }

            return false;

        }
    }
}