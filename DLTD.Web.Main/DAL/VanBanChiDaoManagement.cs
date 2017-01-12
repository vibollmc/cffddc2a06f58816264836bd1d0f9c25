using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DLTD.Web.Main.Common;
using DLTD.Web.Main.Models;
using DLTD.Web.Main.Models.Enum;
using DLTD.Web.Main.Models.MetaData;
using DLTD.Web.Main.ViewModels;
using DLTD.Web.Main.Controllers;

namespace DLTD.Web.Main.DAL
{
    public class VanBanChiDaoManagement
    {
        private static VanBanChiDaoManagement _instance;
        public static VanBanChiDaoManagement Go
        {
            get
            {
                if(_instance ==null) _instance= new VanBanChiDaoManagement();
                return _instance;
            }
        }

        public async Task<bool> SaveVanBanChiDaoFromApi(VanBanChiDaoInput vanBan)
        {
            var dbContext = new MainDbContext();
            using (var dbTransaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var vanBanChiDao = vanBan.Transform();
                    dbContext.VanBanChiDao.Add(vanBanChiDao);
                    await dbContext.SaveChangesAsync();

                    if (!string.IsNullOrWhiteSpace(vanBan.DonViPhoiHop))
                    {
                        var arrIdDonVi = vanBan.DonViPhoiHop.Split(',');
                        foreach (var s in arrIdDonVi)
                        {
                            dbContext.DonViPhoiHop.Add(new DonViPhoiHop
                            {
                                TrangThai = TrangThai.Active,
                                IdDonvi = s.ToIntExt(),
                                IdVanBan = vanBanChiDao.Id
                            });
                        }

                        await dbContext.SaveChangesAsync();
                    }


                    if (vanBan.FileDinhKem == null ||
                        vanBan.FileDinhKem.Count == 0)
                    {
                        dbTransaction.Commit();
                        return true;
                    }


                    foreach (var file in vanBan.FileDinhKem)
                    {
                        var fileDinhKem = new FileVanBanChiDao
                        {
                            Name = file.TenFile,
                            Url = file.UrlFile,
                            IdVanBanChiDao = vanBanChiDao.Id
                        };
                        dbContext.FileVanBanChiDao.Add(fileDinhKem);
                    }

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

        public async Task<IEnumerable<VanBanChiDao>> GetVanBanChiDao(int? userId, NhomNguoiDung nhomNguoiDung, SearchVanBanModel searchObj)
        {
            TrangThaiVanBan? trangthai = TrangThaiVanBan.Undefined;
            if (searchObj != null) trangthai = searchObj.TrangThai;

            var dbContext = new MainDbContext();

            var data =
                    dbContext.VanBanChiDao.Where(
                        x =>
                                (
                                    (trangthai == TrangThaiVanBan.TraLai && x.IsTralai == true)
                                        ||
                                    (trangthai == TrangThaiVanBan.HoanThanh && x.NgayHoanThanh != null)
                                        ||
                                    (trangthai == TrangThaiVanBan.Moi && x.NgayHoanThanh == null &&
                                        x.ThoiHanXuLy >= DateTime.Now && x.TrangThai != TrangThaiVanBan.DangXuLy)
                                        ||
                                    (trangthai == TrangThaiVanBan.DangXuLy && x.NgayHoanThanh == null &&
                                        x.ThoiHanXuLy >= DateTime.Now && x.TrangThai == TrangThaiVanBan.DangXuLy)
                                        ||
                                    (trangthai == TrangThaiVanBan.QuaHan && x.NgayHoanThanh == null && x.ThoiHanXuLy != null &&
                                        x.ThoiHanXuLy < DateTime.Now)
                                        || 
                                    (trangthai == TrangThaiVanBan.Undefined)
                                )
                                &&
                                (x.UserId == userId || x.IdNguoiChiDao == userId || x.IdNguoiTheoDoi == userId || nhomNguoiDung == NhomNguoiDung.QuanTriHeThong)
                            )
                        .OrderBy(x => x.IdVanBan).ThenByDescending(x => x.NgayTao)
                        .Include(x => x.DonVi)
                        .Include(x => x.NguoiTheoDoi)
                        .Include(x => x.FileDinhKem);

            if (searchObj != null && searchObj.IsNormalSearch)
            {
                var search = searchObj.SearchText.ToLower();
                var dateSearch = searchObj.SearchText.ToDateTimeExt();

                data = data.Where(x => x.SoKH.ToLower().Contains(search) ||
                                       x.Trichyeu.ToLower().Contains(search) ||
                                       x.DonVi.Ten.ToLower().Contains(search) ||
                                       x.NguoiTheoDoi.Ten.ToLower().Contains(search) ||
                                       x.YKienChiDao.ToLower().Contains(search) ||
                                       x.Ngayky == dateSearch ||
                                       x.ThoiHanXuLy == dateSearch
                    );
            }
            else if (searchObj != null && searchObj.IsAdvanceSearch)
            {
                var ngaykyTu = searchObj.NgayKyTu.ToDateTimeExt();
                var ngaykyDen = searchObj.NgayKyDen.ToDateTimeExt();
                var donviXuly = searchObj.DonViXuLy.ToIntExt();
                var nguoiChiDao = searchObj.NguoiChiDao.ToIntExt();
                var nguoiTheoDoi = searchObj.NguoiTheoDoi.ToIntExt();
                var thoihanXlTu = searchObj.ThoiHanXuLyTu.ToDateTimeExt();
                var thoihanXlDen = searchObj.ThoiHanXuLyDen.ToDateTimeExt();
                var doUuTien = searchObj.DoUuTien.ToIntExt();
                var donviPhoiHop = searchObj.DonViPhoiHop.ToIntExt();
                var nguonChiDao = searchObj.NguonChiDao.ToIntExt();

                data = data.Where(
                        x =>
                        (!ngaykyTu.HasValue || x.Ngayky >= ngaykyTu)
                        &&
                        (!ngaykyDen.HasValue || x.Ngayky <= ngaykyDen)
                        &&
                        (searchObj.KyHieu == null  || searchObj.KyHieu.Trim() == string.Empty || x.SoKH.ToLower().Contains(searchObj.KyHieu.ToLower()))
                        &&
                        (!donviXuly.HasValue || x.IdDonVi == donviXuly)
                        &&
                        (!nguoiChiDao.HasValue || x.IdNguoiChiDao == nguoiChiDao)
                        &&
                        (!nguoiTheoDoi.HasValue || x.IdNguoiTheoDoi == nguoiTheoDoi)
                        &&
                        (!thoihanXlTu.HasValue || !x.ThoiHanXuLy.HasValue || x.ThoiHanXuLy >= thoihanXlTu)
                        &&
                        (!thoihanXlDen.HasValue || !x.ThoiHanXuLy.HasValue || x.ThoiHanXuLy <= thoihanXlDen)
                        &&
                        (!donviPhoiHop.HasValue || x.DonViPhoihop.Any(y => y.IdDonvi == donviPhoiHop))
                        &&
                        (!nguonChiDao.HasValue || x.IdNguonChiDao == nguonChiDao)
                        &&
                        (searchObj.NoiDung == null || searchObj.NoiDung.Trim() == string.Empty || x.Trichyeu.ToLower().Contains(searchObj.NoiDung.ToLower()))
                        &&
                        (searchObj.YKienChiDao == null || searchObj.YKienChiDao.Trim() == string.Empty || x.YKienChiDao.ToLower().Contains(searchObj.YKienChiDao.ToLower()))
                    );

                if (doUuTien.HasValue)
                    data = data.Where(
                        x => x.DoKhan == (DoKhan)doUuTien.Value);
            }

            return await data.ToListAsync();
            
        }

        public async Task<VanBanChiDao> GetVanBanChiDaoById(int? id)
        {
            var dbContext = new MainDbContext();
            return await dbContext.VanBanChiDao.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<VanBanChiDao>> GetVanBanChiDao(
           int? userId,
            DateTime? tuNgay, 
            DateTime? denNgay)
        {
            var dbContext = new MainDbContext();
            var data = await dbContext.VanBanChiDao.Where(x => (tuNgay == null || x.NgayTao >= tuNgay)&&(denNgay==null||x.NgayTao<=denNgay))
                .Include(x => x.DonVi)
                .Include(x => x.NguoiGui)
                .Include(x => x.FileDinhKem)
                .ToListAsync();
            return data;
        }
        public async Task<IEnumerable<VanBanChiDao>> ThongKeVanBanChiDao(DateTime? tuNgay, DateTime? denNgay, int? idKhoi, int? idDonvi, int? idnguoitheodoi, int?idnguoichidao)
        {
            var dbContext = new MainDbContext();
            var data = await dbContext.VanBanChiDao.Where(x => (tuNgay == null || x.NgayTao >= tuNgay)
                && (denNgay == null || x.NgayTao <= denNgay) && (idKhoi == null || x.IdNguonChiDao== idKhoi)
                && (idDonvi == null || x.IdDonVi == idDonvi)&&(idnguoitheodoi==null)||(x.IdNguoiTheoDoi==idnguoitheodoi)
                &&(idnguoichidao==null)||(x.IdNguoiChiDao==idnguoichidao))
                .Include(x => x.DonVi)
                .Include(x => x.NguoiGui)
                .Include(x => x.FileDinhKem)
                .ToListAsync();
            return data;
        }
        public async Task<bool> UpdateCompleteVanBan(int? id)
        {
            var dbContext = new MainDbContext();
            var vanban = await dbContext.VanBanChiDao.SingleOrDefaultAsync(x => x.Id == id);

            vanban.TrangThai = TrangThaiVanBan.HoanThanh;
            vanban.NgayHoanThanh = DateTime.Now;

            await dbContext.SaveChangesAsync();
            return true;
        }
        public IList<FileDinhKemViewModel> GetFileVanBanChiDao(int idVanBan)
        {
            var dbContext = new MainDbContext();
            return dbContext.FileVanBanChiDao.Where(x => x.IdVanBanChiDao == idVanBan)
                        .Select(x => new FileDinhKemViewModel
                        {
                            FileName = x.Name,
                            FileUrl = x.Url
                        }).ToList();
        }

        //public IList<TinhHinhXuLyViewModel> ThongKeVanBanTheoNguonChiDao(
        //    int? userId,            
        //    int? idKhoi,
        //    TrangThaiVanBan? trangthai)
        //{
        //    var nguonChiDao = _dbContext.Khoi.Where(x => x.NguonChiDao == true && (idKhoi == null || x.Id == idKhoi))
        //        .Include(
        //            x => x.VanBanChiDao);
        //    var results = new List<TinhHinhXuLyViewModel>();
        //    foreach (var item in nguonChiDao)
        //    {
        //        var obj = new TinhHinhXuLyViewModel();

        //        var vanBanChiDao =
        //            item.VanBanChiDao.Where(
        //                y =>    (trangthai == TrangThaiVanBan.HoanThanh && y.NgayHoanThanh != null)
        //                         ||
        //                         (trangthai == TrangThaiVanBan.Moi && y.NgayHoanThanh == null &&
        //                          y.ThoiHanXuLy >= DateTime.Now && y.TrangThai != TrangThaiVanBan.DangXuLy)
        //                         ||
        //                        (trangthai == TrangThaiVanBan.DangXuLy && y.NgayHoanThanh == null &&
        //                          y.ThoiHanXuLy >= DateTime.Now && y.TrangThai == TrangThaiVanBan.DangXuLy)
        //                         ||
        //                         (trangthai == TrangThaiVanBan.QuaHan && y.NgayHoanThanh == null && y.ThoiHanXuLy != null &&
        //                          y.ThoiHanXuLy < DateTime.Now)
        //                         || (trangthai == TrangThaiVanBan.Undefined));

        //        obj.Name = item.Ten;
        //        obj.Id = item.Id;

        //        var sum = vanBanChiDao.Count();
        //        var sumDangXL =
        //            vanBanChiDao.Count(
        //                x =>
        //                    x.TrangThai != TrangThaiVanBan.HoanThanh &&
        //                    (x.ThoiHanXuLy == null || x.ThoiHanXuLy <= DateTime.Today));
        //        var sumDangXLQuaHan =
        //            vanBanChiDao.Count(
        //                x =>
        //                    x.TrangThai != TrangThaiVanBan.HoanThanh &&
        //                    (x.ThoiHanXuLy != null && x.ThoiHanXuLy > DateTime.Today));

        //        var sumHT =
        //            vanBanChiDao.Count(
        //                x =>
        //                    x.TrangThai == TrangThaiVanBan.HoanThanh &&
        //                    (x.ThoiHanXuLy == null || x.ThoiHanXuLy >= x.NgayHoanThanh));
        //        var sumHTQuaHan =
        //            vanBanChiDao.Count(
        //                x =>
        //                    x.TrangThai == TrangThaiVanBan.HoanThanh &&
        //                    (x.ThoiHanXuLy != null && x.ThoiHanXuLy < x.NgayHoanThanh));

        //        obj.HoanThanh = sumHT;
        //        obj.DangThucHien = sumDangXL;
        //        obj.QuaHan = sumDangXLQuaHan;
        //        obj.HoanThanhQuaHan = sumHTQuaHan;
        //        obj.TongNhiemVu = sum;

        //        results.Add(obj);
        //    }

        //    return results;
        //}
        public IList<TinhHinhXuLyViewModel> ThongKeVanBanTheoNguonChiDao(
          int? userId,
          DateTime? tuNgay,
          DateTime? denNgay,
          int? idKhoi,
          TrangThaiVanBan? trangthai)
        {
            var dbContext = new MainDbContext();
            var nguonChiDao = dbContext.Khoi.Where(x => x.NguonChiDao == true && (idKhoi == null || x.Id == idKhoi))
                .Include(
                    x => x.VanBanChiDao);
            var results = new List<TinhHinhXuLyViewModel>();
            foreach (var item in nguonChiDao)
            {
                var obj = new TinhHinhXuLyViewModel();

                var vanBanChiDao =
                    item.VanBanChiDao.Where(
                        y => (y.NgayTao >= tuNgay || tuNgay == null) && (y.NgayTao <= denNgay || denNgay == null) && (trangthai == TrangThaiVanBan.HoanThanh && y.NgayHoanThanh != null)
                                 ||
                                 (trangthai == TrangThaiVanBan.Moi && y.NgayHoanThanh == null &&
                                  y.ThoiHanXuLy >= DateTime.Now && y.TrangThai != TrangThaiVanBan.DangXuLy)
                                 ||
                                (trangthai == TrangThaiVanBan.DangXuLy && y.NgayHoanThanh == null &&
                                  y.ThoiHanXuLy >= DateTime.Now && y.TrangThai == TrangThaiVanBan.DangXuLy)
                                 ||
                                 (trangthai == TrangThaiVanBan.QuaHan && y.NgayHoanThanh == null && y.ThoiHanXuLy != null &&
                                  y.ThoiHanXuLy < DateTime.Now)
                                 || (trangthai == TrangThaiVanBan.Undefined));

                if (!vanBanChiDao.Any()) continue;

                obj.Name = item.Ten;
                obj.Id = item.Id;

                var sum = vanBanChiDao.Count();
                var sumDangXl =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai != TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy == null || x.ThoiHanXuLy <= DateTime.Today));
                var sumDangXlQuaHan =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai != TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy != null && x.ThoiHanXuLy > DateTime.Today));

                var sumHt =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai == TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy == null || x.ThoiHanXuLy >= x.NgayHoanThanh));
                var sumHtQuaHan =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai == TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy != null && x.ThoiHanXuLy < x.NgayHoanThanh));

                obj.HoanThanh = sumHt;
                obj.DangThucHien = sumDangXl;
                obj.QuaHan = sumDangXlQuaHan;
                obj.HoanThanhQuaHan = sumHtQuaHan;
                obj.TongNhiemVu = sum;

                results.Add(obj);
            }

            return results;
        }

        public IList<TinhHinhXuLyViewModel> ThongKeVanBanTheoDonViXuLy(
            int? userId,
            DateTime? tuNgay, 
            DateTime? denNgay, 
            int? idKhoiDonVi, 
            int? idDonVi)
        {
            var dbContext = new MainDbContext();
            var results = new List<TinhHinhXuLyViewModel>();
            var lstKhoi = new List<int> {6, 7, 9};
            var donVi = dbContext.DonVi.Where(x => x.TrangThai == TrangThai.Active 
                && lstKhoi.Contains(x.IdKhoi.Value)
                && (idKhoiDonVi == null || x.Khoi.Id == idKhoiDonVi)
                && (idDonVi == null || x.Id == idDonVi))
                .Include(x=>x.VanBanChiDao);

            foreach (var item in donVi)
            {
                var obj = new TinhHinhXuLyViewModel();

                var vanBanChiDao =
                    item.VanBanChiDao.Where(
                        y => (y.NgayTao >= tuNgay || tuNgay == null) && (y.NgayTao <= denNgay || denNgay == null));

                if (!vanBanChiDao.Any()) continue;

                obj.Name = item.Ten;
                obj.Id = item.Id;

                var sum = vanBanChiDao.Count();
                var sumDangXl =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai != TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy == null || x.ThoiHanXuLy <= DateTime.Today));
                var sumDangXlQuaHan =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai != TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy != null && x.ThoiHanXuLy > DateTime.Today));

                var sumHt =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai == TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy == null || x.ThoiHanXuLy >= x.NgayHoanThanh));
                var sumHtQuaHan =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai == TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy != null && x.ThoiHanXuLy < x.NgayHoanThanh));

                obj.HoanThanh = sumHt;
                obj.DangThucHien = sumDangXl;
                obj.QuaHan = sumDangXlQuaHan;
                obj.HoanThanhQuaHan = sumHtQuaHan;
                obj.TongNhiemVu = sum;

                results.Add(obj);
            }

            return results;
        }
        public IList<TinhHinhXuLyViewModel> ThongKeVanBanTheoNguoiTheoDoi(
            int? userId,
            DateTime? tuNgay, 
            DateTime? denNgay, 
            int? idnguoitheodoi)
        {
            var dbContext = new MainDbContext();
            var results = new List<TinhHinhXuLyViewModel>();
            var nguoitheodoi = dbContext.DangNhap.Where(
                x => x.NhomNguoiDung == NhomNguoiDung.ChuyenVien && (idnguoitheodoi == null || x.Id == idnguoitheodoi))
                .Include(x => x.VanBanChiDao);

            foreach (var item in nguoitheodoi)
            {
                var obj = new TinhHinhXuLyViewModel();

                var vanBanChiDao =
                    item.VanBanChiDao.Where(
                        y => (y.NgayTao >= tuNgay || tuNgay == null) && (y.NgayTao <= denNgay || denNgay == null));

                if (!vanBanChiDao.Any()) continue;

                obj.Name = item.Ten;
                obj.Id = item.Id;

                var sum = vanBanChiDao.Count();
                var sumDangXl =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai != TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy == null || x.ThoiHanXuLy <= DateTime.Today));
                var sumDangXlQuaHan =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai != TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy != null && x.ThoiHanXuLy > DateTime.Today));

                var sumHt =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai == TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy == null || x.ThoiHanXuLy >= x.NgayHoanThanh));
                var sumHtQuaHan =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai == TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy != null && x.ThoiHanXuLy < x.NgayHoanThanh));

                obj.HoanThanh = sumHt;
                obj.DangThucHien = sumDangXl;
                obj.QuaHan = sumDangXlQuaHan;
                obj.HoanThanhQuaHan = sumHtQuaHan;
                obj.TongNhiemVu = sum;

                results.Add(obj);
            }

            return results;
        }
        public IList<TinhHinhXuLyViewModel> ThongKeVanBanTheoNguoiChiDao(
            int? userId,
            DateTime? tuNgay, 
            DateTime? denNgay, 
            int? idnguoichidao)
        {
            var dbContext = new MainDbContext();
            var results = new List<TinhHinhXuLyViewModel>();
            var nguoichidao = dbContext.DangNhap.Where(
                x => x.NhomNguoiDung == NhomNguoiDung.LanhDao && (idnguoichidao == null || x.Id == idnguoichidao))
                .Include(x => x.VanBanChiDao);

            foreach (var item in nguoichidao)
            {
                var obj = new TinhHinhXuLyViewModel();

                var vanBanChiDao =
                    item.VanBanChiDao.Where(
                        y => (y.NgayTao >= tuNgay || tuNgay == null) && (y.NgayTao <= denNgay || denNgay == null));

                if (!vanBanChiDao.Any()) continue;

                obj.Name = item.Ten;
                obj.Id = item.Id;

                var sum = vanBanChiDao.Count();
                var sumDangXl =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai != TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy == null || x.ThoiHanXuLy <= DateTime.Today));
                var sumDangXlQuaHan =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai != TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy != null && x.ThoiHanXuLy > DateTime.Today));

                var sumHt =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai == TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy == null || x.ThoiHanXuLy >= x.NgayHoanThanh));
                var sumHtQuaHan =
                    vanBanChiDao.Count(
                        x =>
                            x.TrangThai == TrangThaiVanBan.HoanThanh &&
                            (x.ThoiHanXuLy != null && x.ThoiHanXuLy < x.NgayHoanThanh));

                obj.HoanThanh = sumHt;
                obj.DangThucHien = sumDangXl;
                obj.QuaHan = sumDangXlQuaHan;
                obj.HoanThanhQuaHan = sumHtQuaHan;
                obj.TongNhiemVu = sum;

                results.Add(obj);
            }

            return results;
        }

        public async Task<bool> DeleteVanBan(int id)
        {
            var dbContext = new MainDbContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var vb = await dbContext.VanBanChiDao.FirstOrDefaultAsync(x => x.Id == id);
                    if (vb == null) return true;

                    var donviPhoiHop = vb.DonViPhoihop;

                    if (donviPhoiHop != null && donviPhoiHop.Count > 0)
                    {
                        foreach (var donvi in donviPhoiHop)
                        {
                            var tinhhinhPhoiHop = donvi.TinhHinhPhoiHop;
                            if (tinhhinhPhoiHop != null && tinhhinhPhoiHop.Count > 0)
                            {
                                foreach (var phoihop in tinhhinhPhoiHop)
                                {
                                    var file = phoihop.FileDinhKem;
                                    if (file != null && file.Count > 0)
                                    {
                                        dbContext.FileTinhHinhPhoiHop.RemoveRange(file);
                                        await dbContext.SaveChangesAsync();
                                    }
                                }
                                dbContext.TinhHinhPhoiHop.RemoveRange(tinhhinhPhoiHop);
                                await dbContext.SaveChangesAsync();
                            }
                        }
                        dbContext.DonViPhoiHop.RemoveRange(donviPhoiHop);
                        await dbContext.SaveChangesAsync();

                    }

                    var tinhhinhThucHien = vb.TinhHinhThucHien;
                    if (tinhhinhThucHien != null && tinhhinhThucHien.Count > 0)
                    {
                        foreach (var thucHien in tinhhinhThucHien)
                        {
                            var file = thucHien.FileDinhKem;
                            if (file != null && file.Count > 0)
                            {
                                dbContext.FileTinhHinhThucHien.RemoveRange(file);
                                await dbContext.SaveChangesAsync();
                            }
                        }

                        dbContext.TinhHinhThucHien.RemoveRange(tinhhinhThucHien);
                        await dbContext.SaveChangesAsync();
                    }

                    var filevb = vb.FileDinhKem;
                    if (filevb != null && filevb.Count > 0)
                    {
                        dbContext.FileVanBanChiDao.RemoveRange(filevb);
                        await dbContext.SaveChangesAsync();
                    }

                    dbContext.VanBanChiDao.Remove(vb);
                    await dbContext.SaveChangesAsync();

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }

            }
        }

        public async Task<bool> TraVanBan(int id, string lydo)
        {
            var dbContext = new MainDbContext();

            var vb = await dbContext.VanBanChiDao.FirstOrDefaultAsync(x => x.Id == id);
            if (vb == null) return true;

            vb.IsTralai = true;
            vb.LydoTraLai = lydo;
            vb.TrangThai = TrangThaiVanBan.TraLai;

            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}