using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface ISoVanbanRepository
    {
        IQueryable<SoVanban> GetActiveSoVanbans { get; }

        IQueryable<SoVanban> GetAllSoVanbans { get; }

        void Themmoi(SoVanban sovb);

        void Capnhat(int intid, SoVanban sovb);

        /// <summary>
        /// cap nhat trang thai notactive
        /// </summary>
        /// <param name="intid"></param>
        void Xoa(int intid);

        void CapnhatKhoiph(int intid, int intidkhoiph);

        void CapnhatLoaivb(int intid, int intidloaivb);

        /// <summary>
        /// update lại isdefault=false của tất cả các sổ văn bản đến/đi
        /// chỉ cho phép 1 sổ vb được hiển thị mặc định
        /// </summary>
        /// <param name="intloai">The intloai.</param>
        void UpdateIsDefault(int intloai);
    }
}