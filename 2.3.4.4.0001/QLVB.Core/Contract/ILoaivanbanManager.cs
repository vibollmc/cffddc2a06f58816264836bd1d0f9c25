using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.DTO.Loaivanban;

namespace QLVB.Core.Contract
{
    public interface ILoaivanbanManager
    {
        /// <summary>
        /// lay cac loai van ban den (active)
        /// </summary>
        /// <returns></returns>
        IEnumerable<PhanloaiVanban> GetLoaivanban(int intloai);
        /// <summary>
        /// lay 1 loai vb den (active)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PhanloaiVanban GetIdLoaivanban(int id);
        /// <summary>
        /// lay cac truong mo ta cua loai van ban
        /// </summary>
        /// <param name="idloaivb"></param>
        /// <returns></returns>
        ListTruongvbViewModel GetLoaitruongvanban(int idloaivb);


        int SaveLoaivanban(EditLoaivanbanViewModel model, int intloai);

        int DeleteLoaivanban(DeleteLoaivanbanViewModel model);

        /// <summary>
        /// cap nhat hien thi va stt cac truong van ban
        /// </summary>
        /// <param name="id"></param>
        /// <param name="IsDisplay"></param>
        /// <param name="intorder"></param>
        void EditPhanloaiTruong(int id, bool IsDisplay, int intorder);
    }
}
