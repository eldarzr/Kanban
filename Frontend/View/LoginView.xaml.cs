using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Frontend.ViewModel;
using Frontend.Model;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        private LoginViewModel viewModel;

        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
            this.viewModel = (LoginViewModel)DataContext;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            UserModel userModel = viewModel.Login();
            if (userModel != null)
            {
                BoardsStystemView boardsStystemView = new BoardsStystemView(userModel);
                boardsStystemView.Show();
                this.Close();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Register();
        }
    }
}
