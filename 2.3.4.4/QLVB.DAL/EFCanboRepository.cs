using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Entities;
using QLVB.Domain.Abstract;

namespace QLVB.DAL
{
    public class EFCanboRepository : ICanboRepository
    {
        private QLVBDatabase context;

        public EFCanboRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Canbo> GetAllCanbo
        {
            get { return context.Canbos.AsNoTracking(); }
        }

        public IQueryable<Canbo> GetActiveCanbo
        {
            get
            {
                return context.Canbos
                    .Where(p => p.inttrangthai == (int)enumcanbo.inttrangthai.IsActive)
                    .OrderBy(p => p.strkyhieu)
                    .ThenBy(p => p.strhoten);
            }
        }
        public int Them(Canbo cb)
        {
            try
            {
                //cb.inttrangthai = (int)EnumDanhmuc.inttrangthai_canbo.IsActive;
                cb.inttrangthai = (int)enumcanbo.inttrangthai.IsActive;
                cb.strngaytao = DateTime.Now;
                cb.strRight = "0000";

                context.Canbos.Add(cb);
                context.SaveChanges();
                return cb.intid;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Sua(int intid, Canbo cb)
        {
            try
            {
                Canbo _cb = context.Canbos.FirstOrDefault(p => p.intid == intid);
                _cb.intchucvu = cb.intchucvu;
                _cb.intgioitinh = cb.intgioitinh;
                _cb.intkivb = cb.intkivb;
                _cb.intnguoixuly = cb.intnguoixuly;
                _cb.intnhomquyen = cb.intnhomquyen;
                _cb.strdienthoai = cb.strdienthoai;
                _cb.stremail = cb.stremail;
                _cb.strhoten = cb.strhoten;
                _cb.strkyhieu = cb.strkyhieu;
                _cb.strmacanbo = cb.strmacanbo;
                _cb.strngaysinh = cb.strngaysinh;
                //_cb.strpassword = cb.strpassword; // khong cap nhat password, chuyen qua ham ResetPassword
                //_cb.strRight = cb.strRight;
                _cb.strusername = cb.strusername;

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ResetPassword(int intid, string strpass)
        {
            try
            {
                Canbo cb = context.Canbos.FirstOrDefault(p => p.intid == intid);
                cb.strpassword = strpass;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string Xoa(int intid)
        {
            try
            {
                Canbo cb = context.Canbos.FirstOrDefault(p => p.intid == intid);
                cb.inttrangthai = (int)enumcanbo.inttrangthai.NotActive;
                cb.strngayxoa = DateTime.Now;
                context.SaveChanges();
                return cb.strhoten;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Canbo GetActiveCanboByID(int id)
        {
            Canbo cb = context.Canbos
                .Where(p => p.inttrangthai == (int)enumcanbo.inttrangthai.IsActive)
                .FirstOrDefault(p => p.intid == id);
            return cb;
        }

        public Canbo GetAllCanboByID(int id)
        {
            Canbo cb = context.Canbos
                .FirstOrDefault(p => p.intid == id);
            return cb;
        }

        public List<Canbo> GetListCanbo()
        {
            List<Canbo> cb = context.Canbos.Where(p => p.inttrangthai == (int)enumcanbo.inttrangthai.IsActive).ToList();
            return cb;
        }

        public void UpdateDonvi(int iduser, int iddonvi)
        {
            try
            {
                var cb = context.Canbos.FirstOrDefault(p => p.intid == iduser);
                cb.intdonvi = iddonvi;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// lay tat ca cac tuy chon cua 1 user(mac dinh)
        /// </summary>
        /// <param name="iduser"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetListOption()
        {
            Dictionary<int, string> listOption = new Dictionary<int, string>();
            try
            {
                var names = Enum.GetNames(typeof(enumcanbo.strRight));
                var n1 = Enum.GetName(typeof(enumcanbo.strRight), enumcanbo.strRight.PhanXLNhieuVB);
                //===============================
                string name = string.Empty;
                name = "Phân xử lý nhiều văn bản cho 1 người";
                listOption.Add((int)enumcanbo.strRight.PhanXLNhieuVB, name);
                name = "Xử lý nhanh";
                listOption.Add((int)enumcanbo.strRight.XulyNhanh, name);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listOption;
        }
        /// <summary>
        /// lay tuy chon cua 1 user (strRight)
        /// </summary>
        /// <param name="iduser"></param>
        /// <returns></returns>
        public string GetUserOption(int iduser)
        {
            try
            {
                return context.Canbos.FirstOrDefault(p => p.intid == iduser).strRight;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// kiem tra cac option cua user
        /// </summary>
        /// <param name="iduser"></param>
        /// <param name="intOption"></param>
        /// <returns></returns>        
        public bool IsOption(int iduser, int intOption)
        {
            string strRight = context.Canbos.FirstOrDefault(p => p.intid == iduser).strRight;
            //string strRight = "1234"; "0000";
            string strOption = string.Empty;
            var arrRight = strRight.ToList();
            //foreach (var a in arrRight)
            //{
            //    strgiatri = a.ToString();
            //}
            //for (int i = 0; i < strRight.Length; i++)
            //{
            //    strgiatri = arrRight[i].ToString();
            //}
            //intOption = (int)enumcanbo.strRight.PhanXLNhieuVB;
            try
            {
                strOption = arrRight[intOption].ToString();
                return (strOption == "1") ? true : false;
            }
            catch
            {   // TH strRight.length < 4
                return false;
            }
        }
        /// <summary>
        /// kiem tra tuy chon cua user 
        /// </summary>
        /// <param name="strRight"></param>
        /// <param name="intOption"></param>
        /// <returns></returns>
        public bool IsOption(string strRight, int intOption)
        {
            //string strRight = "1234"; "0000";
            string strOption = string.Empty;
            var arrRight = strRight.ToList();
            try
            {
                strOption = arrRight[intOption].ToString();
                return (strOption == "1") ? true : false;
            }
            catch
            {   // TH strRight.length < 4
                return false;
            }
        }
        public void UpdateOption(int iduser, int intOption, bool blgiatri)
        {
            try
            {
                var cb = context.Canbos.FirstOrDefault(p => p.intid == iduser);
                string strRight = cb.strRight;
                //string strRight = "0000";
                string strOption = string.Empty;
                var arrRight = strRight.ToList();

                for (int i = 0; i < strRight.Length; i++)
                {
                    if (intOption == i)
                    {   //intOption = (int)enumcanbo.strRight.PhanXLNhieuVB;
                        strOption += blgiatri ? "1" : "0";
                    }
                    else
                    {
                        strOption += arrRight[i].ToString();
                    }
                }
                cb.strRight = strOption;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateOption(int iduser, string strRight)
        {
            try
            {
                var cb = context.Canbos.FirstOrDefault(p => p.intid == iduser);
                cb.strRight = strRight;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// cap nhat nhom quyen cua user
        /// </summary>
        /// <param name="iduser"></param>
        /// <param name="idnhomquyen"></param>
        public void UpdateNhomquyen(int iduser, int idnhomquyen)
        {
            try
            {
                var cb = context.Canbos.FirstOrDefault(p => p.intid == iduser);
                cb.intnhomquyen = idnhomquyen;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// cap nhat avatar cua user
        /// </summary>
        /// <param name="iduser"></param>
        /// <param name="strImageProfile"></param>
        public void UpdateImageProfile(int iduser, string strImageProfile)
        {
            try
            {
                var cb = context.Canbos.FirstOrDefault(p => p.intid == iduser);
                cb.strImageProfile = strImageProfile;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
