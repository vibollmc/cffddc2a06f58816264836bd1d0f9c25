using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DLTD.Web.Main.Models;
using DLTD.Web.Main.Models.Enum;
using DLTD.Web.Main.ViewModels;

namespace DLTD.Web.Main.DAL
{
    public class DonViManagement
    {
        private static DonViManagement _instance;
        public static DonViManagement Go
        {
            get
            {
                if (_instance == null) _instance = new DonViManagement();
                return _instance;
            }
        }

        public async Task<IEnumerable<DonVi>> GetDonViByKhoi(params int[] idKhoi)
        {
            var dbContext = new MainDbContext();

            if (idKhoi == null || idKhoi.Length == 0) 
                return await dbContext.DonVi.Where(x=> x.TrangThai == TrangThai.Active).ToListAsync();
            return await dbContext.DonVi.Where(x => x.TrangThai == TrangThai.Active && idKhoi.Contains(x.IdKhoi.Value)).ToListAsync();
        }
        public async Task<IEnumerable<Khoi>> GetNguonChiDaoByKhoi()
        {
            var dbContext = new MainDbContext();
            return await dbContext.Khoi.Where(x => x.NguonChiDao == true).ToListAsync();
        }
        //public ListKhoiViewModel GetNguonChiDao()
        //{
        //    ListKhoiViewModel model = new ListKhoiViewModel();
        //    model.Khoi = _dbContext.Khoi.Where(x => x.NguonChiDao == true).Select(x=> new KhoiViewModel { Id = x.Id, Ten = x.Ten});

        //    return model;
        //}
        public IEnumerable<KhoiViewModel> GetNguonChiDao()
        {
            var dbContext = new MainDbContext();
            return dbContext.Khoi.Where(x => x.NguonChiDao == true).Select(x => new KhoiViewModel { Id = x.Id, Ten = x.Ten });
        }
        public IEnumerable<DonViViewModel> GetDonViTheoDoi(int? idKhoi, string filter)
        {
            var dbContext = new MainDbContext();
            var donVi =  dbContext.DonVi.Where(
                x => (x.Khoi.Id == 6 || x.Khoi.Id == 7 || x.Khoi.Id == 9) && x.TrangThai == TrangThai.Active && (idKhoi == null || x.Khoi.Id == idKhoi));
            return !string.IsNullOrWhiteSpace(filter)
                ? donVi.Where(x => x.Ten.Contains(filter)).Select(x => new DonViViewModel {Id = x.Id, Ten = x.Ten})
                : donVi.Select(x => new DonViViewModel {Id = x.Id, Ten = x.Ten});
        }

        public IEnumerable<KhoiViewModel> GetKhoi(params int[] idKhoi)
        {
            var dbContext = new MainDbContext();
            return dbContext.Khoi.Where(x => idKhoi.Contains(x.Id.Value)).Select(x => new KhoiViewModel { Id = x.Id, Ten = x.Ten });
        }
    }
}