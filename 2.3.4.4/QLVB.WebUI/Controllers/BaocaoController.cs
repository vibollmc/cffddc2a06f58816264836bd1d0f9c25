using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLVB.Core.Contract;
using QLVB.Domain.Entities;
using QLVB.DTO;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using QLVB.DTO.Baocao;
using QLVB.Common.Date;
using Microsoft.Reporting.WebForms;
using System.Data;
using QLVB.Common.Utilities;
using OfficeOpenXml;
using System.IO;


namespace QLVB.WebUI.Controllers
{
    public class BaocaoController : Controller
    {
        #region Constructor

        private IBaocaoManager _baocao;

        public BaocaoController(IBaocaoManager baocao)
        {
            _baocao = baocao;
        }

        #endregion Constructor

        #region FormBaocao
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult _Toolbar()
        {
            //ngayhientai.DayOfWeek
            ViewBag.strngaybd = DateTime.Now;
            ViewBag.strngaykt = DateTime.Now.AddDays(7);

            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult _Category()
        {
            CategoryBaocaoViewModel model = _baocao.GetCategory();
            return PartialView(model);
        }


        public ActionResult _DetailSovb(int intloai)
        {
            var sovb = _baocao.GetSovanban(intloai);
            if (intloai == 1)
            {
                // so van ban den
                var donvi = _baocao.GetListDonvi();
                ViewBag.listdonvi = donvi;
            }

            return PartialView(sovb);
        }

        public ActionResult _DetailLoaivb(int intloai)
        {
            var loaivb = _baocao.GetLoaivanban(intloai);

            return PartialView(loaivb);
        }
        public ActionResult _Detailvbdt(int intloai)
        {
            return PartialView();
        }

        #endregion FormBaocao

        #region RequestData

        private SoVanbandenViewModel RequestSovanbanden(string strngaybd, string strngaykt, int? sovb,
           string listidso, bool IsXuly, int iddonvi)
        {
            SoVanbandenViewModel sovbDTO = new SoVanbandenViewModel();
            sovbDTO.idsovanban = sovb;
            sovbDTO.strTungay = strngaybd;
            sovbDTO.strDenngay = strngaykt;
            sovbDTO.IsXuly = IsXuly;
            sovbDTO.iddonvi = iddonvi;
            sovbDTO.strtenphong = _baocao.GetTenPhong(iddonvi, IsXuly);
            sovbDTO.strTenso = _baocao.GetTensovanban(listidso); //_baocao.GetTensovanban((int)sovb);
            sovbDTO.strTendonvi = _baocao.GetTenDonvi();

            List<int> listid = new List<int>();
            string[] split = listidso.Split(',');
            foreach (var s in split)
            {
                try
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        int id = Convert.ToInt32(s);
                        listid.Add(id);
                    }
                }
                catch { }
            }
            sovbDTO.listidso = listid;


            return sovbDTO;
        }

