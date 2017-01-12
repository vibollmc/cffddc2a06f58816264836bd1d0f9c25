using System;
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
        private static TinhHinhThucHienManagement _instance;
        public static TinhHinhThucHienManagement Go
        {
            get
            {
                if(_instance == null) _instance = new TinhHinhThucHienManagement();
                return _instance;
            }
        }

        public async Task<IEnumerable<TinhHinhThucHien>> GetTinhHinhThucHien(int? idVanBanChiDao)
        {
            var dbContext = new MainDbContext();

            return
                await
                    dbContext.TinhHinhThucHien.Where(x => x.IdVanBanChiDao == idVanBanChiDao)
                        .Include(x => x.VanBanChiDao)
                        .OrderByDescending(x => x.NgayBaoCao)
                        .ToListAsync();
        }

        public async Task<bool> SaveTinhHinhThucHien(TinhHinhThucHienInput data)
        {
            var dbContext = new MainDbContext();

            using (var dbTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    //cap nhat trang thai dang xu ly cho vanban
                   
                    var vb = await dbContext.VanBanChiDao.FirstOrDefaultAsync(
                        x => 
                            x.Id == data.IdVanBanChiDao &&
                            (
                                (data.TrangThai == TrangThaiVanBan.DangXuLy && x.TrangThai != TrangThaiVanBan.DangXuLy)
                                ||
                                (data.TrangThai == TrangThaiVanBan.HoanThanh && x.TrangThai != TrangThaiVanBan.HoanThanh)
                            ));

                    if (vb != null)
                    {
                        vb.TrangThai = data.TrangThai;
                        if (vb.TrangThai == TrangThaiVanBan.HoanThanh)
                            vb.NgayHoanThanh = DateTime.Now;
                        
                        await dbContext.SaveChangesAsync();
                    }

                    var thth = data.Transform();
                    dbContext.TinhHinhThucHien.Add(thth);

                    await dbContext.SaveChangesAsync();

                    if (string.IsNullOrWhiteSpace(data.FileDinhKem)
                        || string.IsNullOrWhiteSpace(data.FileUrl))
                    {
                        dbTransaction.Commit();
                        return true;
                    }

                    dbContext.FileTinhHinhThucHien.Add(new FileTinhHinhThucHien
                    {
                        IdTinhHinhThucHien = thth.Id,
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

        public async Task<bool> DeleteTinhHinhThucHien(int? id)
        {
            var dbContext = new MainDbContext();
            using (var dbTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var fileDinhKem =
                        dbContext.FileTinhHinhThucHien.Where(x => x.IdTinhHinhThucHien == id);

                    dbContext.FileTinhHinhThucHien.RemoveRange(fileDinhKem);

                    await dbContext.SaveChangesAsync();

                    var thth = dbContext.TinhHinhThucHien.Where(x => x.Id == id);

                    dbContext.TinhHinhThucHien.RemoveRange(thth);

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