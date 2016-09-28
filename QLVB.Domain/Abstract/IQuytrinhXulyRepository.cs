using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IQuytrinhXulyRepository
    {
        IQueryable<QuytrinhXuly> QuytrinhXulys { get; }

        int Them(QuytrinhXuly xuly);

        int Xoa(int intidNode);

    }
}