        private SovanbandiViewModel RequestSovanbandi(string strngaybd, string strngaykt, int? sovb, string listidso)
        {
            SovanbandiViewModel sovbDTO = new SovanbandiViewModel();
            sovbDTO.idsovanban = sovb;
            sovbDTO.strTungay = strngaybd;
            sovbDTO.strDenngay = strngaykt;
            sovbDTO.strTenso = _baocao.GetTensovanban(listidso);
            sovbDTO.strTendonvi = _baocao.GetTenDonvi();

            List<int> listid = new List<int>();
            string[] split = listidso.Split(',');
            foreach (var s in split)
            {
                try
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        int id = Convert.ToInt32(s);
                        listid.Add(id);
                    }
                }
                catch { }
            }
            sovbDTO.listidso = listid;

            return sovbDTO;
        }

        private LoaivanbandenViewModel RequestLoaivanbanden(string strngaybd, string strngaykt, int? loaivb, string listidloai)
        {
            LoaivanbandenViewModel loaivbDTO = new LoaivanbandenViewModel();
            loaivbDTO.idloaivanban = loaivb;
            loaivbDTO.strTungay = strngaybd;
            loaivbDTO.strDenngay = strngaykt;
            loaivbDTO.strTenloai = _baocao.GetTenLoaivanban(listidloai);
            loaivbDTO.strTendonvi = _baocao.GetTenDonvi();

            List<int> listid = new List<int>();
            string[] split = listidloai.Split(',');
            foreach (var s in split)
            {
                try
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        int id = Convert.ToInt32(s);
                        listid.Add(id);
                    }
                }
                catch { }
            }
            loaivbDTO.listidloai = listid;

            return loaivbDTO;
        }

        private LoaivanbandiViewModel RequestLoaivanbandi(string strngaybd, string strngaykt, int? loaivb, string listidloai)
        {
            LoaivanbandiViewModel loaivbDTO = new LoaivanbandiViewModel();
            loaivbDTO.idloaivanban = loaivb;
            loaivbDTO.strTungay = strngaybd;
            loaivbDTO.strDenngay = strngaykt;
            loaivbDTO.strTenloai = _baocao.GetTenLoaivanban(listidloai);
            loaivbDTO.strTendonvi = _baocao.GetTenDonvi();

            List<int> listid = new List<int>();
            string[] split = listidloai.Split(',');
            foreach (var s in split)
            {
                try
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        int id = Convert.ToInt32(s);
                        listid.Add(id);
                    }
                }
                catch { }
            }
            loaivbDTO.listidloai = listid;
            return loaivbDTO;
        }

        #endregion RequestData

        #region Report
        public ActionResult Sovanbanden(string strngaybd, string strngaykt, int? sovb,
            string listidso, bool IsXuly, int iddonvi)
        {
            SoVanbandenViewModel sovbDTO = RequestSovanbanden(strngaybd, strngaykt, sovb,
                    listidso, IsXuly, iddonvi);

            var vanban = _baocao.GetSovanbanden(sovbDTO);
            ReportSoVanbandenViewModel model = new ReportSoVanbandenViewModel();
            model.Sovanban = QLVB.Common.Utilities.ListtoDataTableConverter.ToDataTable(vanban);
            model.Thamso = sovbDTO;

            return View(model);
        }

        public ActionResult Sovanbandi(string strngaybd, string strngaykt, int? sovb, string listidso)
        {
            SovanbandiViewModel sovbDTO = RequestSovanbandi(strngaybd, strngaykt, sovb, listidso);

            var vanban = _baocao.GetSovanbandi(sovbDTO);
            ReportSovanbandiViewModel model = new ReportSovanbandiViewModel();
            model.Sovanban = ListtoDataTableConverter.ToDataTable(vanban);
            model.Thamso = sovbDTO;

            return View(model);
        }

        public ActionResult Loaivanbanden(string strngaybd, string strngaykt, int? loaivb, string listidloai)
        {
            LoaivanbandenViewModel loaivbDTO = RequestLoaivanbanden(strngaybd, strngaykt, loaivb, listidloai);


            var vanban = _baocao.GetLoaivanbanden(loaivbDTO);
            ReportLoaivanbandenViewModel model = new ReportLoaivanbandenViewModel();
            model.Loaivanban = ListtoDataTableConverter.ToDataTable(vanban);
            model.Thamso = loaivbDTO;

            return View(model);
        }

        public ActionResult Loaivanbandi(string strngaybd, string strngaykt, int? loaivb, string listidloai)
        {
            LoaivanbandiViewModel loaivbDTO = RequestLoaivanbandi(strngaybd, strngaykt, loaivb, listidloai);


            var vanban = _baocao.GetLoaivanbandi(loaivbDTO);
            ReportLoaivanbandiViewModel model = new ReportLoaivanbandiViewModel();
            model.Loaivanban = ListtoDataTableConverter.ToDataTable(vanban);
            model.Thamso = loaivbDTO;

            return View(model);
        }


        #endregion Report

        #region InphieunhanHS

        public ActionResult InPhieuNhanVBDen(int id)
        //(string strngaybd, string strngaykt, string strngayhientai,
        //string strtendonvi, string strtrichyeu, string strnguoitiepnhan, string strtensovanban)
        {
            PhieunhanVBDenViewModels model = _baocao.GetNoidungPhieuVBDen(id);


            return View(model);
        }

        #endregion InphieunhanHS

        #region Excel

        private int GetLineCount(String text, int columnWidth)
        {
            try
            {
                var lineCount = 1;
                var textPosition = 0;

                while (textPosition <= text.Length)
                {
                    textPosition = Math.Min(textPosition + columnWidth, text.Length);
                    if (textPosition == text.Length)
                        break;

                    if (text[textPosition - 1] == ' ' || text[textPosition] == ' ')
                    {
                        lineCount++;
                        textPosition++;
                    }
                    else
                    {
                        textPosition = text.LastIndexOf(' ', textPosition) + 1;

                        var nextSpaceIndex = text.IndexOf(' ', textPosition);
                        if (nextSpaceIndex - textPosition >= columnWidth)
                        {
                            lineCount += (nextSpaceIndex - textPosition) / columnWidth;
                            textPosition = textPosition + columnWidth;
                        }
                        else
                            lineCount++;
                    }
                }

                return lineCount;
            }
            catch
            {
                return 1;
            }
        }


        public ActionResult Sovanbanden_Excel(string strngaybd, string strngaykt, int? sovb,
            string listidso, bool IsXuly, int iddonvi)
        {

            var sovbDTO = RequestSovanbanden(strngaybd, strngaykt, sovb,
             listidso, IsXuly, iddonvi);

            string physicalPath = HttpContext.Server.MapPath("~/Report/rptSoVanbanden.xlsx");

            FileInfo templateFile = new FileInfo(physicalPath);

            using (FileStream templateDocumentStream = System.IO.File.OpenRead(physicalPath))
            {
                // Create Excel EPPlus Package based on template stream
                using (ExcelPackage pck = new ExcelPackage(templateDocumentStream))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                    ws.Cells["C2"].Value = sovbDTO.strTendonvi;
                    ws.Cells["C6"].Value = QLVB.Common.Date.DateServices.FormatDateTimeVN(DateTime.Now);
                    ws.Cells["C3"].Value = sovbDTO.strtenphong;
                    ws.Cells["C3:G3"].Merge = true;
                    ws.Cells["C3"].Style.Font.Bold = true;
                    ws.Cells["C3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells["K5"].Value = "Theo sổ: " + sovbDTO.strTenso;
                    ws.Cells["J9"].Value = "Từ ngày: " + sovbDTO.strTungay + " đến ngày: " + sovbDTO.strDenngay;


                    int count = 0, start = 11, row = 0;


                    var vanban = _baocao.GetSovanbanden(sovbDTO);

                    foreach (var vb in vanban)
                    {
                        count++;
                        row = count + start;


                        string b = "B" + row.ToString();
                        ws.Cells[b].Value = count.ToString();
                        string c = b + ":" + "C" + row.ToString();
                        var cell = ws.Cells[c];
                        cell.Merge = true;
                        ws.Cells[b].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        var border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string d = "D" + row.ToString();
                        ws.Cells[d].Value = vb.strnoiphathanh;
                        cell = ws.Cells[d];
                        ws.Cells[d].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string e = "E" + row.ToString();
                        string ngayden = QLVB.Common.Date.DateServices.FormatDateVN(vb.strngayden);
                        ws.Cells[e].Value = ngayden;
                        cell = ws.Cells[e];
                        cell.Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string f = "F" + row.ToString();
                        ws.Cells[f].Value = vb.intsoden;
                        cell = ws.Cells[f];
                        cell.Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string g = "G" + row.ToString();
                        string h = g + ":" + "H" + row.ToString();
                        ws.Cells[g].Value = vb.strkyhieu;
                        cell = ws.Cells[h];
                        cell.Merge = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string i = "I" + row.ToString();
                        string j = i + ":" + "L" + row.ToString();
                        ws.Cells[i].Value = vb.strtrichyeu;
                        cell = ws.Cells[j];
                        cell.Merge = true;
                        ws.Cells[i].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string m = "M" + row.ToString();
                        string n = m + ":" + "N" + row.ToString();
                        ws.Cells[m].Value = vb.strnoinhan;
                        cell = ws.Cells[n];
                        cell.Merge = true;
                        ws.Cells[m].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string o = "O" + row.ToString();
                        //ws.Cells[h].Value = "";
                        cell = ws.Cells[o];
                        cell.Style.WrapText = true;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        var lineCount_trichyeu = GetLineCount(vb.strtrichyeu, 50);
                        var lineCount_noiphathanh = GetLineCount(vb.strnoiphathanh, 26);
                        ExcelRow hang = ws.Row(row);
                        hang.Height = 13 * (lineCount_trichyeu > lineCount_noiphathanh ? lineCount_trichyeu : lineCount_noiphathanh);

                    }


                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Sovanbanden.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                }

            }
            return null;
        }


        public ActionResult Sovanbandi_Excel(string strngaybd, string strngaykt, int? sovb, string listidso)
        {
            SovanbandiViewModel sovbDTO = RequestSovanbandi(strngaybd, strngaykt, sovb, listidso);

            string physicalPath = HttpContext.Server.MapPath("~/Report/rptSoVanbandi.xlsx");

            FileInfo templateFile = new FileInfo(physicalPath);

            using (FileStream templateDocumentStream = System.IO.File.OpenRead(physicalPath))
            {
                // Create Excel EPPlus Package based on template stream
                using (ExcelPackage pck = new ExcelPackage(templateDocumentStream))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                    ws.Cells["B2"].Value = sovbDTO.strTendonvi;
                    ws.Cells["B6"].Value = QLVB.Common.Date.DateServices.FormatDateTimeVN(DateTime.Now);



                    ws.Cells["J5"].Value = "Theo sổ: " + sovbDTO.strTenso;
                    ws.Cells["I9"].Value = "Từ ngày: " + sovbDTO.strTungay + " đến ngày: " + sovbDTO.strDenngay;


                    int count = 0, start = 11, row = 0;


                    var vanban = _baocao.GetSovanbandi(sovbDTO);

                    foreach (var vb in vanban)
                    {
                        count++;
                        row = count + start;

                        string c = "C" + row.ToString();
                        ws.Cells[c].Value = count.ToString();
                        var cell = ws.Cells[c];
                        cell.Merge = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        var border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string d = "D" + row.ToString();
                        ws.Cells[d].Value = QLVB.Common.Date.DateServices.FormatDateVN(vb.strngayky);
                        cell = ws.Cells[d];
                        ws.Cells[d].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string e = "E" + row.ToString();
                        ws.Cells[e].Value = vb.intso + "/" + vb.strkyhieu;
                        cell = ws.Cells[e];
                        ws.Cells[e].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string f = "F" + row.ToString();
                        string g = f + ":G" + row.ToString();
                        ws.Cells[f].Value = vb.strnguoiky;
                        cell = ws.Cells[g];
                        cell.Merge = true;
                        ws.Cells[f].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string h = "H" + row.ToString();
                        string i = h + ":J" + row.ToString();
                        ws.Cells[h].Value = vb.strtrichyeu;
                        cell = ws.Cells[i];
                        cell.Merge = true;
                        ws.Cells[i].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string k = "K" + row.ToString();
                        string l = k + ":L" + row.ToString();
                        ws.Cells[k].Value = vb.strnguoisoan;
                        cell = ws.Cells[l];
                        cell.Merge = true;
                        ws.Cells[l].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string m = "M" + row.ToString();
                        string n = m + ":N" + row.ToString();
                        ws.Cells[m].Value = vb.strnoinhan;
                        cell = ws.Cells[n];
                        cell.Merge = true;
                        ws.Cells[n].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string o = "O" + row.ToString();
                        //ws.Cells[h].Value = "";
                        cell = ws.Cells[o];
                        cell.Style.WrapText = true;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        var lineCount_trichyeu = GetLineCount(vb.strtrichyeu, 36);
                        var lineCount_noinhan = GetLineCount(vb.strnoinhan, 25);
                        ExcelRow hang = ws.Row(row);
                        hang.Height = 13 * (lineCount_trichyeu > lineCount_noinhan ? lineCount_trichyeu : lineCount_noinhan);

                    }

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Sovanbandi.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                }
            }

            return null;
        }

        public ActionResult Loaivanbanden_Excel(string strngaybd, string strngaykt, int? loaivb, string listidloai)
        {
            var loaivbDTO = RequestLoaivanbanden(strngaybd, strngaykt, loaivb, listidloai);

            string physicalPath = HttpContext.Server.MapPath("~/Report/rptSoVanbanden.xlsx");

            FileInfo templateFile = new FileInfo(physicalPath);

            using (FileStream templateDocumentStream = System.IO.File.OpenRead(physicalPath))
            {
                // Create Excel EPPlus Package based on template stream
                using (ExcelPackage pck = new ExcelPackage(templateDocumentStream))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                    ws.Cells["C2"].Value = loaivbDTO.strTendonvi;
                    ws.Cells["C6"].Value = QLVB.Common.Date.DateServices.FormatDateTimeVN(DateTime.Now);
                    //ws.Cells["C3"].Value = loaivbDTO.strtenphong;
                    //ws.Cells["C3:G3"].Merge = true;
                    //ws.Cells["C3"].Style.Font.Bold = true;
                    //ws.Cells["C3"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    ws.Cells["K5"].Value = "Theo loại: " + loaivbDTO.strTenloai;
                    ws.Cells["J9"].Value = "Từ ngày: " + loaivbDTO.strTungay + " đến ngày: " + loaivbDTO.strDenngay;


                    int count = 0, start = 11, row = 0;


                    var vanban = _baocao.GetLoaivanbanden(loaivbDTO);

                    foreach (var vb in vanban)
                    {
                        count++;
                        row = count + start;


                        string b = "B" + row.ToString();
                        ws.Cells[b].Value = count.ToString();
                        string c = b + ":" + "C" + row.ToString();
                        var cell = ws.Cells[c];
                        cell.Merge = true;
                        ws.Cells[b].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        var border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string d = "D" + row.ToString();
                        ws.Cells[d].Value = vb.strnoiphathanh;
                        cell = ws.Cells[d];
                        ws.Cells[d].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string e = "E" + row.ToString();
                        string ngayden = QLVB.Common.Date.DateServices.FormatDateVN(vb.strngayden);
                        ws.Cells[e].Value = ngayden;
                        cell = ws.Cells[e];
                        cell.Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string f = "F" + row.ToString();
                        ws.Cells[f].Value = vb.intsoden;
                        cell = ws.Cells[f];
                        cell.Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string g = "G" + row.ToString();
                        string h = g + ":" + "H" + row.ToString();
                        ws.Cells[g].Value = vb.strkyhieu;
                        cell = ws.Cells[h];
                        cell.Merge = true;
                        cell.Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string i = "I" + row.ToString();
                        string j = i + ":" + "L" + row.ToString();
                        ws.Cells[i].Value = vb.strtrichyeu;
                        cell = ws.Cells[j];
                        cell.Merge = true;
                        ws.Cells[i].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string m = "M" + row.ToString();
                        string n = m + ":" + "N" + row.ToString();
                        ws.Cells[m].Value = vb.strnoinhan;
                        cell = ws.Cells[n];
                        cell.Merge = true;
                        ws.Cells[m].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string o = "O" + row.ToString();
                        //ws.Cells[h].Value = "";
                        cell = ws.Cells[o];
                        cell.Style.WrapText = true;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        var lineCount_trichyeu = GetLineCount(vb.strtrichyeu, 50);
                        var lineCount_noiphathanh = GetLineCount(vb.strnoiphathanh, 26);
                        ExcelRow hang = ws.Row(row);
                        hang.Height = 13 * (lineCount_trichyeu > lineCount_noiphathanh ? lineCount_trichyeu : lineCount_noiphathanh);

                    }


                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Loaivanbanden.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                }

            }
            return null;
        }

        public ActionResult Loaivanbandi_Excel(string strngaybd, string strngaykt, int? loaivb, string listidloai)
        {
            var loaivbDTO = RequestLoaivanbandi(strngaybd, strngaykt, loaivb, listidloai);

            string physicalPath = HttpContext.Server.MapPath("~/Report/rptSoVanbandi.xlsx");

            FileInfo templateFile = new FileInfo(physicalPath);

            using (FileStream templateDocumentStream = System.IO.File.OpenRead(physicalPath))
            {
                // Create Excel EPPlus Package based on template stream
                using (ExcelPackage pck = new ExcelPackage(templateDocumentStream))
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.First();

                    ws.Cells["B2"].Value = loaivbDTO.strTendonvi;
                    ws.Cells["B6"].Value = QLVB.Common.Date.DateServices.FormatDateTimeVN(DateTime.Now);



                    ws.Cells["J5"].Value = "Theo loại: " + loaivbDTO.strTenloai;
                    ws.Cells["I9"].Value = "Từ ngày: " + loaivbDTO.strTungay + " đến ngày: " + loaivbDTO.strDenngay;


                    int count = 0, start = 11, row = 0;


                    var vanban = _baocao.GetLoaivanbandi(loaivbDTO);

                    foreach (var vb in vanban)
                    {
                        count++;
                        row = count + start;

                        string c = "C" + row.ToString();
                        ws.Cells[c].Value = count.ToString();
                        var cell = ws.Cells[c];
                        cell.Merge = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        var border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string d = "D" + row.ToString();
                        ws.Cells[d].Value = QLVB.Common.Date.DateServices.FormatDateVN(vb.strngayky);
                        cell = ws.Cells[d];
                        ws.Cells[d].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string e = "E" + row.ToString();
                        ws.Cells[e].Value = vb.intso + "/" + vb.strkyhieu;
                        cell = ws.Cells[e];
                        ws.Cells[e].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string f = "F" + row.ToString();
                        string g = f + ":G" + row.ToString();
                        ws.Cells[f].Value = vb.strnguoiky;
                        cell = ws.Cells[g];
                        cell.Merge = true;
                        ws.Cells[f].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string h = "H" + row.ToString();
                        string i = h + ":J" + row.ToString();
                        ws.Cells[h].Value = vb.strtrichyeu;
                        cell = ws.Cells[i];
                        cell.Merge = true;
                        ws.Cells[i].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string k = "K" + row.ToString();
                        string l = k + ":L" + row.ToString();
                        ws.Cells[k].Value = vb.strnguoisoan;
                        cell = ws.Cells[l];
                        cell.Merge = true;
                        ws.Cells[l].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;

                        string m = "M" + row.ToString();
                        string n = m + ":N" + row.ToString();
                        ws.Cells[m].Value = vb.strnoinhan;
                        cell = ws.Cells[n];
                        cell.Merge = true;
                        ws.Cells[n].Style.WrapText = true;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Font.Size = 10;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        cell.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Top;


                        string o = "O" + row.ToString();
                        //ws.Cells[h].Value = "";
                        cell = ws.Cells[o];
                        cell.Style.WrapText = true;
                        border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Bottom.Style = border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


                        var lineCount_trichyeu = GetLineCount(vb.strtrichyeu, 36);
                        var lineCount_noinhan = GetLineCount(vb.strnoinhan, 25);
                        ExcelRow hang = ws.Row(row);
                        hang.Height = 13 * (lineCount_trichyeu > lineCount_noinhan ? lineCount_trichyeu : lineCount_noinhan);

                    }

                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=Loaivanbandi.xlsx");
                    Response.BinaryWrite(pck.GetAsByteArray());
                }
            }

            return null;
        }


        #endregion Excel

    }
}