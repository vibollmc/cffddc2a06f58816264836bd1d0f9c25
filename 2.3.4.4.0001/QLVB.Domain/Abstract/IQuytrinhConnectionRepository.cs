using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IQuytrinhConnectionRepository
    {
        IQueryable<QuytrinhConnection> QuytrinhConnections { get; }
        int Them(QuytrinhConnection connection);
        void Xoa(int from, int to);

        /// <summary>
        /// xoa danh sach cac node from
        /// </summary>
        /// <param name="listidFrom"></param>
        /// <returns></returns>
        int Xoa(List<int> listidFrom);
    }
}
