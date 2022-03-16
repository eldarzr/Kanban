using System;
using System.Collections.Generic;
using System.Linq;
//using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    internal abstract class DTO
    {
        public const string IDColumnName = "ID";
        protected DalController _controller;

        public int Id { get; set; } = -1;
        protected DTO(DalController controller)
        {
            _controller = controller;

        }

        /// <summary>
        /// Insert the DTO to the DB using DalController
        /// </summary>
        /// <returns>returns if the insertion succssed. </returns>
        public abstract bool Insert();

    }
}
