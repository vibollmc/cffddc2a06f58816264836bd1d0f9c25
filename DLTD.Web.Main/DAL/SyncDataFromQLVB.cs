using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using DLTD.Web.Main.Models;
using DLTD.Web.Main.Models.Enum;
using Microsoft.Ajax.Utilities;

namespace DLTD.Web.Main.DAL
{
    public class SyncDataFromQlvb
    {
        public static async Task<bool> Sync()
        {
            var syncDataFromQlvb = new SyncDataFromQlvb();


            if (!await syncDataFromQlvb.CheckSynced())
            {
                await syncDataFromQlvb.DeleteExistsData();

                await syncDataFromQlvb.SyncKhoiPhatHanh();

                await syncDataFromQlvb.SyncDonVi();

                await syncDataFromQlvb.SyncDonViTrucThuoc();

                await syncDataFromQlvb.SyncCanBo();

                await syncDataFromQlvb.MarkedSync();
            }

            return true;
        }

        private async Task<bool> MarkedSync()
        {
            var dbContext = new MainDbContext();

            var connectionString = WebConfigurationManager.ConnectionStrings["QLVBDatabase"].ToString();

            dbContext.MarkedDatabaseChange.RemoveRange(dbContext.MarkedDatabaseChange);

            await dbContext.SaveChangesAsync();

            dbContext.MarkedDatabaseChange.Add(new MarkedDatabaseChange
            {
                ConnectionString = connectionString,
                IsSyncData = true
            });

            await dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> CheckSynced()
        {
            var connectionString = WebConfigurationManager.ConnectionStrings["QLVBDatabase"].ToString();
            var dbContext = new MainDbContext();
            var checkSync = await 
                dbContext.MarkedDatabaseChange.FirstOrDefaultAsync(
                    x => x.ConnectionString.ToLower() == connectionString.ToLower());


            return checkSync != null && checkSync.IsSyncData;
        }


        private async Task<bool> DeleteExistsData()
        {
            var dbContext = new MainDbContext();

            dbContext.FileTinhHinhPhoiHop.RemoveRange(dbContext.FileTinhHinhPhoiHop);
            await dbContext.SaveChangesAsync();

            dbContext.FileTinhHinhThucHien.RemoveRange(dbContext.FileTinhHinhThucHien);
            await dbContext.SaveChangesAsync();

            dbContext.FileVanBanChiDao.RemoveRange(dbContext.FileVanBanChiDao);
            await dbContext.SaveChangesAsync();

            dbContext.TinhHinhPhoiHop.RemoveRange(dbContext.TinhHinhPhoiHop);
            await dbContext.SaveChangesAsync();

            dbContext.TinhHinhThucHien.RemoveRange(dbContext.TinhHinhThucHien);
            await dbContext.SaveChangesAsync();

            dbContext.DonViPhoiHop.RemoveRange(dbContext.DonViPhoiHop);
            await dbContext.SaveChangesAsync();

            dbContext.VanBanChiDao.RemoveRange(dbContext.VanBanChiDao);
            await dbContext.SaveChangesAsync();

            dbContext.DangNhap.RemoveRange(dbContext.DangNhap);
            await dbContext.SaveChangesAsync();

            dbContext.DonViTrucThuoc.RemoveRange(dbContext.DonViTrucThuoc);
            await dbContext.SaveChangesAsync();

            dbContext.DonVi.RemoveRange(dbContext.DonVi);
            await dbContext.SaveChangesAsync();

            dbContext.Khoi.RemoveRange(dbContext.Khoi);
            await dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> SyncKhoiPhatHanh()
        {
            var qlvbDatabase = new QlvbDatabase();
            var dbContext = new MainDbContext();
            var khoiPhatHanh = qlvbDatabase.Khoiphathanhs.Select(x => new Khoi
            {
                Id = x.intid,
                KyHieu = x.strkyhieu,
                Ten = x.strtenkhoi,
                TrangThai = (TrangThai) x.inttrangthai,
                MacDinh = x.IsDefault
            });

            dbContext.Khoi.AddRange(khoiPhatHanh);
            await dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> SyncDonVi()
        {
            var qlvbDatabase = new QlvbDatabase();
            var dbContext = new MainDbContext();

            var donvi = qlvbDatabase.Tochucdoitacs.Select(x => new DonVi
            {
                Id = x.intid,
                IdKhoi = x.intidkhoi,
                Ma = x.strmatochucdoitac,
                Ten = x.strtentochucdoitac,
                DiaChi = x.strdiachi,
                DienThoai = x.strphone,
                Email = x.stremail,
                EmailVbdt = x.stremailvbdt,
                TrangThai = (TrangThai) x.inttrangthai,
                Fax = x.strfax
            });

            dbContext.DonVi.AddRange(donvi);
            await dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> SyncDonViTrucThuoc()
        {
            var qlvbDatabase = new QlvbDatabase();
            var dbContext = new MainDbContext();

            var donviTructhuoc = qlvbDatabase.Donvitructhuocs.Select(x => new DonViTrucThuoc
            {
                Id = x.Id,
                Level = x.intlevel,
                Ma = x.strmadonvi,
                Ten = x.strtendonvi,
                TrangThai = (TrangThai) x.inttrangthai,
                ParentId = x.ParentId
            });

            dbContext.DonViTrucThuoc.AddRange(donviTructhuoc);
            await dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> SyncCanBo()
        {
            var qlvbDatabase = new QlvbDatabase();
            var dbContext = new MainDbContext();

            var canbo = await qlvbDatabase.Canbos
                .Where(x => !string.IsNullOrEmpty(x.strusername) && x.inttrangthai == 1)
                .Select(x => new DangNhap
                {
                    TrangThai = (TrangThai) x.inttrangthai,
                    Ten = x.strhoten,
                    Ma = x.strmacanbo,
                    DangNhapTu = LoaiQlvb.Qlvb,
                    Email = x.stremail,
                    GioiTinh = (GioiTinh) x.intgioitinh,
                    IdDonVi = x.intdonvi,
                    MatKhau = x.strpassword,
                    NgaySinh = x.strngaysinh,
                    NhomNguoiDung = (NhomNguoiDung) x.intnhomquyen,
                    SoDienThoai = x.strdienthoai,
                    TenDangNhap = x.strusername,
                    UrlImage = x.strImageProfile
                }).ToListAsync();

            dbContext.DangNhap.AddRange(canbo);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}