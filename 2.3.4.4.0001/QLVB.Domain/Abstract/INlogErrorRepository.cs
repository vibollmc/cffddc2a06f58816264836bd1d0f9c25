using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface INlogErrorRepository
    {
        IQueryable<NLogError> Nlogger { get; }
    }
}