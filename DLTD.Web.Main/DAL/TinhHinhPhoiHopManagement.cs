using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DLTD.Web.Main.Common;
using DLTD.Web.Main.Models;
using DLTD.Web.Main.Models.MetaData;

namespace DLTD.Web.Main.DAL
{
    public class TinhHinhPhoiHopManagement
    {
        private readonly MainDbContext _dbContext;
        private static TinhHinhPhoiHopManagement _instance;

        public static TinhHinhPhoiHopManagement Go
        {
            get { return _instance ?? (_instance = new TinhHinhPhoiHopManagement()); }
        }
        public TinhHinhPhoiHopManagement()
        {
            _dbContext = new MainDbContext();
        }

        public async Task<IEnumerable<TinhHinhPhoiHop>> GetTinhHinhPhoiHop(int? idVanBanChiDao)
        {
            return await _dbContext.TinhHinhPhoiHop.Where(x => x.DonViPhoiHop.IdVanBan == idVanBanChiDao).OrderByDescending(x=>x.NgayXuLy).ToListAsync();
        }
        public async Task<IEnumerable<DonViPhoiHop>> GetDonViPhoiHop(int? idVanBan)
        {
            return await _dbContext.DonViPhoiHop.Where(x => x.IdVanBan == idVanBan).ToListAsync();
        }
        public async Task<bool> SaveTinhHinhPhoiHop(TinhHinhThucHienInput data)
        {
            using (var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    var donViPhoiHop =
                        await
                            _dbContext.DonViPhoiHop.SingleOrDefaultAsync(
                                x => x.IdDonvi == data.IdDonVi && x.IdVanBan == data.IdVanBanChiDao);

                    if (donViPhoiHop == null) return false;

                    var thph = new TinhHinhPhoiHop
                    {
                        IdDonViPhoiHop = donViPhoiHop.Id,
                        NgayXuLy = data.NgayBaoCao,
                        UserId = data.UserId,
                        NoiDungThucHien = data.NoiDungBaoCao
                    };

                    _dbContext.TinhHinhPhoiHop.Add(thph);

                    await _dbContext.SaveChangesAsync();

                    if (string.IsNullOrWhiteSpace(data.FileDinhKem)
                        || string.IsNullOrWhiteSpace(data.FileUrl))
                    {
                        dbTransaction.Commit();
                        return true;
                    }

                    _dbContext.FileTinhHinhPhoiHop.Add(new FileTinhHinhPhoiHop
                    {
                        IdTinhHinhPhoiHop = thph.Id,
                        Name = data.FileDinhKem,
                        Url = data.FileUrl
                    });

                    await _dbContext.SaveChangesAsync();

                    dbTransaction.Commit();
                    return true;
                }
                catch
                {
                    dbTransaction.Rollback();
                    return false;
                }
            }
        }
        public async Task<bool> DeleteTinhHinhPhoiHop(int? id)
        {
            using (var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var fileDinhKem =
                        _dbContext.FileTinhHinhPhoiHop.Where(x => x.IdTinhHinhPhoiHop == id);

                    _dbContext.FileTinhHinhPhoiHop.RemoveRange(fileDinhKem);

                    await _dbContext.SaveChangesAsync();

                    var thth = _dbContext.TinhHinhPhoiHop.Where(x => x.Id == id);

                    _dbContext.TinhHinhPhoiHop.RemoveRange(thth);

                    await _dbContext.SaveChangesAsync();

                    dbTransaction.Commit();
                    return true;
                }
                catch
                {
                    dbTransaction.Rollback();
                    return false;
                }
            }
        }
    }
}