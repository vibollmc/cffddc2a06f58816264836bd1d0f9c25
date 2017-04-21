using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IVanbandiCanboRepository
    {
        IQueryable<VanbandiCanbo> VanbandiCanbos { get; }

        void Them(int idvanban, int idcanbo);

        void Xoa(int idvanban, int idcanbo);
    }
}
