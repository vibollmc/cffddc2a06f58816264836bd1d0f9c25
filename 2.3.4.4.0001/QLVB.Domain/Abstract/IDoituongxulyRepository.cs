using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IDoituongxulyRepository
    {
        IQueryable<Doituongxuly> Doituongxulys { get; }

        /// <summary>
        /// chỉ lấy các user đã từng tham gia xử lý
        /// kể cả đã chuyển xử lý
        /// </summary>
        IQueryable<Doituongxuly> GetAllCanboxulys { get; }

        /// <summary>
        /// chỉ lấy các user đang tham gia xử lý
        /// không lấy user đã chuyển xử lý
        /// </summary>
        IQueryable<Doituongxuly> GetCanboDangXulys { get; }

        int Them(Doituongxuly dt);

        void Sua(int intid, Doituongxuly dt);

        void Xoa(int intid);

        void XoaCacCanbo(int idhosocongviec);

        /// <summary>
        /// cap nhat vai tro xu ly
        /// chuyen vai tro qua vai tro cu
        /// ghi nhan thoi gian va nguoi chuyen
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="intvaitro"></param>
        void CapnhatVaitrocu(int intid, int idcanbo);

        /// <summary>
        /// kiem tra xem co idcanbo da co vai tro nay trong ho so chua? True: co, False: khong
        /// </summary>
        /// <param name="idhosocongviec"></param>
        /// <param name="idcanbo"></param>
        /// <param name="intvaitro"></param>
        /// <returns>
        /// true:  co
        /// false: khong co
        /// </returns>
        bool KiemtraVaitroxuly(int idhosocongviec, int idcanbo, int intvaitro);
    }
}