using QLVB.Domain.Entities;
using System.Linq;
using System.Collections.Generic;

namespace QLVB.Domain.Abstract
{
    public interface IVanbandenRepository
    {
        IQueryable<Vanbanden> Vanbandens { get; }

        Vanbanden GetVanbandenById(int id);

        int Them(Vanbanden vb);

        void Sua(int intid, Vanbanden vb);

        void Xoa(int intid);

        /// <summary>
        /// Duyet hoac huy duyet van ban 
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="inttrangthai"></param>
        void Duyet(int intid, int inttrangthai);


        /// <summary>
        /// Cap nhat strnoinhan(nguoi xu ly chinh)  sau khi phan xu ly van ban        
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="strxulychinh"></param>
        void CapnhatNguoixulychinh(int intid, string strxulychinh);

        void CapquyenxemPublic(int intid, int intpublic);

        /// <summary>
        /// cap nhat dang vb: vb dien tu/vb giay/...
        /// </summary>
        /// <param name="idvanban"></param>
        /// <param name="intdangvb"></param>
        void CapnhatDangVanban(int idvanban, int intdangvb);

        void CapnhatVBDT(int idvanban, int inttrangthai);

        /// <summary>
        /// list vanbanden
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        object RunSqlListVBDen(string query);

        object RunSqlListVBDenLienquan(string query);

    }
}