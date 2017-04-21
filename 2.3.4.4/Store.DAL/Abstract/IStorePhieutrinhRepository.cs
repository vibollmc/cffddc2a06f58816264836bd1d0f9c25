using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DAL.Abstract
{
    public interface IStorePhieutrinhRepository
    {
        IQueryable<Phieutrinh> Phieutrinhs { get; }

        int ThemNoidungtrinh(int idhoso, int idcanbotrinh, int idlanhdaotrinh, string strnoidungtrinh);

        int ThemYkienchidao(int idphieutrinh, int idlanhdao, string strykienchidao);
    }
}
