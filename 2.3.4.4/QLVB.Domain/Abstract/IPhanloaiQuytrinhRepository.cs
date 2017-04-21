using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IPhanloaiQuytrinhRepository
    {
        IQueryable<PhanloaiQuytrinh> PhanloaiQuytrinhs { get; }

        int Them(PhanloaiQuytrinh hs);

        int Them(string strtenloaiquytrinh);

        int Sua(int intid, string strten);

        void Xoa(int intid);
    }
}
