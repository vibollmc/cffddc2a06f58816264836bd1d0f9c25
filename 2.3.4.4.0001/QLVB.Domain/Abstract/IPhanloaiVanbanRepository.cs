using QLVB.Domain.Entities;
using System;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IPhanloaiVanbanRepository
    {
        /// <summary>
        /// chi lay cac loai van ban dang active
        /// </summary>
        IQueryable<PhanloaiVanban> GetActivePhanloaiVanbans { get; }

        /// <summary>
        /// lay tat ca cac loai van ban
        /// </summary>
        IQueryable<PhanloaiVanban> GetAllPhanloaiVanbans { get; }

        /// <summary>
        /// lay loai vb theo id
        /// </summary>
        /// <param name="intid"></param>
        /// <returns></returns>
        PhanloaiVanban GetLoaiVB(Int32 intid);

        int AddLoaiVB(PhanloaiVanban loaivb);

        void EditLoaiVB(Int32 intid, PhanloaiVanban loaivb);

        /// <summary>
        /// chi cap nhat trang thai: notactive
        /// </summary>
        /// <param name="intid"></param>
        void DeleteLoaiVB(Int32 intid);

        /// <summary>
        /// update lại isdefault=false của tất cả các loại văn bản đến/đi
        /// chỉ cho phép 1 loại vb được hiển thị mặc định
        /// </summary>
        /// <param name="intloai">The intloai.</param>
        void UpdateIsDefault(Int32 intloai);
    }
}