using System;
using System.ComponentModel.DataAnnotations;
using DLTD.Web.Main.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DLTD.Web.Main.ViewModels
{
    public class ListDonViViewModel
    {
        [Display(Name = "Id")]
        public int? Id { get; set; }
        public IEnumerable<DonViViewModel> DonVi{ get; set; }
    }
    public class DonViViewModel
    {
        public int? Id { get; set; }
        public string Ten { get; set; }
    }
}