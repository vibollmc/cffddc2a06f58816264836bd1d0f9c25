using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IBaocaoRepository
    {
        IQueryable<Baocao> Baocaos { get; }
    }
}