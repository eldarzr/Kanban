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
    /// Interaction logic for BoardView.xaml
    /// </summary>
    public partial class BoardView : Window
    {
        BoardViewModel viewModel;
        private UserModel userModel;

        public BoardView(UserModel user, BoardModel boardModel)
        {
            InitializeComponent();
            userModel = user;
            this.viewModel = new BoardViewModel(user, boardModel);
            this.DataContext = viewModel;
        }

        public BoardView(BoardViewModel boardViewModel)
        {
            InitializeComponent();
            userModel = boardViewModel.UserModel;
            this.viewModel = boardViewModel;
            this.DataContext = viewModel;
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            TaskWindowViewModel taskViewModel = viewModel.AddTask();
            if (taskViewModel != null)
            {
                TaskWindow taskWindow = new TaskWindow(taskViewModel);
                taskWindow.Show();
                this.Close();
            }
        }

        private void AddColumn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.addColumn();
        }

        private void RemoveColumn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveColumn();
        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AssignTask();
        }

        private void AdvanceTask_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AdvanceTask();
        }

        private void ReOrderColumn(object sender, RoutedEventArgs e)
        {
            viewModel.ReOrderColumn();
        }

        private void EditTask(object sender, RoutedEventArgs e)
        {
            TaskWindowViewModel taskViewModel = viewModel.EditTask();
            if (taskViewModel != null)
            {
                TaskWindow taskWindow = new TaskWindow(taskViewModel);
                taskWindow.Show();
                this.Close();
            }
        }

        private void FilterChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void ChangeLimitation(object sender, RoutedEventArgs e)
        {
            viewModel.ChangeLimitation();

        }

        private void ChangeColumnName(object sender, RoutedEventArgs e)
        {
            viewModel.ChangeColumnName();
        }

        private void SearchFilter(object sender, RoutedEventArgs e)
        {
            viewModel.Search();

        }

        private void DueDateSort(object sender, RoutedEventArgs e)
        {
            viewModel.DueDateSort();
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
            }

        private void Back(object sender, RoutedEventArgs e)
        {
            BoardsStystemView boardsStystemView = new BoardsStystemView(userModel);
            boardsStystemView.Show();
            this.Close();

        }
    }

}
