using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.DTO.Hethong;
using QLVB.DTO;

namespace QLVB.Core.Contract
{
    public interface IHethongManager
    {
        /// <summary>
        /// Lay tat ca cac gia tri he thong
        /// </summary>
        /// <returns></returns>
        IEnumerable<Config> GetAllConfig();

        /// <summary>
        /// Luu cac gia tri he thong
        /// </summary>
        /// <param name="config"></param>
        /// <returns>1: success</returns>
        int SaveAllConfig(Dictionary<string, string> config);

        int SaveAllConfig(Dictionary<int, string> config);

        /// <summary>
        /// lay tat ca cac nhom quyen
        /// </summary>
        /// <returns></returns>
        IEnumerable<NhomQuyen> GetAllNhomQuyen();

        /// <summary>
        /// lay tat ca quyen trong nhom quyen
        /// </summary>
        /// <param name="idgroup"></param>
        /// <returns></returns>
        ListRoleGroupViewModel GetQuyenNhom(int idgroup);

        /// <summary>
        /// luu quyen thuoc nhom quyen
        /// </summary>
        /// <param name="idgroup"></param>
        /// <param name="role"></param>
        /// <returns>1: success</returns>
        int SaveQuyenNhom(int idgroup, List<int> role);

        /// <summary>
        /// lay tat cac user trong nhom quyen
        /// </summary>
        /// <param name="idgroup"></param>
        /// <returns></returns>
        ListUserViewModel GetUserGroup(int idgroup);

        IEnumerable<UserViewModel> GetUserInGroup(int idgroup);

        /// <summary>
        /// doc thong tin nhom quyen
        /// </summary>
        /// <param name="intid"></param>
        /// <returns></returns>
        EditGroupViewModel GetGroup(int intid);
        /// <summary>
        /// them moi/cap nhat nhom quyen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int SaveGroup(EditGroupViewModel model);

        /// <summary>
        /// xoa nhom quyen
        /// </summary>
        /// <param name="intid"></param>
        /// <returns></returns>
        int DeleteGroup(int intid);

        /// <summary>
        /// lay tat ca cac user thuoc nhom quyen dang xet
        /// </summary>
        /// <param name="idnhomquyen"></param>
        /// <returns></returns>
        MoveNhomquyenUserViewModel GetNhomquyenUser(int idnhomquyen);

        /// <summary>
        /// cap nhat nhom quyen cua cac user
        /// </summary>
        /// <param name="listiduser"></param>
        /// <param name="idnhomquyen"></param>
        /// <returns></returns>
        ResultFunction SaveNhomquyenUser(string listiduser, int idnhomquyen);


        BackupViewModel GetFormBackup();

        /// <summary>
        /// backup database
        /// </summary>
        /// <returns></returns>
        ResultFunction BackupDatabase(BackupViewModel model);
    }
}
