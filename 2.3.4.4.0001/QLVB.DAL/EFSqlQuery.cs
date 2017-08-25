using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFSqlQuery : ISqlQuery
    {
        private QLVBDatabase context;

        public EFSqlQuery(QLVBDatabase _context)
        {
            context = _context;
        }
        public int RunSqlCommand(string query)
        {
            try
            {
                context.Database.CommandTimeout = 300;
                return context.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// dung de tong hop van ban di dien tu 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public object ExecSql_TonghopVBDiDientu(string query)
        {
            try
            {
                var vb =
               context.Database.SqlQuery<QLVB.DTO.Vanbandientu.TonghopVBDiViewModel>(query);
                return vb;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
