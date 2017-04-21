using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IUyQuyenRepository
    {
        IQueryable<UyQuyen> UyQuyens { get; }

        int Them(UyQuyen u);

        int Them(int idUserSend, int idUserRec);

        void Xoa(int idUserSend);

        void Xoa(int idUserSend, int idUserRec);
    }
}
