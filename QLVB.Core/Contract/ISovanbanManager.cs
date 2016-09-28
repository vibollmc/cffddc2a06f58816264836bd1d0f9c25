using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Sovb;

namespace QLVB.Core.Contract
{
    public interface ISovanbanManager
    {
        /// <summary>
        /// lay danh sach cac loai so van ban
        /// </summary>
        /// <returns></returns>
        ListSovanbanViewModel GetListSovb();

        /// <summary>
        /// lay loai so van ban den/ so van ban di
        /// </summary>
        /// <param name="idsovb"></param>
        /// <returns></returns>
        int GetLoaiSovb(int idsovb);

        /// <summary>
        /// lay danh sach cac khoi phat hanh cua so van ban den
        /// </summary>
        /// <param name="idsovb"></param>
        /// <returns></returns>
        ListKhoiphathanhViewModel GetListKhoiphathanh(int idsovb);

        /// <summary>
        /// cap nhat khoi phat hanh cua so vb den
        /// </summary>
        /// <param name="idsovb"></param>
        /// <param name="idkhoiph"></param>
        /// <returns></returns>
        ResultFunction SaveKhoiphathanh(int idsovb, int idkhoiph);

        /// <summary>
        /// lay ds cac loai van ban cua so vb di
        /// </summary>
        /// <param name="idsovb"></param>
        /// <returns></returns>
        ListLoaivanbanViewModel GetListLoaivanban(int idsovb);

        /// <summary>
        /// cap nhat loai vb cua so vb di
        /// </summary>
        /// <param name="idsovb"></param>
        /// <param name="idloaivb"></param>
        /// <returns></returns>
        ResultFunction SaveLoaivanban(int idsovb, int idloaivb);

        /// <summary>
        /// lay thong tin cua so van ban
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EditSovanbanViewModel GetEditSovanban(int id);

        /// <summary>
        /// them moi/cap nhat so van ban den/di
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultFunction SaveSovanban(EditSovanbanViewModel model);
        /// <summary>
        /// cap nhat trang thai notactive
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultFunction DeleteSovanban(EditSovanbanViewModel model);

    }
}
