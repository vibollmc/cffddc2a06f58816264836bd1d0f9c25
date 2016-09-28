using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IVanbandenmailRepository
    {
        IQueryable<Vanbandenmail> Vanbandenmails { get; }

        int Them(Vanbandenmail vb);

        void Xoa(int intid);
        void UpdateTrangthai(int intid);
    }
}