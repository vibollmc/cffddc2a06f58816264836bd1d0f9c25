using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IMenuRepository
    {
        IQueryable<Menu> Menus { get; }

        /// <summary>
        /// cap nhat trang thai cua menu ykcd
        /// </summary>
        /// <param name="inttrangthai"></param>
        /// <returns></returns>
        int UpdateYkcd(int inttrangthai);
    }
}