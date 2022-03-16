using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Frontend;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class TaskWindowViewModel : NotifiableObject
    {
        private TaskModel task;
        public BackendController Controller { get; private set; }
        public TaskModel Task
        {
            get
            {
                return task;
            }
            set
            {
                task = value;
            }
        }


        private int id;
        public int Id
        {
            get => id;
            set
            {
                this.id = value;
            }
        }
        private string title;
        public string Title
        {
            get => title;
            set
            {
                this.title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string description;
        public string Description
        {
            get => description;
            set
            {
                this.description = value;
                RaisePropertyChanged("Description");
            }
        }
        private DateTime dueDate;
        public DateTime DueDate
        {
            get => dueDate;
            set
            {
                this.dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        private string assigne;
        public string Assignee
        {
            get => assigne;
            set
            {
                this.assigne = value;
                RaisePropertyChanged("Assignee");
            }
        }
        private DateTime creationTime;
        public DateTime CreationTime
        {
            get => creationTime;
            set
            {
                this.creationTime = value;
                RaisePropertyChanged("CreationTime");
            }

        }
        private string userEmail;
        private string creatorEmail;
        private string boardName;
        private int columnOrdinal;

        private Visibility _visibilityEdit = Visibility.Hidden;
        public Visibility VisibilityEdit
        {
            get => _visibilityEdit;
            set
            {
                _visibilityEdit = value;
                RaisePropertyChanged("VisibilityEdit");
            }
        }

        private Visibility _visibilityAdd = Visibility.Hidden;
        public Visibility VisibilityAdd
        {
            get => _visibilityAdd;
            set
            {
                _visibilityAdd = value;
                RaisePropertyChanged("VisibilityAdd");
            }
        }


        private BoardModel boardModel;
        private UserModel userModel;
        private ColumnModel columnModel;

        //UserEmail, CreatorEmail, BoardName, ColumnOrdinal

        public TaskWindowViewModel(UserModel user, BoardModel boardModel,ColumnModel columnModel, TaskModel taskModel)
        {
            Controller = boardModel.Controller;
            Task = taskModel;
            CreationTime = task.CreationTime;
            Assignee = task.Assignee;
            DueDate = task.DueDate;
            Description = task.Description;
            Title = task.Title;
            Id = task.Id;
            columnOrdinal = columnModel.Ordinal;
            userModel = user;
            this.boardModel = boardModel;
            this.columnModel = columnModel;
            userEmail = user.Email;
            creatorEmail = boardModel.EmailCreator;
            boardName = boardModel.BoardName;
            //columnOrdinal = columnModel.Ordinal;
            if (task.Title.Equals(""))
                VisibilityAdd = Visibility.Visible;
            else VisibilityEdit = Visibility.Visible;
        }

        public void UpdateTaskTitle()
        {
            try
            {
                Controller.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, task.Id, Title);
                Task.Title = Title;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void UpdateTaskDescription()
        {
            try
            {
                Controller.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, Task.Id, Description);
                Task.Description = Description;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void UpdateTaskDuedate()
        {
            try
            {
                Controller.UpdateTaskDuedate(userEmail, creatorEmail, boardName, columnOrdinal, Task.Id, DueDate);
                Task.DueDate = DueDate;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void UpdateTaskAssigne()
        {
            try
            {
                Controller.UpdateTaskAssigne(userEmail, creatorEmail, boardName, columnOrdinal, Task.Id, Assignee);
                Task.Assignee = Assignee;


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void AddTask()
        {
            try
            {
            //  TaskModel taskModela = new TaskModel(Controller, CreationTime, Title, Description, DueDate, Assignee, -1);
                TaskModel taskModel = Controller.AddTask(userModel, creatorEmail, boardName, Title, Description, DueDate);
                boardModel.AddTask(taskModel);
                Back();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public BoardViewModel Back()
        {
            return new BoardViewModel(userModel, boardModel);
        }

    }
}
