using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.DTO.Menu;

namespace QLVB.Core.Contract
{
    public interface IMenuManager
    {
        /// <summary>
        /// lay menu tuy theo quyen cua user
        /// </summary>
        /// <param name="idcanbo"></param>
        /// <returns></returns>
        IQueryable<Menu> GetMainMenu(int idcanbo);

        /// <summary>
        /// Lấy tên menu đang yêu cầu dựa trên request controller và action
        /// cho biết người dùng đang ở vị trí nào
        /// </summary>
        /// <returns>string tên menu</returns>
        string GetMenuName(string controllername, string actionname);

        /// <summary>
        /// lấy parentid menu đang active
        /// </summary>
        /// <param name="controllername"></param>
        /// <param name="actionname"></param>
        /// <returns></returns>
        int GetParentIdMenu(string controllername, string actionname);

        IEnumerable<Menu> GetSubMenu(int idcanbo);

        /// <summary>
        /// lay id user tu trong cookie da luu
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        int GetIdUser(string username);

        /// <summary>
        /// lay danh sach cac user uy quyen dua tren RealUserId
        /// </summary>
        /// <returns></returns>
        IEnumerable<UyquyenUserViewModel> GetListUserUyquyen();

        /// <summary>
        /// lay ten, chuc danh, quyen cua user dang login
        /// </summary>
        /// <returns></returns>
        HeaderViewModel GetHeaderUser(int iduser);

        bool CheckIsClickMenu(int iduser);

        string GetMenuType(int iduser);

        /// <summary>
        ///  kiem tra cau hinh cho phep tu dong nhan mail va quyen nhan vbdt cua user
        /// </summary>
        /// <returns></returns>
        bool CheckAutoReceiveMail();
        bool CheckAutoReceiveVBTructinh();

        /// <summary>
        /// kiem tra cau hinh cho phep tu dong send mail va quyen gui vbdt cua user
        /// </summary>
        /// <returns></returns>
        bool CheckAutoSendMail();

        QLVB.DTO.Hethong.SettingSendTonghopVBViewModel GetSettingSendTHVB();

        bool CheckTimeStartSendTHVB(QLVB.DTO.Hethong.SettingSendTonghopVBViewModel model);
    }
}
