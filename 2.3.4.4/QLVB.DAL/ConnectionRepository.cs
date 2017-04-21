using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class ConnectionRepository : IConnectionRepository
    {
        private QLVBDatabase context;

        public ConnectionRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Connection> Connections
        {
            get { return context.Connections; }
        }

        public string GetConnectionId(int UserId)
        {
            return context.Connections
                .FirstOrDefault(p => p.UserId == UserId)
                .ConnectionId;
        }
        public void AddUserConnection(Connection con)
        {
            try
            {
                con.LastActivity = DateTime.Now;
                context.Connections.Add(con);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateConnection(int UserId, bool IsConnected)
        {
            try
            {
                var con = context.Connections.FirstOrDefault(p => p.UserId == UserId);
                if (con != null)
                {
                    con.Connected = IsConnected;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
