using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IHosoQuytrinhXulyRepository
    {
        IQueryable<HosoQuytrinhXuly> HosoQuytrinhxulys { get; }

        int Them(HosoQuytrinhXuly hoso);

        int CapnhatTrangthai_DaXuly(int id, int inttrangthai);

        int CapnhatTrangthai_DangXuly(int id, int inttrangthai);

        int CapnhatTrangthai_DaXuly(int idhoso, string nodeidFrom, int inttrangthai);

        int CapnhatTrangthai_DangXuly(int idhoso, string nodeidFrom, int inttrangthai);

        /// <summary>
        /// cap nhat trang thai cua tat cac cac node tru node da duyet
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="listidNodeDaduyet"></param>
        /// <param name="inttrangthai"></param>
        /// <returns></returns>
        int CapnhatTrangthai(int idhoso, List<int> listidNodeDaduyet, int inttrangthai);

        bool Xoa(int idhoso);

        bool CapnhatCanbo(int idhoso, string NodeId, int idcanbo);

        /// <summary>
        /// cap nhat thoi gian xu ly tai nodefrom
        /// </summary>
        /// <param name="idhoso"></param>
        /// <param name="intidFrom"></param>
        /// <param name="intsongay"></param>
        /// <returns></returns>
        int CapnhatThoigianXuly(int idhoso, int intidFrom, int intsongay);
    }
}
