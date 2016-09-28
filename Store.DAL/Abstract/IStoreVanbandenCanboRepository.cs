using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Abstract
{
    public interface IStoreVanbandenCanboRepository
    {
        IQueryable<VanbandenCanbo> VanbandenCanbos { get; }

        void Them(int idvanban, int idcanbo);

        void Xoa(int idvanban, int idcanbo);
    }
}
