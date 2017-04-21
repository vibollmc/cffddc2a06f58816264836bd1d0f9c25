using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Abstract
{
    public interface IStoreVanbandiCanboRepository
    {
        IQueryable<VanbandiCanbo> VanbandiCanbos { get; }

        void Them(int idvanban, int idcanbo);

        void Xoa(int idvanban, int idcanbo);
    }
}
