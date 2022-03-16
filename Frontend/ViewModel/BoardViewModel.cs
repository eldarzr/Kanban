using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Frontend.Model;

namespace Frontend.ViewModel
{
    public class BoardViewModel : NotifiableObject
    {
        private UserModel user;
        private BoardModel _board;
        private string addColumnName;
        private string newColumnName;
        private string addColumnOrdinal;
        private string newOrdinallity;
        private string newLimitation;
        private SolidColorBrush _bgcolor;
        private SolidColorBrush _bordercolor;

        public UserModel UserModel { get => user; }

        public BackendController Controller { get; private set; }

        public string Title { get; private set; }

        public BoardModel Board
        {
            get
            {
                return _board;
            }
            set
            {
                _board = value;
            }
        }

        public SolidColorBrush BackgroundColor
        {
            get
            {
                if (Board.SelectedColumn.Tasks != null && Board.SelectedColumn.Equals(user.Email))
                    _bgcolor = new SolidColorBrush(Colors.Blue);
                else _bgcolor = new SolidColorBrush(Colors.Khaki);
                //   return new SolidColorBrush(user.Email.Equals(Board.) ? Colors.Blue : Colors.Khaki);
                  return _bgcolor;
             //  return new SolidColorBrush(Colors.White);
             //   return new SolidColorBrush(user.Email.Equals(Board.SelectedTask.Assignee) ? Colors.Blue : Colors.Khaki);
            }
            set {
                _bgcolor= value;
            }
        }


        public SolidColorBrush BorderColor
        {
            get
            {

                if(Board.SelectedTask!=null && Board.SelectedTask.Assignee.Equals(user.Email))
                  _bordercolor= new SolidColorBrush(Colors.Blue);
                else _bordercolor = new SolidColorBrush(Colors.Red);

                //   return _bgcolor
                return _bordercolor;
            }
            set
            {
                _bordercolor = value; 
            }
        }

        public string AddColumnName
        {
            get => addColumnName;
  
            set
            {
                this.addColumnName = value;
            }
        }

        public string NewColumnName
        {
            get => newColumnName;

            set
            {
                this.newColumnName = value;
            }
        }


        public string AddColumnOrdinal
        {
            get => addColumnOrdinal;
            set
            {
                //addColumnOrdinal = int.Parse(value.ToString());
                this.addColumnOrdinal = value;
            }
        }

        public string NewOrdinallity
        {
            get => newOrdinallity;
            set
            {
                this.newOrdinallity = value;
            }
        }

        public string NewLimitation
        {
            get => newLimitation;
            set
            {
                this.newLimitation = value;
            }
        }

        private string searchFilter = "";

        public string SearchFilter
        {
            get => searchFilter;
            set
            {
                this.searchFilter = value;
                RaisePropertyChanged("SearchFilter");
            }
        }

        public BoardViewModel(UserModel user, BoardModel boardModel)
        {
            this.user = user;
            Board = boardModel;
            //Board.Columns[0]
            this.Controller = user.Controller;

            //Title = user.Email+ "is Logged in to the Board named :";

        }

        public void addColumn()
        {
            try
            {
              Board.AddColumn(AddColumnName, int.Parse(AddColumnOrdinal));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        internal TaskWindowViewModel EditTask()
        {
            try
            {
                TaskWindowViewModel taskviewModel = new TaskWindowViewModel(user, Board, Board.SelectedColumn, Board.SelectedTask);
                return taskviewModel;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        internal TaskWindowViewModel AddTask()
        {
            try
            {
                TaskModel task = new TaskModel(Controller, user);
                TaskWindowViewModel taskviewModel = new TaskWindowViewModel(user, Board, Board.SelectedColumn, task);
                


                return taskviewModel;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;
        }

        internal void ChangeColumnName()
        {
            try
            {
                Board.ChangeColumnName(NewColumnName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        internal void ChangeLimitation()
        {
            try
            {
                Board.ChangeLimitation(int.Parse(NewLimitation));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        internal void ReOrderColumn()
        {
            try
            {
                Board.ReOrderColumn(int.Parse(NewOrdinallity));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void RemoveColumn()
        {
            try
            {
                Board.RemoveColumn();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void AssignTask()
        {
            try
            {
                Board.AssignTask();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void AdvanceTask()
        {
            try
            {
                Board.AdvanceTask();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        internal void Search()
        {
            Board.Search(SearchFilter);
        }


        internal void DueDateSort()
        {
            Board.Sort("DueDate");
        }

        public void Logout()
        {
            try
            {
                Controller.Logout(user.Email);
                //boardControllerModel.Boards.Add(new BoardModel(Controller, user, user.Email, AddBoardName, true));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    } 
}
