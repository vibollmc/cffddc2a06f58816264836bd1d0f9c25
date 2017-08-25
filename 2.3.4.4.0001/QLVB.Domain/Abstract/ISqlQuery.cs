using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.Domain.Abstract
{
    public interface ISqlQuery
    {
        int RunSqlCommand(string query);

        object ExecSql_TonghopVBDiDientu(string query);
    }
}
