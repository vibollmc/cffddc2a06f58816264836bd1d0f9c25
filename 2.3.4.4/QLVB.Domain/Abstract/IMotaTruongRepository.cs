using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IMotaTruongRepository
    {
        IQueryable<MotaTruong> MotaTruongs { get; }
    }
}