using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QLVB.Core.Implementation
{
    public class ReportManager : IReportManager
    {
        #region Constructor

        private IVanbandenRepository _vbdenRepo;
        private ISoVanbanRepository _sovbRepo;
        private IVanbandiRepository _vbdiRepo;

        public ReportManager(IVanbandenRepository vbdenRepo, ISoVanbanRepository sovbRepo,
            IVanbandiRepository vbdiRepo)
        {
            _vbdenRepo = vbdenRepo;
            _sovbRepo = sovbRepo;
            _vbdiRepo = vbdiRepo;
        }

        #endregion Constructor

        public string GetTenSovanban(int idsovb)
        {
            return _sovbRepo.GetActiveSoVanbans.FirstOrDefault(p => p.intid == idsovb).strten;
        }

        public IEnumerable<Vanbanden> GetRptSovanbanden(DateTime? dtengaybd, DateTime? dtengaykt, int idsovb)
        {
            var vanban = _vbdenRepo.Vanbandens
                            .Where(p => p.intidsovanban == idsovb)
                            .Where(p => p.strngayden >= dtengaybd)
                            .Where(p => p.strngayden <= dtengaykt);
            return vanban;
        }
    }
}