using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface INhatkyRepository
    {
        IQueryable<Nhatky> Nhatkys { get; }
        IQueryable<NLogError> NlogError { get; }

        IQueryable<Nhatky> Tonghop { get; }

    }

}