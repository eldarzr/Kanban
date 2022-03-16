using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Frontend.Model;

namespace Frontend.ViewModel
{
    class LoginViewModel: NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                this.email = value;
            }
        }
        private string password;
        public string Password
        {
            get => password;
            set
            {
                this.password = value;
            }
        }
        private string message;
        public string Message
        {
            get => message;
            set
            {
                this.message = value;
                RaisePropertyChanged("Message");
            }
        }

        public LoginViewModel()
        {
            Controller = new BackendController();
            Email = "";
            Password = "";
            Message = "";
        }

        public void Register()
        {
            try
            {
                Controller.Register(Email, Password);
                Message = "Registered successfully";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public UserModel Login()
        {
            try
            {
                return Controller.Login(Email, Password);
                //return Controller.Login("eldar@gmail.com", "Eldar123");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }


    }
}
