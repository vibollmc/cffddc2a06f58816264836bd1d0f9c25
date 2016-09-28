using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface ICanboRepository
    {
        /// <summary>
        /// Lấy tất cả cán bộ
        /// </summary>
        IQueryable<Canbo> GetAllCanbo { get; }

        /// <summary>
        /// Chỉ lấy cán bộ đang active
        /// </summary>
        IQueryable<Canbo> GetActiveCanbo { get; }
        int Them(Canbo cb);

        /// <summary>
        /// cap nhat cac truong canbo, tru strpassword
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cb"></param>
        void Sua(int id, Canbo cb);

        void ResetPassword(int id, string strpass);

        /// <summary>
        /// cap nhat trang thai user: notactive
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>strhoten</returns>
        string Xoa(int id);

        Canbo GetActiveCanboByID(int id);

        Canbo GetAllCanboByID(int id);

        Canbo GetAllCanboByUserName(string strUsername);

        List<Canbo> GetListCanbo();

        /// <summary>
        /// cap nhat don vi cho user
        /// </summary>
        /// <param name="iduser"></param>
        /// <param name="iddonvi"></param>
        void UpdateDonvi(int iduser, int iddonvi);

        /// <summary>
        /// lay tat ca cac tuy chon cua 1 user (gia tri mac dinh)
        /// </summary>
        /// <param name="iduser"></param>
        /// <returns></returns>
        Dictionary<int, string> GetListOption();
        /// <summary>
        /// lay tuy chon cua 1 user (strRight)
        /// </summary>
        /// <param name="iduser"></param>
        /// <returns></returns>
        string GetUserOption(int iduser);
        /// <summary>
        /// kiem tra cac option cua user
        /// </summary>
        /// <param name="iduser"></param>
        /// <param name="intOption"></param>
        /// <returns></returns>        
        bool IsOption(int iduser, int intOption);

        /// <summary>
        /// kiem tra tuy chon cua user 
        /// </summary>
        /// <param name="strRight"></param>
        /// <param name="intOption"></param>
        /// <returns></returns>
        bool IsOption(string strRight, int intOption);

        void UpdateOption(int iduser, int intOption, bool blgiatri);

        void UpdateOption(int iduser, string strRight);

        /// <summary>
        /// cap nhat nhom quyen cua user
        /// </summary>
        /// <param name="iduser"></param>
        /// <param name="idnhomquyen"></param>
        void UpdateNhomquyen(int iduser, int idnhomquyen);

        /// <summary>
        /// cap nhat avatar cua user
        /// </summary>
        /// <param name="iduser"></param>
        /// <param name="strImageProfile"></param>
        void UpdateImageProfile(int iduser, string strImageProfile);

    }
}
