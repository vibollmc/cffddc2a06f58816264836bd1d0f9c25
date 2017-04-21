using QLVB.Domain.Entities;
using System;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface ILinhvucRepository
    {
        IQueryable<Linhvuc> GetActiveLinhvucs { get; }

        IQueryable<Linhvuc> GetAllLinhvucs { get; }

        void AddLinhvuc(Linhvuc linhvuc);

        void EditLinhvuc(Int32 intid, Linhvuc linhvuc);

        /// <summary>
        /// cap nhat trang thai notactive
        /// </summary>
        /// <param name="intid"></param>
        void DeleteLinhvuc(Int32 intid);
    }
}