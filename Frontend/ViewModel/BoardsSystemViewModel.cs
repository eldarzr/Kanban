using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Frontend.Model;

namespace Frontend.ViewModel
{
    class BoardsSystemViewModel : NotifiableObject
    {
        public BackendController Controller { get; private set; }

        private BoardControllerModel _boardControllerModel;
        private UserModel user;
        public string Title { get; private set; }

        private string addBoardName;
        public string AddBoardName
        {
            get => addBoardName;
            set
            {
                this.addBoardName = value;
            }
        }
        private string joinBoardName;
        public string JoinBoardName
        {
            get => joinBoardName;
            set
            {
                this.joinBoardName = value;
            }
        }
        private string joinEmailCreator;
        public string JoinEmailCreator
        {
            get => joinEmailCreator;
            set
            {
                this.joinEmailCreator = value;
            }
        }

        public BoardControllerModel boardControllerModel
        {
            get => _boardControllerModel;
            set
            {
                this._boardControllerModel = value;
                RaisePropertyChanged("boardControllerModel");
            }
        }

        private BoardModel _selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;

                EnableOpen = value != null;
                if (value != null)
                    EnableRemove = value.EmailCreator.Equals(user.Email);
                else EnableRemove = false;
                RaisePropertyChanged("SelectedBoard");
            }
        }

        private bool _enableOpen = false;
        public bool EnableOpen
        {
            get => _enableOpen;
            set
            {
                _enableOpen = value;
                RaisePropertyChanged("EnableOpen");
            }
        }



        private bool _enableRemove = false;
        public bool EnableRemove
        {
            get => _enableRemove;
            set
            {
                _enableRemove = value;
                RaisePropertyChanged("EnableRemove");
            }
        }

        public BoardsSystemViewModel(UserModel user)
        {
            this.Controller = user.Controller;
            this.user = user;
            Title = "ALL BOARDS";
            boardControllerModel = new BoardControllerModel(Controller, user);
        }


        public void AddBoard()
        {
            try
            {
                boardControllerModel.AddBoard(user.Email, AddBoardName);
                //boardControllerModel.Boards.Add(new BoardModel(Controller, user, user.Email, AddBoardName, true));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void JoinBoard()
        {
            try
            {
                boardControllerModel.JoinBoard(JoinEmailCreator, joinBoardName);
                //boardControllerModel.Boards.Add(new BoardModel(Controller, user, user.Email, AddBoardName, true));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        
        internal BoardModel GetAllInProgress()
        {

            try
            {
                IList<TaskModel> list = boardControllerModel.GetAllInProgress(user);
                BoardModel boardModel = new BoardModel(Controller, user, "My In Progress");
                foreach (TaskModel task in list)
                {
                    boardModel.AddTask(task);
                }
                return boardModel;

                //boardControllerModel.Boards.Add(new BoardModel(Controller, user, user.Email, AddBoardName, true));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return null;

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

        public void RemoveBoard()
        {
            try
            {
                boardControllerModel.RemoveBoard(SelectedBoard);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

    }
}
