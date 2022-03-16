using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {

        private string _emailCreator;
        private string _boardName;
        private UserModel user;

        private Visibility isBoardVisible = Visibility.Visible;
        public Visibility IsBoardVisible
        {
            get => isBoardVisible;
            set
            {
                this.isBoardVisible = value;
                RaisePropertyChanged("IsBoardVisible");
            }
        }

        public ObservableCollection<ColumnModel> Columns { get; set; }



        private ColumnModel _selectedColumn;
        public ColumnModel SelectedColumn
        {
            get
            {
                return _selectedColumn;
            }
            set
            {
                _selectedColumn = value;
                if (value != null)
                    EnableAddTask = !value.IsFull();
                //_selectedColumn.AddTasks(Controller, user, EmailCreator, BoardName);
                RaisePropertyChanged("SelectedColumn");
            }
        }

        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                _selectedTask = value;
                EnableTask();
                RaisePropertyChanged("SelectedTask");
            }
        }


        public string EmailCreator
        {
            get => _emailCreator;
            set
            {
                _emailCreator = value;
            }
        }

        public string BoardName
        {
            get => _boardName;
            set
            {
                _boardName = value;
            }
        }


        private bool _enableAssignee = false;
        public bool EnableAssignee
        {
            get => _enableAssignee;
            set
            {
                _enableAssignee = value;
                RaisePropertyChanged("EnableAssignee");
            }
        }

        private bool _enableAddTask =false;
        public bool EnableAddTask
        {
            get => _enableAddTask;
            set
            {
                _enableAddTask = value;
                RaisePropertyChanged("EnableAddTask");
            }
        }

        private bool _enableEditTask = false;
        public bool EnableEdit
        {
            get => _enableEditTask;
            set
            {
                _enableEditTask = value;
                RaisePropertyChanged("EnableEdit");
            }
        }


        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                for (int i = 0; i < Columns.Count; i++)
                {
                    Columns[i].Ordinal = i;
                    Columns[i].AddTasks(Controller.GetColumn(user, EmailCreator, BoardName, i), user);
                }
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                for (int i = 0; i < Columns.Count; i++)
                {
                    Columns[i].Ordinal = i;
                }

            }
            if (e.Action == NotifyCollectionChangedAction.Add){}

        }



        public BoardModel(BackendController backendController, UserModel user, string emailCreator, string boardName) : base(backendController)
        {
            this.user = user;
            EmailCreator = emailCreator;
            BoardName = boardName;
            Columns = new ObservableCollection<ColumnModel>();
            AddColumns(backendController,user);
            SelectedColumn = Columns[0];
        }

        public BoardModel(BackendController backendController, UserModel user, string boardName) : base(backendController)
        {
            this.user = user;
            EmailCreator =user.Email;
            BoardName = boardName;
            Columns = new ObservableCollection<ColumnModel>();
            ColumnModel column = new ColumnModel(Controller, "", 0);
            IsBoardVisible = Visibility.Collapsed;
            Columns.Add(column);
            SelectedColumn = Columns[0];
            SelectedTask = null;
        }


        private void AddColumns(BackendController controller, UserModel user)
        {
            Columns = new ObservableCollection<ColumnModel>();
            Columns.CollectionChanged += HandleChange;
            
            int numOfColumns = controller.GetNumberOfColumns(user.Email, BoardName, EmailCreator);
            for (int i = 0; i < numOfColumns; i++)
            {
                string columnName = controller.GetColumnName(user.Email, BoardName, EmailCreator, i);
                //Columns.Add(new ColumnModel(controller, columnName, i));
                ColumnModel columnModel = new ColumnModel(controller, columnName, i);
                columnModel.AddTasks(controller.GetColumn(user, EmailCreator, BoardName, i),user);
                Columns.Add(columnModel);
            }

        }
        /// <summary>
        /// Adds a new column
        /// </summary>
        /// <param name="addColumnOrdinal">The location of the new column. Location for old columns with index>=columnOrdinal is increased by 1 (moved right). The first column is identified by 0, the location increases by 1 for each column.</param>
        /// <param name="addColumnName">The name for the new columns</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        internal void AddColumn(string addColumnName, int addColumnOrdinal)
        {
            Controller.AddColumn(user.Email, EmailCreator, BoardName, addColumnName, addColumnOrdinal);
            ColumnModel newColumn = new ColumnModel(Controller, addColumnName, addColumnOrdinal);
            Columns.Insert(addColumnOrdinal, newColumn);
            //Columns.Add(newColumn);
        }

        /// <summary>
        /// Removes a specific column
        /// </summary>
       
        internal void RemoveColumn()
        {
            Controller.RemoveColumn(user.Email, EmailCreator, BoardName, SelectedColumn.Ordinal);
            Columns.Remove(SelectedColumn);
            Columns.CollectionChanged += HandleChange;

        }
        /// <summary>
        /// assign task to a user
        /// </summary>
        internal void AssignTask()
        {
            Controller.AssignTask(user.Email, EmailCreator, BoardName, SelectedColumn.Ordinal, SelectedTask.Id, user.Email);
            SelectedTask.Assignee = user.Email;
            EnableTask();
        }
        /// <summary>
        /// add task to a column
        /// </summary>
        internal void AddTask(TaskModel taskModel)
        {
            Columns[0].AddTask(taskModel);
        }
        /// <summary>
        /// move task
        /// </summary>
        internal void AdvanceTask()
        {
            Controller.AdvanceTask(user.Email, EmailCreator, BoardName, SelectedColumn.Ordinal, SelectedTask.Id);
            TaskModel task = SelectedTask;
            SelectedColumn.Tasks.Remove(task);
            Columns[_selectedColumn.Ordinal + 1].Tasks.Add(task);
        }
        /// <summary>
        /// move column .
        /// </summary>
        /// <param name="newOrdinallity">The  new column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        internal void ReOrderColumn(int newOrdinallity)
        {
            if (!(SelectedColumn.Ordinal == newOrdinallity))
            {
                Controller.MoveColumn(user.Email, EmailCreator, BoardName, SelectedColumn.Ordinal, newOrdinallity - SelectedColumn.Ordinal);
                Columns.Move(SelectedColumn.Ordinal, newOrdinallity);
                //  SelectedColumn.Ordinal = newOrdinallity;
            }
            
        }
        /// <summary>
        /// change the size of the column.
        /// </summary>
        /// <param name="newLimitation">the size of column</param>
        
        internal void ChangeLimitation(int newLimitation)
        {
            if (SelectedColumn != null)
            {
                Controller.ChangeLimit(user.Email, EmailCreator, BoardName, SelectedColumn.Ordinal, newLimitation);
                SelectedColumn.Limitation = newLimitation;
            }
        }
        /// <summary>
        /// change the size of the column.
        /// </summary>
        /// <param name="newColumnName">the new name</param>
        internal void ChangeColumnName(string newColumnName)
        {
            if (SelectedColumn != null)
            {
                Controller.ChangeColumnName(user.Email, EmailCreator, BoardName, SelectedColumn.Ordinal, newColumnName);
                SelectedColumn.ColumnName= newColumnName;
                //Columns.CollectionChanged += HandleChange;
            }
        }
        /// <summary>
        /// search in some column.
        /// </summary>
        /// <param name="searchFilter">a filter </param>
        internal void Search(string searchFilter)
        {
            if(SelectedColumn != null)
                SelectedColumn.SearchFilter(searchFilter);
        }
        /// <summary>
        /// sort a column.
        /// </summary>
        /// <param name="sortKey">a key to sort</param>
        internal void Sort(string sortKey)
        {
            if (SelectedColumn != null)
                SelectedColumn.Sort(sortKey);
        }


        private void EnableTask()
        {
            if (SelectedTask != null)
            {
                EnableAssignee = true;
                EnableEdit = SelectedTask.Assignee.Equals(user.Email);
            }
        }
    }
}
