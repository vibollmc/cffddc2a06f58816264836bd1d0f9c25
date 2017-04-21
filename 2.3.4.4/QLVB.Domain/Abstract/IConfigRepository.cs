using QLVB.Domain.Entities;
using System.Linq;

namespace QLVB.Domain.Abstract
{
    public interface IConfigRepository
    {
        IQueryable<Config> Configs { get; }

        void SaveConfig(Config config);

        void SaveConfig(string strthamso, string strgiatri);

        void SaveConfig(int intid, string strgiatri);

        string GetConfig(string strthamso);

        int GetConfigToInt(string strthamso);

        bool GetConfigToBool(string strthamso);
    }
}