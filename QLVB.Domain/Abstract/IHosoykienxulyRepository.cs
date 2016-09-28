using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IHosoykienxulyRepository
    {
        IQueryable<Hosoykienxuly> Hosoykienxulys { get; }

        int Them(Hosoykienxuly hs);

        /// <summary>
        /// chi update trang thai, va ykien
        /// </summary>
        /// <param name="intid"></param>
        /// <param name="hs"></param>
        void Sua(int intid, Hosoykienxuly hs);

        void Xoa(int intid);
    }
}