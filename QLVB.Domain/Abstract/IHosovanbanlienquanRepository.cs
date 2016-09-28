using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IHosovanbanlienquanRepository
    {
        IQueryable<Hosovanbanlienquan> Hosovanbanlienquans { get; }

        int Them(Hosovanbanlienquan hoso);

        void Sua(int intid, Hosovanbanlienquan hoso);

        void Xoa(int intid);
    }
}