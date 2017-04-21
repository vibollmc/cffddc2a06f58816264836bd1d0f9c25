using QLVB.Common.Logging;
using QLVB.Core.Contract;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using QLVB.DTO.Capcoquan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace QLVB.Core.Implementation
{
    public class CapcoquanManager : ICapcoquanManager
    {
        #region Constructor

        private IKhoiphathanhRepository _khoiRepo;
        private ITochucdoitacRepository _tochucRepo;
        private ILogger _logger;

        public CapcoquanManager(IKhoiphathanhRepository khoiRepo,
                    ITochucdoitacRepository tochucRepo,
                    ILogger logger)
        {
            _khoiRepo = khoiRepo;
            _tochucRepo = tochucRepo;
            _logger = logger;
        }

        #endregion Constructor

        #region Interface Implementation

        public IEnumerable<Khoiphathanh> GetListKhoiph()
        {
            return _khoiRepo.GetActiveKhoiphathanhs;
        }

        public IEnumerable<Tochucdoitac> GetListTochuc(int idkhoiph)
        {
            return _tochucRepo.GetActiveTochucdoitacs
                 .Where(p => p.intidkhoi == idkhoiph)
                 .OrderBy(p => p.strtentochucdoitac);
        }

        public EditKhoiphViewModel GetEditKhoiph(int id)
        {
            var khoiph = _khoiRepo.GetActiveKhoiphathanhs
                .Where(p => p.intid == id)
                .Select(p => new EditKhoiphViewModel
                {
                    intid = p.intid,
                    strtenkhoi = p.strtenkhoi
                })
                .FirstOrDefault();
            return khoiph;
        }

        public ResultFunction SaveKhoiph(EditKhoiphViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                Khoiphathanh khoiph = new Khoiphathanh();
                khoiph.strtenkhoi = model.strtenkhoi;
                if (model.intid == 0)
                {
                    // them moi
                    _khoiRepo.AddKhoi(khoiph);
                    _logger.Info("Thêm mới cấp cơ quan: " + model.strtenkhoi);
                }
                else
                {
                    // cap nhat
                    _khoiRepo.EditKhoi(model.intid, khoiph);
                    _logger.Info("Cập nhật cấp cơ quan: " + model.strtenkhoi + ", id: " + model.intid.ToString());
                }
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = "Lỗi: " + ex.Message;
            }
            return kq;
        }

        public ResultFunction DeleteKhoiph(EditKhoiphViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                _khoiRepo.DeleteKhoi(model.intid);
                _logger.Info("Xóa cấp cơ quan: " + model.strtenkhoi + ", id: " + model.intid.ToString());
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = "Lỗi: " + ex.Message;
            }
            return kq;
        }

        public EditTochucViewModel GetEditTochuc(int id)
        {
            var dv = _tochucRepo.GetActiveTochucdoitacs
                .Where(p => p.intid == id)
                .Select(p => new EditTochucViewModel
                {
                    intid = p.intid,
                    intidkhoiph = p.intidkhoi,
                    IsHoibao = p.inthoibao == (int)enumTochucdoitac.inthoibao.IsActive ? true : false,
                    Isvbdt = p.Isvbdt == (int)enumTochucdoitac.isvbdt.IsActive ? true : false,
                    strdiachi = p.strdiachi,
                    stremail = p.stremail,
                    stremailvbdt = p.stremailvbdt,
                    strmatochucdoitac = p.strmatochucdoitac,
                    strphone = p.strphone,
                    strtentochucdoitac = p.strtentochucdoitac,
                    strmadinhdanh = p.strmadinhdanh
                }).FirstOrDefault();
            return dv;
        }

        public ResultFunction SaveTochuc(EditTochucViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                Tochucdoitac tochuc = new Tochucdoitac();
                tochuc.strdiachi = model.strdiachi;
                tochuc.stremail = model.stremail;
                tochuc.stremailvbdt = model.stremailvbdt;
                tochuc.strphone = model.strphone;
                tochuc.strmadinhdanh = model.strmadinhdanh;
                tochuc.strmatochucdoitac = model.strmatochucdoitac;
                tochuc.strtentochucdoitac = model.strtentochucdoitac;
                tochuc.intidkhoi = model.intidkhoiph;
                tochuc.inthoibao = model.IsHoibao == true ? (int)enumTochucdoitac.inthoibao.IsActive : (int)enumTochucdoitac.inthoibao.NotActive;
                tochuc.Isvbdt = model.Isvbdt == true ? (int)enumTochucdoitac.isvbdt.IsActive : (int)enumTochucdoitac.isvbdt.NotActive;

                if (model.intid == 0)
                {
                    // them moi
                    _tochucRepo.AddTochuc(tochuc);
                    _logger.Info("Thêm mới đơn vị bên ngoài: " + model.strtentochucdoitac);
                }
                else
                {
                    // cap nhat
                    _tochucRepo.EditTochuc(model.intid, tochuc);
                    _logger.Info("Cập nhật đơn vị bên ngoài: " + model.strtentochucdoitac + ", id: " + model.intid.ToString());
                }
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = "Lỗi: " + ex.Message;
            }
            return kq;
        }

        public ResultFunction DeleteTochuc(EditTochucViewModel model)
        {
            ResultFunction kq = new ResultFunction();
            try
            {
                _tochucRepo.DeleteTochuc(model.intid);
                _logger.Info("Xóa đơn vị bên ngoài: " + model.strtentochucdoitac + ", id: " + model.intid.ToString());
                kq.id = (int)ResultViewModels.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                kq.id = (int)ResultViewModels.Error;
                kq.message = "Lỗi: " + ex.Message;
            }
            return kq;
        }

        public CapnhatDanhmucDonviViewModel GetDanhmucDonvi()
        {
            CapnhatDanhmucDonviViewModel model = new CapnhatDanhmucDonviViewModel();

            model.listdonviDB = GetDonviDB();
            //====================================================================
            string folderPath = QLVB.Common.Utilities.AppSettings.BackupDatabase;
            folderPath = HttpContext.Current.Server.MapPath(folderPath);

            string filepath = folderPath + "/EmailDonvi.xml";

            XElement allData = XElement.Load(filepath);
            if (allData != null)
            {
                string Update = allData.AncestorsAndSelf().FirstOrDefault().Attribute("Update").Value;

                var listdonvi = allData.Descendants("Donvi")
                    .Select(p => new EmailDonviXMLViewModel
                    {
                        id = 0,
                        strten = p.Element("Ten").Value,
                        email = p.Element("Email").Value,
                        madinhdanh = p.Element("Madinhdanh").Value
                    })
                    .OrderBy(p => p.strten)
                    .ToList()
                    ;

                model.listdonvixml = listdonvi;
                model.strNgayCapnhatXML = Update;
            }



            return model;
        }

        private List<EmailDonviXMLViewModel> GetDonviXML()
        {
            string folderPath = QLVB.Common.Utilities.AppSettings.BackupDatabase;
            folderPath = HttpContext.Current.Server.MapPath(folderPath);

            string filepath = folderPath + "/EmailDonvi.xml";

            XElement allData = XElement.Load(filepath);
            if (allData != null)
            {
                string Update = allData.AncestorsAndSelf().FirstOrDefault().Attribute("Update").Value;

                var listdonvi = allData.Descendants("Donvi")
                    .Select(p => new EmailDonviXMLViewModel
                    {
                        strten = p.Element("Ten").Value,
                        email = p.Element("Email").Value,
                        madinhdanh = p.Element("Madinhdanh").Value
                    })
                    .OrderBy(p => p.strten)
                    .ToList()
                    ;
                return listdonvi;
            }
            else
            {
                return null;
            }
        }

        private List<EmailDonviDBViewModel> GetDonviDB()
        {
            var model = _tochucRepo.GetActiveTochucdoitacs
               .Where(p => p.Isvbdt == (int)enumTochucdoitac.isvbdt.IsActive)
               .Select(p => new EmailDonviDBViewModel
               {
                   id = p.intid,
                   strten = p.strtentochucdoitac,
                   email = p.stremailvbdt,
                   madinhdanh = p.strmadinhdanh
               })
               .OrderBy(p => p.strten)
               .ToList()
               ;

            return model;
        }

        public int UpdateDanhmucDonvi(string listid)
        {
            var listdonviDB = GetDonviDB();
            var listdonviXML = GetDonviXML();

            List<int> listidupdate = new List<int>();

            string[] split = listid.Split(new Char[] { ';' });
            foreach (var s in split)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    int id = Convert.ToInt32(s);
                    listidupdate.Add(id);
                }
            }

            foreach (var p in listidupdate)
            {
                var db = listdonviDB.FirstOrDefault(d => d.id == p);
                var xml = listdonviXML.Where(x => x.email == db.email).FirstOrDefault();

                db.madinhdanh = xml.madinhdanh;
                //db.strten = xml.strten;
                _tochucRepo.UpdateTochuc(db.id, db.madinhdanh);

            }

            return 1;
        }
        public int AddNewDanhmucDonvi(string listemail, int idkhoiph)
        {
            string[] split = listemail.Split(new Char[] { ';' });
            List<string> listadd = new List<string>();
            foreach (var s in split)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    listadd.Add(s);
                }
            }
            var listdonviXML = GetDonviXML();
            foreach (var d in listadd)
            {
                var xml = listdonviXML.Where(p => p.email == d).FirstOrDefault();
                Tochucdoitac donvi = new Tochucdoitac();
                donvi.strmadinhdanh = xml.madinhdanh;
                donvi.strtentochucdoitac = xml.strten;
                donvi.stremailvbdt = xml.email;
                donvi.intidkhoi = idkhoiph;
                donvi.inttrangthai = (int)enumTochucdoitac.inttrangthai.IsActive;
                donvi.Isvbdt = (int)enumTochucdoitac.isvbdt.IsActive;

                _tochucRepo.AddTochuc(donvi);
            }

            return 0;
        }
        #endregion Interface Implementation
    }
}