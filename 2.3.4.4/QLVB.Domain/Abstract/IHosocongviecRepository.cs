using QLVB.Domain.Entities;
using System;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IHosocongviecRepository
    {
        IQueryable<Hosocongviec> Hosocongviecs { get; }

        Hosocongviec GetHSCVById(int id);

        int Them(Hosocongviec hs);

        /// <summary>
        /// cap nhat ho so cong viec va mo lai ho so
        /// rieng strnoidung thi cap nhat tuy theo quyen cua user
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="hs"></param>
        /// <param name="IsUpdateStrNoidung"></param>
        void Sua(int intid, Hosocongviec hs, bool IsUpdateStrNoidung);

        string Xoa(int intid);

        void LuuHoso(int intid, int idcanbo);

        void HoanthanhHoso(int intid, int idcanbo);

        void Trinhky(int intid, int idcanbo);

        void TamngungQuytrinh(int intid, int intluuhoso, DateTime dteHanxuly);

        void CapnhatThoihanxuly(int idhoso, DateTime dteHanxuly);
    }
}