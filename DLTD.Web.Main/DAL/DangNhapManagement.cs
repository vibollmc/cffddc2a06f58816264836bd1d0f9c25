using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DLTD.Web.Main.Models;
using DLTD.Web.Main.Models.Enum;
using DLTD.Web.Main.ViewModels;
using DLTD.Web.Main.Common;
namespace DLTD.Web.Main.DAL
{
    public class DangNhapManagement
    {
        private readonly MainDbContext _dbContext;

        public static DangNhapManagement Go => new DangNhapManagement();

        public DangNhapManagement()
        {
            _dbContext = new MainDbContext();
        }

        public async Task<DangNhap> SaveUserLoginWithThirdParty(DangNhap input)
        {
            var user =
                await _dbContext.DangNhap.SingleOrDefaultAsync(
                    x => x.TenDangNhap == input.TenDangNhap && x.DangNhapTu == input.DangNhapTu);
            if (user != null)
            {
                user.NhomNguoiDung = input.NhomNguoiDung;
                user.NgaySinh = input.NgaySinh;
                user.Ma = input.Ma;
                user.Ten = input.Ten;
                user.Email = input.Email;
                user.SoDienThoai = input.SoDienThoai;
                user.UrlImage = input.UrlImage;
                user.IdDonVi = input.IdDonVi;
                user.GioiTinh = input.GioiTinh;
                user.DangNhapTu = input.DangNhapTu;
                user.TrangThai = input.TrangThai;

                await _dbContext.SaveChangesAsync();
                return user;
            }
            
            _dbContext.DangNhap.Add(input);
            await _dbContext.SaveChangesAsync();    
            
            return input;
        }

        public async Task<DangNhap> Login(string username, string password)
        {
            string passwordinMD5 = CryptServices.HashMD5(password);
            var user = await _dbContext.DangNhap.SingleOrDefaultAsync(x => x.TenDangNhap == username && x.MatKhau == passwordinMD5);

            return user;
        }
        public async Task<IEnumerable<DangNhap>> GetNguoiChiDao(int? donvi)
        {
            return
                await
                    _dbContext.DangNhap.Where(
                        x =>
                            x.IdDonVi == donvi &&
                            x.NhomNguoiDung == NhomNguoiDung.LanhDao).OrderBy(x => x.Ten).ToListAsync();
        }

        public async Task<IEnumerable<DangNhap>> GetNguoiTheoDoi()
        {
            return
                await
                    _dbContext.DangNhap.Where(x => x.NhomNguoiDung == NhomNguoiDung.ChuyenVien || x.NhomNguoiDung == NhomNguoiDung.LanhDaoPhong)
                        .OrderBy(x => x.Ten)
                        .ToListAsync();
        }
        public IEnumerable<DangNhapViewModel> GetTonghopNguoitheoDoi()
        {
            return _dbContext.DangNhap.Where(x => x.NhomNguoiDung == NhomNguoiDung.ChuyenVien).OrderBy(x => x.Ten).Select(x => new DangNhapViewModel { Id = x.Id, Ten = x.Ten });
        }
        public IEnumerable<DangNhapViewModel> GetTonghopNguoichidao()
        {
            return _dbContext.DangNhap.Where(x => x.NhomNguoiDung == NhomNguoiDung.LanhDao).OrderBy(x => x.Ten).Select(x => new DangNhapViewModel { Id = x.Id, Ten = x.Ten });
        }

    }
}