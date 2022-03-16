using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    class UserDTO : DTO
    {

        public const string UserEmailColumnName = "Email";
        public const string UserPasswordColumnName = "Password";



        private string _email;
        public string Email { get => _email; set { _email = value; _controller.Update(Id, UserEmailColumnName, value); } }

        private string _pass;
        public string Pass { get => _pass; set { _pass = value; _controller.Update(Id, UserPasswordColumnName, value); } }

        public UserDTO(string email, string pass) : base(new UserDalController())
        {
            _email = email;
            _pass = pass;
        }

        public override bool Insert()
        {
            return ((UserDalController)_controller).Insert(this);
        }



    }
}
