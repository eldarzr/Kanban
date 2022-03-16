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
using Frontend.ViewModel;
using Frontend.Model;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        TaskWindowViewModel viewModel;
        public TaskWindow(TaskWindowViewModel taskWindowViewModel)
        {
            InitializeComponent();
            this.DataContext = taskWindowViewModel;
            this.viewModel = (TaskWindowViewModel)DataContext;
        }
        private void UpdateTitle(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateTaskTitle();
        }

        private void UpdateDescription(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateTaskDescription();
        }

        private void UpdateDuedate(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateTaskDuedate();
        }

        private void UpdateAssigne(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateTaskAssigne();
        }

        private void AddTask(object sender, RoutedEventArgs e)
        {
            viewModel.AddTask();
            BackWindow();
/*            BoardModel boardModel = viewModel.;
            if (boardModel != null)
            {
                BoardView boardView = new BoardView(userModel, boardModel);
                boardView.Show();
                this.Close();
}*/
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            BackWindow();
        }

        private void BackWindow()
        {
            BoardViewModel boardViewModel = viewModel.Back();
            if (boardViewModel != null)
            {
         
                BoardView boardView = new BoardView(boardViewModel);
                boardView.Show();
                this.Close();
            }
        }
    }
}
