using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Frontend.Model;
using Frontend.ViewModel;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for BoardsStystemView.xaml
    /// </summary>
    public partial class BoardsStystemView : Window
    {

        UserModel userModel;
        BoardsSystemViewModel viewModel;

        public BoardsStystemView(UserModel userModel)
        {
            this.userModel = userModel;
            InitializeComponent();
            this.DataContext = new BoardsSystemViewModel(userModel);
            this.viewModel = (BoardsSystemViewModel)DataContext;
        }

        private void OpenBoard(object sender, RoutedEventArgs e)
        {
            BoardModel boardModel = viewModel.SelectedBoard;
            if (boardModel != null)
            {
                BoardView boardView = new BoardView(userModel, boardModel);
                boardView.Show();
                this.Close();
            }
        }

        private void RemoveBoard(object sender, RoutedEventArgs e)
        {
             viewModel.RemoveBoard();
        }

        private void AddBoard(object sender, RoutedEventArgs e)
        {
            viewModel.AddBoard();
        }

        private void JoinBoard(object sender, RoutedEventArgs e)
        {
            viewModel.JoinBoard();
        }

        private void Logout(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void InProgress(object sender, RoutedEventArgs e)
        {
            //IList<TaskModel> list = viewModel.GetAllInProgress();
                BoardModel boardModel = viewModel.GetAllInProgress();
                BoardView boardView = new BoardView(userModel, boardModel);
                boardView.Show();
                this.Close();
            }
        }
    }

