using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IQuytrinhVersionRepository
    {
        IQueryable<QuytrinhVersion> QuytrinhVersions { get; }

        int Them(QuytrinhVersion qt);

        bool Xoa(int idquytrinh, DateTime ngayapdung);


    }
}
