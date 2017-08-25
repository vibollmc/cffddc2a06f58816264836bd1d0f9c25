using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IQuytrinhNodeRepository
    {
        IQueryable<QuytrinhNode> QuytrinhNodes { get; }

        int Them(QuytrinhNode node);

        void Xoa(int idquytrinh, string nodeid);

        /// <summary>
        /// xoa danh sach cac node
        /// </summary>
        /// <param name="listidnode"></param>
        /// <returns></returns>
        int Xoa(List<int> listidnode);

        int Capnhat(QuytrinhNode node);

    }
}
