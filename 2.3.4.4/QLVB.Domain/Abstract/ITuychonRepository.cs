using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;

namespace QLVB.Domain.Abstract
{
    public interface ITuychonRepository
    {
        IQueryable<Tuychon> Tuychons { get; }

        string GetTuychon(string strthamso);

        int GetTuychonToInt(string strthamso);

        bool GetTuychonToBool(string strthamso);
    }
}
