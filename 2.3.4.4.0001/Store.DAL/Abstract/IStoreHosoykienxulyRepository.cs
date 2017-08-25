using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Abstract
{
    public interface IStoreHosoykienxulyRepository
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
