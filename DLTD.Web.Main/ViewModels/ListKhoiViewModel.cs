using System;
using System.ComponentModel.DataAnnotations;
using DLTD.Web.Main.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DLTD.Web.Main.ViewModels
{
    public class ListKhoiViewModel 
    {
        [Display(Name = "Id")]
        public int? Id { get; set; }
        
        public IEnumerable<KhoiViewModel> Khoi { get; set; }

       
    }
    public class KhoiViewModel
    {
        public int? Id { get; set; }
        public string Ten { get; set; }
    }
}