using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Column
    {
        public readonly string Name;
        public readonly Dictionary<int, Task> Tasks;
        public readonly int Limitition;
        internal Column(string name, Dictionary<int, Task> tasks, int limitition)
        {
            this.Name = name;
            this.Tasks = tasks;
            this.Limitition = limitition;
        }
    }
}
