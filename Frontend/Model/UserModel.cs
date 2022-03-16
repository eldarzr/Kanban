using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject
    {

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
            }
        }

        public UserModel(BackendController backendController, string email) : base(backendController)
        {
            Email = email;
        }

    }
}
