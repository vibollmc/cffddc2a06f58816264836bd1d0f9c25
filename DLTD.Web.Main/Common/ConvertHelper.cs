using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using DLTD.Web.Main.Models;
using DLTD.Web.Main.Models.Enum;
using DLTD.Web.Main.Models.MetaData;
using DLTD.Web.Main.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace DLTD.Web.Main.Common
{
    public static class ConvertHelper
    {
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string GetDisplayValue<T>(this T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
        public static DataTable ListToDataTable<T>(this IList<T> list)
        {
            var dt = new DataTable(typeof(T).Name);
            foreach (var info in typeof(T).GetProperties())
            {
                if (info.PropertyType == typeof(decimal?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(decimal)));
                else if (info.PropertyType == typeof(DateTime?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(DateTime)));
                else if (info.PropertyType == typeof(int?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(int)));
                else
                    dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            foreach (var t in list)
            {
                var row = dt.NewRow();
                foreach (var info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public static DataTable ObjectToDataTable(this object obj)
        {
            try
            {
                if (obj == null) return new DataTable("T");
                else if (obj is DataTable) return (DataTable)obj;
                else if (obj is DataRow)
                {
                    var a = ((DataRow)obj).Table.Clone();
                    a.ImportRow((DataRow)obj);
                    return a;
                }
                else return EntityToDataTable(obj);
            }
            catch
            {
                return new DataTable("T");
            }
        }

        public static DataTable EntityToDataTable(this object entity)
        {
            var dt = new DataTable(entity.GetType().Name);
            foreach (var info in entity.GetType().GetProperties())
            {
                if (info.PropertyType == typeof(decimal?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(decimal)));
                else if (info.PropertyType == typeof(DateTime?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(DateTime)));
                else if (info.PropertyType == typeof(int?))
                    dt.Columns.Add(new DataColumn(info.Name, typeof(int)));
                else
                    dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            var row = dt.NewRow();
            foreach (var info in entity.GetType().GetProperties())
            {
                row[info.Name] = info.GetValue(entity, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);

            return dt;
        }

        public static T DataRowToEntity<T>(this DataRow dataRow) where T : new()
        {
            try
            {
                var objT = new T();
                var infos = TypeDescriptor.GetProperties(objT.GetType());
                foreach (DataColumn dc in dataRow.Table.Columns)
                {
                    foreach (PropertyDescriptor info in infos)
                    {
                        if (!string.Equals(info.Name, dc.ColumnName, StringComparison.CurrentCultureIgnoreCase)) continue;
                        var value = dataRow[dc.ColumnName];
                        if (info.IsReadOnly || value == DBNull.Value) continue;
                        var typeConverter = info.Converter;
                        var obj = value;
                        if (typeConverter != null)
                        {
                            obj = value.GetType().Name == "Byte[]" ? value : typeConverter.ConvertFromString(value.ToString());
                        }
                        info.SetValue(objT, obj);
                    }
                }
                return objT;
            }
            catch
            {
                return default(T);
            }
        }

        public static List<T> DataTableToList<T>(this DataTable data) where T : new()
        {
            return (from DataRow dr in data.Rows select DataRowToEntity<T>(dr)).ToList();
        }

        public static IEnumerable<T> Append<T>(
        this IEnumerable<T> source, params T[] tail)
        {
            return source.Concat(tail);
        }
        public static IEnumerable<T> Prepend<T>(
        this IEnumerable<T> source, params T[] tail)
        {
            var results = tail.ToList();
            return results.Concat(source);
        }

        public static int? ToIntExt(this string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return null;

            int val;
            if (int.TryParse(data, out val)) return val;

            return null;
        }

        public static DateTime? ToDateTimeExt(this string data, string format)
        {
            if (string.IsNullOrWhiteSpace(data)) return null;

            DateTime val;
            if (DateTime.TryParseExact(data, format, null, DateTimeStyles.None, out val)) return val;

            return null;
        }

        /// <summary>
        /// Default format "dd/MM/yyyy"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeExt(this string data)
        {
            return data.ToDateTimeExt("dd/MM/yyyy");
        }

        public static VanBanChiDao Transform(this VanBanChiDaoInput data)
        {
            if (data == null) return null;
            return new VanBanChiDao
            {
                Id = data.Id,
                TrangThai = TrangThaiVanBan.Moi,
                IdDonVi = data.IdDonVi,
                IdVanBan = data.IdVanBan,
                YKienChiDao = data.YKienChiDao,
                UserId = data.UserId,
                Trichyeu = data.Trichyeu,
                ThoiHanXuLy = data.ThoiHanXuLy,
                SoKH = data.SoKH,
                DoKhan = data.DoKhan,
                IdNguonChiDao = data.NguonChiDao,
                Ngayky = data.Ngayky,
                IdNguoiTheoDoi = data.IdNguoiTheoDoi,
                IdNguoiChiDao = data.IdNguoiChiDao
            };
        }
       
        public static VanBanChiDaoViewModel Transform(this VanBanChiDao data)
        {
            if (data == null) return null;
            
            var obj = new VanBanChiDaoViewModel
            {
                TrangThai = data.TrangThai,
                Id = data.Id,
                SoKH = data.SoKH,
                UserId = data.UserId,
                IdDonVi = data.IdDonVi,
                IdNguoiChiDao=data.IdNguoiChiDao,
                ThoiHanXuLy = data.ThoiHanXuLy,
                Ngayky = data.Ngayky,
                IdVanBan = data.IdVanBan,
                Trichyeu = data.Trichyeu,
                YKienChiDao = data.YKienChiDao,
                NgayTao = data.NgayTao,
                FileDinhKem = data.FileDinhKem != null && data.FileDinhKem.Any() ? data.FileDinhKem.Count : 0,
                LinkFileDinhKem = data.FileDinhKem != null && data.FileDinhKem.Any() ? data.FileDinhKem.FirstOrDefault().Url : null,
                NguoiTheoDoi = data.NguoiTheoDoi != null ? data.NguoiTheoDoi.Ten : null,
                NguoiChiDao = data.NguoiChiDao!=null?data.NguoiChiDao.Ten:null,
                TenDonVi = data.DonVi.Ten,
                NgayHoanThanh = data.NgayHoanThanh,
                DoKhan = data.DoKhan,
                IdNguonChiDao = data.IdNguonChiDao,
                NguonChiDao = data.NguonChiDao != null ? data.NguonChiDao.Ten : null,
            };
            //if (data.FileDinhKem != null)
            //{
            //    if (data.FileDinhKem.Any())
            //    {
            //        obj.FileDinhKem = data.FileDinhKem.Count;
            //        var file = data.FileDinhKem.FirstOrDefault();
            //        if (file != null)
            //        {
            //            obj.LinkFileDinhKem = file.Url;
            //        }
            //        else
            //        {
            //            obj.LinkFileDinhKem = null;
            //        }
            //    }
            //    else
            //    {
            //        obj.FileDinhKem = 0;
            //        obj.LinkFileDinhKem = null;
            //    }
            //}
            return obj;
        }

      
        public static TinhHinhThucHienViewModel Transform(this TinhHinhThucHien data)
        {
            if (data == null) return null;
            return new TinhHinhThucHienViewModel
            {
                Id = data.Id,
                NoiDungThucHien = data.NoiDungThucHien,
                NgayBaoCao = data.NgayBaoCao,
                NguoiDung = data.NguoiDung != null ? data.NguoiDung.Ten : null,
                DonVi = data.VanBanChiDao != null ? data.VanBanChiDao.DonVi.Ten + " (Xử lý chính)" : null,
                IdVanBanChiDao = data.IdVanBanChiDao,
                FileDinhKem = data.FileDinhKem != null && data.FileDinhKem.Any() ? data.FileDinhKem.SingleOrDefault().Name : null,
                FileUrl = data.FileDinhKem != null && data.FileDinhKem.Any() ? data.FileDinhKem.SingleOrDefault().Url : null,
                LoaiBaoCao = LoaiBaoCao.XuLyChinh
            };
        }

        public static TinhHinhThucHienViewModel Transform(this TinhHinhPhoiHop data)
        {
            if (data == null) return null;
            return new TinhHinhThucHienViewModel
            {
                Id = data.Id,
                NoiDungThucHien = data.NoiDungThucHien,
                NgayBaoCao = data.NgayXuLy,
                NguoiDung = data.NguoiDung != null ? data.NguoiDung.Ten : null,
                DonVi = data.DonViPhoiHop != null ? data.DonViPhoiHop.DonVi.Ten + " (Phối hợp xử lý)" : null,
                IdVanBanChiDao = data.DonViPhoiHop != null ? data.DonViPhoiHop.IdVanBan : null,
                FileDinhKem = data.FileDinhKem != null && data.FileDinhKem.Any() ? data.FileDinhKem.SingleOrDefault().Name : null,
                FileUrl = data.FileDinhKem != null && data.FileDinhKem.Any() ? data.FileDinhKem.SingleOrDefault().Url : null,
                LoaiBaoCao = LoaiBaoCao.PhoiHop
            };
        }

        public static TinhHinhThucHien Transform(this TinhHinhThucHienInput data)
        {
            if (data == null) return null;

            return new TinhHinhThucHien
            {
                IdVanBanChiDao = data.IdVanBanChiDao,
                NgayBaoCao = data.NgayBaoCao,
                UserId = data.UserId,
                NoiDungThucHien = data.NoiDungBaoCao
            };
        }
    }
}