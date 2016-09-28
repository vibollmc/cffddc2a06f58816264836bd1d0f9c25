using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface ISystems
    {
        IQueryable<Systems> Systems { get; }
    }
}