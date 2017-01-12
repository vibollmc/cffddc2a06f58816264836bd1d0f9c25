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
        private static TinhHinhPhoiHopManagement _instance;
        public static TinhHinhPhoiHopManagement Go
        {
            get
            {
                if (_instance == null) _instance = new TinhHinhPhoiHopManagement();
                return _instance;
            }
        }

        public async Task<IEnumerable<TinhHinhPhoiHop>> GetTinhHinhPhoiHop(int? idVanBanChiDao)
        {
            var dbContext = new MainDbContext();
            return await dbContext.TinhHinhPhoiHop.Where(x => x.DonViPhoiHop.IdVanBan == idVanBanChiDao).OrderByDescending(x=>x.NgayXuLy).ToListAsync();
        }
        public async Task<IEnumerable<DonViPhoiHop>> GetDonViPhoiHop(int? idVanBan)
        {
            var dbContext = new MainDbContext();
            return await dbContext.DonViPhoiHop.Where(x => x.IdVanBan == idVanBan).ToListAsync();
        }
        public async Task<bool> SaveTinhHinhPhoiHop(TinhHinhThucHienInput data)
        {
            var dbContext = new MainDbContext();
            using (var dbTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {

                    var donViPhoiHop =
                        await
                            dbContext.DonViPhoiHop.SingleOrDefaultAsync(
                                x => x.IdDonvi == data.IdDonVi && x.IdVanBan == data.IdVanBanChiDao);

                    if (donViPhoiHop == null) return false;

                    var vb = await dbContext.VanBanChiDao.FirstOrDefaultAsync(x => x.Id == data.IdVanBanChiDao);

                    if (vb == null) return false;

                    vb.TrangThai = data.TrangThai;

                    await dbContext.SaveChangesAsync();

                    var thph = new TinhHinhPhoiHop
                    {
                        IdDonViPhoiHop = donViPhoiHop.Id,
                        NgayXuLy = data.NgayBaoCao,
                        UserId = data.UserId,
                        NoiDungThucHien = data.NoiDungBaoCao
                    };

                    dbContext.TinhHinhPhoiHop.Add(thph);

                    await dbContext.SaveChangesAsync();

                    if (string.IsNullOrWhiteSpace(data.FileDinhKem)
                        || string.IsNullOrWhiteSpace(data.FileUrl))
                    {
                        dbTransaction.Commit();
                        return true;
                    }

                    dbContext.FileTinhHinhPhoiHop.Add(new FileTinhHinhPhoiHop
                    {
                        IdTinhHinhPhoiHop = thph.Id,
                        Name = data.FileDinhKem,
                        Url = data.FileUrl
                    });

                    await dbContext.SaveChangesAsync();

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
            var dbContext = new MainDbContext();

            using (var dbTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var fileDinhKem =
                        dbContext.FileTinhHinhPhoiHop.Where(x => x.IdTinhHinhPhoiHop == id);

                    dbContext.FileTinhHinhPhoiHop.RemoveRange(fileDinhKem);

                    await dbContext.SaveChangesAsync();

                    var thth = dbContext.TinhHinhPhoiHop.Where(x => x.Id == id);

                    dbContext.TinhHinhPhoiHop.RemoveRange(thth);

                    await dbContext.SaveChangesAsync();

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