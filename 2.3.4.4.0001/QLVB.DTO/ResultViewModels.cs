using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLVB.DTO
{
    /// <summary>
    /// cac ket qua json return
    /// </summary>
    public enum ResultViewModels
    {
        Error = 0,
        Success = 1,
        ErrorForeignKey = 2
    }
    /// <summary>
    /// cac ket qua return cua ham
    /// </summary>
    public class ResultFunction
    {
        public int id { get; set; }
        public string message { get; set; }
    }
}
