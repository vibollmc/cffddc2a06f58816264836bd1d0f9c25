using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface ITinhtrangxulyRepository
    {
        IQueryable<Tinhtrangxuly> Tinhtrangxulys { get; }
    }
}
