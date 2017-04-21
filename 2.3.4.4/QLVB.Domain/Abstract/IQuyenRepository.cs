using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IQuyenRepository
    {
        IQueryable<Quyen> Quyens { get; }

        /// <summary>
        /// cap nhat trang thai quyen cua module ykcd
        /// </summary>
        /// <param name="inttrangthai"></param>
        /// <returns></returns>
        int UpdateYKCD(int inttrangthai);
    }
}