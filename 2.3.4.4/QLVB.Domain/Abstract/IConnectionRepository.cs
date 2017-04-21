using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface IConnectionRepository
    {
        IQueryable<Connection> Connections { get; }

        string GetConnectionId(int UserId);

        void AddUserConnection(Connection con);

        void UpdateConnection(int UserId, bool IsConnected);

    }
}
