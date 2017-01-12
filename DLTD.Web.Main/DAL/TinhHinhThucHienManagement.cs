using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DLTD.Web.Main.Common;
using DLTD.Web.Main.Models;
using DLTD.Web.Main.Models.MetaData;
using DLTD.Web.Main.Models.Enum;

namespace DLTD.Web.Main.DAL
{
    public class TinhHinhThucHienManagement
    {
        private readonly MainDbContext _dbContext;

        public static TinhHinhThucHienManagement Go
        {
            get { return new TinhHinhThucHienManagement(); }
        }

        public TinhHinhThucHienManagement()
        {
            _dbContext = new MainDbContext();
        }

        public async Task<IEnumerable<TinhHinhThucHien>> GetTinhHinhThucHien(int? idVanBanChiDao)
        {
            return
                await
                    _dbContext.TinhHinhThucHien.Where(x => x.IdVanBanChiDao == idVanBanChiDao)
                        .Include(x => x.VanBanChiDao)
                        .OrderByDescending(x => x.NgayBaoCao)
                        .ToListAsync();
        }

        public async Task<bool> SaveTinhHinhThucHien(TinhHinhThucHienInput data)
        {
            using (var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    //cap nhat trang thai dang xu ly cho vanban
                   
                    var vb = await this._dbContext.VanBanChiDao.FirstOrDefaultAsync(
                        x => 
                            x.Id == data.IdVanBanChiDao &&
                            x.TrangThai != TrangThaiVanBan.DangXuLy &&
                            x.TrangThai != TrangThaiVanBan.HoanThanh);

                    if (vb != null)
                    {
                        vb.TrangThai = data.TrangThai;
                        await _dbContext.SaveChangesAsync();
                    }

                    var thth = data.Transform();
                    _dbContext.TinhHinhThucHien.Add(thth);

                    await _dbContext.SaveChangesAsync();

                    if (string.IsNullOrWhiteSpace(data.FileDinhKem)
                        || string.IsNullOrWhiteSpace(data.FileUrl))
                    {
                        dbTransaction.Commit();
                        return true;
                    }

                    _dbContext.FileTinhHinhThucHien.Add(new FileTinhHinhThucHien
                    {
                        IdTinhHinhThucHien = thth.Id,
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

        public async Task<bool> DeleteTinhHinhThucHien(int? id)
        {
            using (var dbTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var fileDinhKem =
                        _dbContext.FileTinhHinhThucHien.Where(x => x.IdTinhHinhThucHien == id);

                    _dbContext.FileTinhHinhThucHien.RemoveRange(fileDinhKem);

                    await _dbContext.SaveChangesAsync();

                    var thth = _dbContext.TinhHinhThucHien.Where(x => x.Id == id);

                    _dbContext.TinhHinhThucHien.RemoveRange(thth);

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