using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    class BoardControllerModel: NotifiableModelObject
    {

        private readonly UserModel user;
        public ObservableCollection<BoardModel> Boards { get; set; }

        private BoardControllerModel(BackendController backendController, ObservableCollection<BoardModel> boards) : base(backendController)
        {
            Boards = boards;
            Boards.CollectionChanged += HandleChange;
        }

        public BoardControllerModel(BackendController controller, UserModel user) : base(controller)
        {
            this.user = user;
            Boards = new ObservableCollection<BoardModel>();
            foreach ((String, String) boardTuple in controller.GetAllUserBoards(user.Email))
                Boards.Add(new BoardModel(controller, user, boardTuple.Item1, boardTuple.Item2));
            Boards.CollectionChanged += HandleChange;
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            //read more here: https://stackoverflow.com/questions/4279185/what-is-the-use-of-observablecollection-in-net/4279274#4279274
            /*
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (BoardModel board in e.OldItems)
                {
                    Controller.RemoveBoard(user.Email, board.EmailCreator, board.BoardName);
                }

            }
                        if (e.Action == NotifyCollectionChangedAction.Add)
                        {
                            foreach (BoardModel board in e.NewItems)
                            {
                                Controller.AddBoard(board.EmailCreator, board.BoardName);
                            }

                        }*/
        }
        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="emailCreator">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the new board</param>

        public void AddBoard(String emailCreator, String boardName)
        {
            Controller.AddBoard(emailCreator, boardName);
            Boards.Add(new BoardModel(Controller, user, emailCreator, boardName));
        }
        /// <summary>
        /// Join to an exist board.
        /// </summary>

        /// <param name="emailCreator">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board to join</param>
        public void JoinBoard(String emailCreator, String boardName)
        {
            Controller.JoinBoard(user.Email, emailCreator, boardName);
            Boards.Add(new BoardModel(Controller, user, emailCreator, boardName));
        }
        /// <summary>
        /// remove a board.
        /// </summary>
        /// <param name="boardModel">a board model</param>

        public void RemoveBoard(BoardModel boardModel)
        {
            Controller.RemoveBoard(user.Email, boardModel.EmailCreator, boardModel.BoardName);
            Boards.Remove(boardModel);
        }
        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="user"> the user. Must be logged in</param>
        /// <returns>returns list of all tasks that in progress by a specific user.
        internal IList<TaskModel> GetAllInProgress(UserModel user)
        {

                return Controller.getAllInProgress(user);
        }
    }
}
