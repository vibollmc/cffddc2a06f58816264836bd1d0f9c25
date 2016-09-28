using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Donvi;

namespace QLVB.Core.Contract
{
    public interface IDonviManager
    {
        /// <summary>
        /// lay tat ca cac don vi truc thuoc
        /// theo dang cay
        /// </summary>
        /// <returns></returns>
        IList<Donvitructhuoc> GetRootDonvi();

        /// <summary>
        /// lay cac user trong phong ban
        /// </summary>
        /// <param name="iddonvi"></param>
        /// <returns></returns>
        IEnumerable<ListUserViewModel> GetListUsers(int iddonvi);

        /// <summary>
        /// lay 1 don vi truc thuoc theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EditDonviViewModel GetDonvi(int id);

        /// <summary>
        /// cap nhat don vi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int SaveDonvi(EditDonviViewModel model);

        /// <summary>
        /// kiem tra neu co quan he thi chi doi inttrangthai: notactive,
        /// neu khong co thi xoa han trong database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int DeleteDonvi(DeleteDonviViewModel model);

        /// <summary>
        /// them moi user thuoc phong ban nao
        /// hoac lay thong tin user hien co
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EditUserViewModel GetUser(int iduser, int iddonvi);

        /// <summary>
        /// Luu thong tin user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultFunction SaveUser(EditUserViewModel model);

        /// <summary>
        /// Kiem tra username da co trong database chua
        /// </summary>
        /// <param name="strusername"></param>
        /// <returns>true: chua co
        /// false: da co
        /// </returns>
        bool CheckUsername(string strusername);

        /// <summary>
        /// cap nhat trang thai cua user: notactive
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultFunction DeleteUser(int id);


        /// <summary>
        /// lay cac danh sach phong ban
        /// </summary>
        /// <param name="iddonvi">id don vi dang chon de move user</param>
        /// <returns></returns>
        MoveUserViewModel GetMoveUser(int idsource, int iddest);
        /// <summary>
        /// cap nhat phong ban cho cac user
        /// </summary>
        /// <param name="strlistiduser"></param>
        /// <param name="iddonvi"></param>
        /// <returns></returns>
        ResultFunction UpdateMoveUser(string strlistiduser, int iddonvi);

    }
}
