using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IChitietVanbandenRepository
    {
        IQueryable<ChitietVanbanden> Chitietvanbandens { get; }

        int Them(ChitietVanbanden vb);

        void Sua(int intid, ChitietVanbanden ct);

        void Xoa(int intid);
    }
}