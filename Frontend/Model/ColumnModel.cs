using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Frontend.Model
{
    public class ColumnModel : NotifiableModelObject
    { 

        private int _ordinal;
        private int _limitation;
        private string _columnName;
        private string search;

        public ObservableCollection<TaskModel> Tasks { get; set; }

        public string ColumnName
        {
            get => _columnName;
            set
            {
                _columnName = value;
                RaisePropertyChanged("ColumnName");
            }
        }


        public int Ordinal
        {
            get => _ordinal;
            set
            {
                _ordinal= value;
            }
        }

        internal bool IsFull()
        {
            if (Limitation == Tasks.Count())
                return true;
            return false;
        }

        public int Limitation
        {
            get => _limitation;
            set
            {
                _limitation = value;
            }
        }

        public ColumnModel(BackendController backendController, String columnName, int ordinallity) : base(backendController)
        {
            ColumnName = columnName;
            Ordinal = ordinallity;
            Tasks = new ObservableCollection<TaskModel>();
            Tasks.CollectionChanged += HandleChange;
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="list">list of tasks model. </param>
        public void AddTasks(IList<TaskModel> list)
        {
            //int numOfColumns = controller.GetNumberOfColumns(user.Email, BoardName, EmailCreator);
            foreach (TaskModel task in list)
            {
                Tasks.Add(task);
            }

        }
        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="list">list of tasks model. </param>
        /// <param name="user">a user model</param>
        public void AddTasks(IList<TaskModel> list,UserModel user)
        {
            Tasks = new ObservableCollection<TaskModel>();
            Tasks.CollectionChanged += HandleChange;
            foreach (TaskModel task in list)
            {
                Tasks.Add(task);
            }


        }
        /// <summary>
        /// Add a new task.
        /// </summary>
        /// <param name="taskModel">a task model. </param>

        public void AddTask(TaskModel taskModel)
        {
            Tasks.Add(taskModel);
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
/*                for (int i = e.OldStartingIndex; i < Columns.Count; i++)
                {
                    Columns[i].Ordinal = i;
                }*/

            }

        }
        /// <summary>
        /// search in some column.
        /// </summary>
        /// <param name="searchFilter">a filter </param>
        internal void SearchFilter(string searchfilter)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Tasks);
            view.Filter = o =>
            {
                TaskModel t = o as TaskModel;
                return t.Title.ToLower().Contains(searchfilter.ToLower())
                       || t.Description.ToLower().Contains(searchfilter.ToLower());
            };

        }


        /// <summary>
        /// sort a column.
        /// </summary>
        /// <param name="sortKey">a key to sort</param>
        internal void Sort(string sortKey)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Tasks);
            view.SortDescriptions.Add
            (
                new SortDescription(sortKey, ListSortDirection.Descending)
            );
        }
    }
}
