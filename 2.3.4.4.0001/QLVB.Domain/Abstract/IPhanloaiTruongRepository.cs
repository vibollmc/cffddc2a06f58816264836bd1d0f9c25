using QLVB.Domain.Entities;
using System;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IPhanloaiTruongRepository
    {
        IQueryable<PhanloaiTruong> PhanloaiTruongs { get; }

        void AddPhanloaiTruong(PhanloaiTruong loaitruong);

        void EditPhanloaiTruong(PhanloaiTruong loaitruong);

        /// <summary>
        /// cap nhat IsDisplay va intorder
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="IsDisplay"></param>
        /// <param name="intorder"></param>
        void EditPhanloaiTruong(Int32 intid, bool IsDisplay, Int32 intorder);

        /// <summary>
        /// chi edit field IsDisplay
        /// </summary>
        /// <param name="intid">The intid.</param>
        /// <param name="IsDisplay">The is display.</param>
        void EditDisplay(Int32 intid, bool IsDisplay);

        /// <summary>
        /// cap nhat intorder cua cac truong IsRequire
        /// mac dinh IsDisplay = true
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="intorder"></param>
        void EditOrder(Int32 intid, int intorder);
    }
}