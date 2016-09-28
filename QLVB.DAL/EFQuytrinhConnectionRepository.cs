using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFQuytrinhConnectionRepository : IQuytrinhConnectionRepository
    {
        private QLVBDatabase context;

        public EFQuytrinhConnectionRepository(QLVBDatabase _context)
        {
            context = _context;
        }
        public IQueryable<QuytrinhConnection> QuytrinhConnections
        {
            get { return context.QuytrinhConnections; }
        }
        public int Them(QuytrinhConnection connection)
        {
            context.QuytrinhConnections.Add(connection);
            context.SaveChanges();
            return connection.intid;
        }
        public void Xoa(int from, int to)
        {
            var con = context.QuytrinhConnections
                .Where(p => p.intidFrom == from)
                .Where(p => p.intidTo == to)
                .FirstOrDefault();
            context.QuytrinhConnections.Remove(con);
            context.SaveChanges();
        }

        /// <summary>
        /// xoa danh sach cac node from
        /// </summary>
        /// <param name="listidFrom"></param>
        /// <returns></returns>
        public int Xoa(List<int> listidFrom)
        {
            try
            {
                var conn = context.QuytrinhConnections
                    .Where(p => listidFrom.Contains(p.intidFrom));
                context.QuytrinhConnections.RemoveRange(conn);
                context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}
