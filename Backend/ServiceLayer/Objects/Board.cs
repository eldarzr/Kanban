using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Board
    {
        public readonly int Id;
        public readonly string Name;
        public readonly string CreatorEmail;
        public readonly List<String> BoardMembers;

        internal Board(int id, string name, string creatorEmail, List<String> boardMembers)
        {
            this.Id = id;
            this.Name = name;
            this.CreatorEmail = creatorEmail;
            this.BoardMembers = boardMembers;
        }
    }
}
