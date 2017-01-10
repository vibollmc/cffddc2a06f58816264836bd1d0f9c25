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
        private readonly QLVBDatabase _qlvbDatabase;
        private readonly MainDbContext _dbContext;
        public SyncDataFromQlvb()
        {
            this._qlvbDatabase = new QLVBDatabase();
            this._dbContext = new MainDbContext();
        }

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
            var connectionString = WebConfigurationManager.ConnectionStrings["QLVBDatabase"].ToString();

            _dbContext.MarkedDatabaseChange.RemoveRange(_dbContext.MarkedDatabaseChange);

            await _dbContext.SaveChangesAsync();

            _dbContext.MarkedDatabaseChange.Add(new MarkedDatabaseChange
            {
                ConnectionString = connectionString,
                IsSyncData = true
            });

            await _dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> CheckSynced()
        {
            var connectionString = WebConfigurationManager.ConnectionStrings["QLVBDatabase"].ToString();

            var checkSync = await 
                _dbContext.MarkedDatabaseChange.FirstOrDefaultAsync(
                    x => x.ConnectionString.ToLower() == connectionString.ToLower());


            return checkSync != null && checkSync.IsSyncData;
        }


        private async Task<bool> DeleteExistsData()
        {
            _dbContext.FileTinhHinhPhoiHop.RemoveRange(_dbContext.FileTinhHinhPhoiHop);
            await _dbContext.SaveChangesAsync();

            _dbContext.FileTinhHinhThucHien.RemoveRange(_dbContext.FileTinhHinhThucHien);
            await _dbContext.SaveChangesAsync();

            _dbContext.FileVanBanChiDao.RemoveRange(_dbContext.FileVanBanChiDao);
            await _dbContext.SaveChangesAsync();

            _dbContext.TinhHinhPhoiHop.RemoveRange(_dbContext.TinhHinhPhoiHop);
            await _dbContext.SaveChangesAsync();

            _dbContext.TinhHinhThucHien.RemoveRange(_dbContext.TinhHinhThucHien);
            await _dbContext.SaveChangesAsync();

            _dbContext.DonViPhoiHop.RemoveRange(_dbContext.DonViPhoiHop);
            await _dbContext.SaveChangesAsync();

            _dbContext.VanBanChiDao.RemoveRange(_dbContext.VanBanChiDao);
            await _dbContext.SaveChangesAsync();

            _dbContext.DangNhap.RemoveRange(_dbContext.DangNhap);
            await _dbContext.SaveChangesAsync();

            _dbContext.DonViTrucThuoc.RemoveRange(_dbContext.DonViTrucThuoc);
            await _dbContext.SaveChangesAsync();

            _dbContext.DonVi.RemoveRange(_dbContext.DonVi);
            await _dbContext.SaveChangesAsync();

            _dbContext.Khoi.RemoveRange(_dbContext.Khoi);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> SyncKhoiPhatHanh()
        {
            var khoiPhatHanh = this._qlvbDatabase.Khoiphathanhs.Select(x => new Khoi
            {
                Id = x.intid,
                KyHieu = x.strkyhieu,
                Ten = x.strtenkhoi,
                TrangThai = (TrangThai) x.inttrangthai,
                MacDinh = x.IsDefault
            });

            _dbContext.Khoi.AddRange(khoiPhatHanh);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> SyncDonVi()
        {
            var donvi = this._qlvbDatabase.Tochucdoitacs.Select(x => new DonVi
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

            _dbContext.DonVi.AddRange(donvi);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> SyncDonViTrucThuoc()
        {
            var donviTructhuoc = this._qlvbDatabase.Donvitructhuocs.Select(x => new DonViTrucThuoc
            {
                Id = x.Id,
                Level = x.intlevel,
                Ma = x.strmadonvi,
                Ten = x.strtendonvi,
                TrangThai = (TrangThai) x.inttrangthai,
                ParentId = x.ParentId
            });

            _dbContext.DonViTrucThuoc.AddRange(donviTructhuoc);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> SyncCanBo()
        {
            var canbo = await this._qlvbDatabase.Canbos
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

            _dbContext.DangNhap.AddRange(canbo);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}