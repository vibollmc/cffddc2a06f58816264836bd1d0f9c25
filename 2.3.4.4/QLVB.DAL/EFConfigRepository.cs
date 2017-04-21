using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLVB.Domain.Abstract;
using QLVB.Domain.Entities;

namespace QLVB.DAL
{
    public class EFConfigRepository : IConfigRepository
    {
        private QLVBDatabase context;

        public EFConfigRepository(QLVBDatabase _context)
        {
            context = _context;
        }

        public IQueryable<Config> Configs
        {
            get
            {
                return context.Configs.AsNoTracking()
                    .Where(p => p.inttrangthai == (int)enumConfig.inttrangthai.IsActive); //(int)EnumDanhmuc.inttrangthai_config.IsActive);
            }
        }

        public void SaveConfig(Config _config)
        {
            Config cf = context.Configs.FirstOrDefault(p => p.strthamso == _config.strthamso);
            cf.strgiatri = _config.strgiatri;
            context.SaveChanges();
        }

        public void SaveConfig(string strthamso, string strgiatri)
        {
            Config cf = context.Configs
                //.AsNoTracking()
                        .FirstOrDefault(p => p.strthamso == strthamso);
            cf.strgiatri = strgiatri;
            context.SaveChanges();
        }

        public void SaveConfig(int intid, string strgiatri)
        {
            Config cf = context.Configs.FirstOrDefault(p => p.intid == intid);
            cf.strgiatri = strgiatri;
            context.SaveChanges();
        }

        public string GetConfig(string strthamso)
        {
            try
            {
                return context.Configs
                .FirstOrDefault(p => p.strthamso == strthamso)
                .strgiatri;
            }
            catch
            {
                return string.Empty;
            }

        }

        public int GetConfigToInt(string strthamso)
        {
            try
            {
                string strgiatri = context.Configs
                    .FirstOrDefault(p => p.strthamso == strthamso)
                    .strgiatri;
                int intgiatri = Convert.ToInt32(strgiatri);
                return intgiatri;
            }
            catch
            {
                return 0;
            }

        }

        public bool GetConfigToBool(string strthamso)
        {
            try
            {
                string strgiatri = context.Configs
                   .FirstOrDefault(p => p.strthamso == strthamso)
                   .strgiatri;
                bool isgiatri = (strgiatri.ToLower() == "true") ? true : false;
                return isgiatri;
            }
            catch
            {
                return false;
            }

        }
    }
}
