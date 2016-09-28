using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO.Account;
using QLVB.DTO;

namespace QLVB.Core.Contract
{
    public interface IAccountManager
    {
        /// <summary>
        /// kiểm tra tên và mật khẩu đăng nhập, return iduser!=0
        /// </summary>
        /// <param name="strusername"></param>
        /// <param name="strpassword"></param>
        /// <returns>id user
        /// 0: khong dang nhap dc
        /// iduser!=0: thanh cong
        /// </returns>
        int ValidateUser(string strusername, string strpassword);

        /// <summary>
        /// set ten don vi sau khi login
        /// </summary>
        /// <param name="strtendonvi"></param>
        void SetTenDonvi(string strtendonvi);

        /// <summary>
        /// Thay doi password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultFunction ChangePassword(ChangePasswordViewModel model);

        /// <summary>
        /// trang chu sau khi login
        /// </summary>
        /// <returns></returns>
        HomeViewModel GetHomePage(int idcanbo);

        /// <summary>
        /// login voi cac quyen duoc uy quyen
        /// </summary>
        /// <param name="idcanbo"></param>
        void GetUyquyen(int idcanbo);

        /// <summary>
        /// lay ds cac user dc uy quyen
        /// </summary>
        /// <returns></returns>
        ListUserUyquyenViewModel GetListUserUyquyen();

        ResultFunction SaveUyquyen(string strAddIdUser);

        /// <summary>
        ///  lay cac option cua user
        ///  hien gio chi moi co phan xu ly nhieu van ban 
        /// </summary>
        /// <returns></returns>
        OptionViewModel GetOption();

        /// <summary>
        /// cap nhat tuy chon ca nhan
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int SaveOption(OptionViewModel model);

        TuychonViewModel GetTuychon();

        ResultFunction SaveTuychon(Dictionary<string, string> dic);

        /// <summary>
        /// lay ten cua don vi
        /// </summary>
        /// <param name="strentext"></param>
        /// <returns></returns>
        string GetTenDonvi(string strentext);

        string GetTenDonvi();

        /// <summary>
        /// kiem tra xem co su dung module ykcd khong
        /// </summary>
        void CheckYKCD();

        IList<string> GetCanBoByUserName(string strUsername);

    }
}
