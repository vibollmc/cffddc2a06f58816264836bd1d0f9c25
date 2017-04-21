using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DAL
{
    public class EFNhatkyRepository : INhatkyRepository
    {
        private QLVBDatabase context;

        public EFNhatkyRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Nhatky> Nhatkys
        {
            get { return context.Nhatkys; }
        }

        public IQueryable<NLogError> NlogError
        {
            get { return context.NlogErrors; }
        }

        public IQueryable<Nhatky> Tonghop
        {
            get
            {
                var nhatky = context.Nhatkys;

                var Error = context.NlogErrors
                            .Select(p => new Nhatky
                            {
                                //Id = p.Id,
                                client = p.client,
                                host = p.host,
                                level = p.level,
                                username = p.username,
                                time_stamp = p.time_stamp,
                                message = p.message

                            });

                var tonghop = nhatky.Union(Error)
                    .OrderByDescending(p => p.time_stamp);

                return tonghop;

            }
        }
    }
}
