using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using QLVB.DTO.Log;

namespace QLVB.Core.Implementation
{
    public class LogManager : ILogManager
    {
        #region Constructor

        private IConfigRepository _configRepo;
        private INhatkyRepository _nhatkyRepo;
        private INlogErrorRepository _nlogRepo;

        public LogManager(INhatkyRepository nhatkyRepo, INlogErrorRepository nlogRepo,
                            IConfigRepository configRepo)
        {
            _nhatkyRepo = nhatkyRepo;
            _nlogRepo = nlogRepo;
            _configRepo = configRepo;
        }

        #endregion Constructor

        #region Interface Implementation

        public IEnumerable<LogViewModel> GetNhatkysudung(int intloai)
        {
            int intngay = _configRepo.GetConfigToInt(ThamsoHethong.SoNgayHienThi);

            if (intloai == 1)
            {
                var nhatky = _nhatkyRepo.Nhatkys//.Where(p => EntityFunctions.DiffDays(p.time_stamp, DateTime.Now) < intngay);
                        .Select(p => new LogViewModel
                        {
                            intloai = intloai,
                            client = p.client,
                            host = p.host,
                            level = p.level,
                            username = p.username,
                            time_stamp = p.time_stamp,
                            message = p.message

                        })
                       .OrderByDescending(p => p.time_stamp);
                return nhatky;
            }
            else
            {
                var Error = _nlogRepo.Nlogger
                            .Select(p => new LogViewModel
                            {
                                intloai = intloai,
                                client = p.client,
                                host = p.host,
                                level = p.level,
                                username = p.username,
                                time_stamp = p.time_stamp,
                                message = p.message

                            })
                            .OrderByDescending(p => p.time_stamp);
                return Error;
            }
        }

        #endregion Interface Implementation
    }
}