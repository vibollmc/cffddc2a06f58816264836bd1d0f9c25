using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface ICategoryRepository
    {
        IQueryable<CategoryVanban> CategoryVanbans { get; }
    }
}