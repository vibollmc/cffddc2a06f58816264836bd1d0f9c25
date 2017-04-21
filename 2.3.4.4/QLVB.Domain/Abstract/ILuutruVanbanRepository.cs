using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface ILuutruVanbanRepository
    {
        IQueryable<LuutruVanban> LuutruVanbans { get; }

        int Them(LuutruVanban vb);

        void Sua(int intid, LuutruVanban vb);
    }
}
