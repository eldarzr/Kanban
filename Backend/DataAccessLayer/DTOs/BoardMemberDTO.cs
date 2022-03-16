using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class BoardMemberDTO : DTO
{
    public const string MemberColumnName = "Member";

    private string _member;
    public string Member { get => _member; set { _member = value; _controller.Update(Id, MemberColumnName, value); } }
    public BoardMemberDTO(int ID, string member) : base(new BoardMembersDalController())
    {
        Id = ID;
        _member = member;
    }

    public override bool Insert()
    {
        return ((BoardMembersDalController)_controller).Insert(this);
    }

}
