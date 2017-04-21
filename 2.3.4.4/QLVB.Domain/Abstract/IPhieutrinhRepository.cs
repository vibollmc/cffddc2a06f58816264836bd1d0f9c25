using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IPhieutrinhRepository
    {
        IQueryable<Phieutrinh> Phieutrinhs { get; }

        int ThemNoidungtrinh(int idhoso, int idcanbotrinh, int idlanhdaotrinh, string strnoidungtrinh);

        int ThemYkienchidao(int idphieutrinh, int idlanhdao, string strykienchidao);
    }
}
