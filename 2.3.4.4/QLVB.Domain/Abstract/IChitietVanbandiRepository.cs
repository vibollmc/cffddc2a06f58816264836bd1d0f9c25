using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IChitietVanbandiRepository
    {
        IQueryable<ChitietVanbandi> ChitietVanbandis { get; }

        int Them(ChitietVanbandi ct);

        void Sua(int intid, ChitietVanbandi ct);

        void Xoa(int intid);
    }
}
