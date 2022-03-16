using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {

        private int _id;
        private string _title;
        private string _description;
        private string _assignee;
        private UserModel _user;
        private DateTime _creationTime;
        private DateTime _dueDate;


        public int Id
        {
            get => _id;
            set
            {
                _id = value;
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged("Title");
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        public string Assignee
        {
            get => _assignee;
            set
            {
                _assignee = value;
                updateColor();
                RaisePropertyChanged("Assignee");
            }
        }


        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        public DateTime CreationTime
        {
            get => _creationTime;
        }

        public SolidColorBrush _bordercolor = new SolidColorBrush(Colors.WhiteSmoke);
        public SolidColorBrush _bgColor = new SolidColorBrush(Colors.WhiteSmoke);

        public SolidColorBrush BgColor
        {
            get
            {

                return _bgColor;
            }
            set
            {
                _bgColor = value;
                RaisePropertyChanged("BgColor");

            }
        }

        public SolidColorBrush BorderColor
        {
            get
            {

             ColorByTimeElapsed();
                return _bordercolor;
            }
            set
            {
                _bordercolor = value;
                RaisePropertyChanged("BorderColor");

            }
        }

        private void ColorByTimeElapsed()
        {

            if (DateTime.Compare(DateTime.Now, DueDate) == 1)
                _bordercolor = new SolidColorBrush(Colors.Firebrick);
          
              else if (TimeSpan.FromTicks(DueDate.Date.Ticks).TotalSeconds-TimeSpan.FromTicks(DateTime.Now.Ticks).TotalSeconds<=0.75*(TimeSpan.FromTicks(DueDate.Date.Ticks).TotalSeconds - TimeSpan.FromTicks(CreationTime.Date.Ticks).TotalSeconds))
                _bordercolor = new SolidColorBrush(Colors.Orange);
           
            else
                _bordercolor = new SolidColorBrush(Colors.WhiteSmoke);

        }

        private void updateColor()
        {
            if (Assignee.Equals(_user.Email))
                BgColor = new SolidColorBrush(Colors.CornflowerBlue);
            else
                BgColor = new SolidColorBrush(Colors.WhiteSmoke);

        }


        public TaskModel(BackendController backendController, DateTime creationTime, String title, String description, DateTime dueDate, String assignee, int id, UserModel user) : base(backendController)
        {
            _user = user;
            _id = id;
            _title = title;
           _description = description;
            Assignee = assignee;
            _dueDate = dueDate;
            _creationTime = creationTime;
        }

        public TaskModel(BackendController controller, UserModel userModel) : base(controller)
        {
            _id = 0;
            _title = "";
            _description = "";
            _user = userModel;
            Assignee = userModel.Email;
            _dueDate = DateTime.Now;
            _creationTime = DateTime.Now;
        }
    }
}

