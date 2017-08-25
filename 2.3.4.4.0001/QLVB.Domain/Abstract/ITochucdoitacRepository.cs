using QLVB.Domain.Entities;
using System;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface ITochucdoitacRepository
    {
        IQueryable<Tochucdoitac> GetActiveTochucdoitacs { get; }

        IQueryable<Tochucdoitac> GetAllTochucdoitacs { get; }

        void AddTochuc(Tochucdoitac tochuc);

        void EditTochuc(Int32 intid, Tochucdoitac tochuc);

        /// <summary>
        /// cap nhat trang thai notactive
        /// </summary>
        /// <param name="intid"></param>
        void DeleteTochuc(Int32 intid);
        /// <summary>
        /// xoa thuc trong database
        /// </summary>
        /// <param name="id"></param>
        void XoaTochuc(int id);

        void UpdateTochuc(int id, string madinhdanh, string matructinh);
    }
}