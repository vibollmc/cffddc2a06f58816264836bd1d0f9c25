using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    // HIEN KHONG SU DUNG
    // TABLE SOVANBAN CO THEM TRUONG KHOI PHAT HANH
    public interface ISovbKhoiphRepository
    {
        IQueryable<SovbKhoiph> SovbKhoiphs { get; }

        void Them(SovbKhoiph sovb);

        void Xoa(int intid);
    }
}