using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFQuytrinhNodeRepository : IQuytrinhNodeRepository
    {
        private QLVBDatabase context;

        public EFQuytrinhNodeRepository(QLVBDatabase _context)
        {
            context = _context;
        }
        public IQueryable<QuytrinhNode> QuytrinhNodes
        {
            get { return context.QuytrinhNodes; }
        }

        public int Them(QuytrinhNode node)
        {
            context.QuytrinhNodes.Add(node);
            context.SaveChanges();
            return node.intid;
        }

        public void Xoa(int idquytrinh, string nodeid)
        {
            try
            {
                var node = context.QuytrinhNodes
                .Where(p => p.intidquytrinh == idquytrinh)
                .Where(p => p.NodeId == nodeid)
                .FirstOrDefault();
                context.QuytrinhNodes.Remove(node);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// xoa danh sach cac node
        /// </summary>
        /// <param name="listidnode"></param>
        /// <returns></returns>
        public int Xoa(List<int> listidnode)
        {
            try
            {
                var nodes = context.QuytrinhNodes
                    .Where(p => listidnode.Contains(p.intid));
                context.QuytrinhNodes.RemoveRange(nodes);
                context.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }
        }


        public int Capnhat(QuytrinhNode node)
        {
            var _node = context.QuytrinhNodes
                .FirstOrDefault(p => p.intid == node.intid);
            if (_node != null)
            {
                try
                {
                    _node = node;
                    context.SaveChanges();
                    return 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return 0;
            }
        }

    }
}
