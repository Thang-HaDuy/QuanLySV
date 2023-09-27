using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.HSHocSinh.Models
{
    public class LopHoc
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Tên Lớp")]
        public string name { get; set; }
        
        [Required(ErrorMessage = "Phải nhập {0}")]
        public Guid ChuNghiemId {set; get;}
        [ForeignKey("ChuNghiemId")]
        [Display(Name = "Chủ Nhiệm")]
        public ChuNghiem? ChuNghiem {set; get;}
        public ICollection<HocSinh>  hocSinhs { get; set; }

        public LopHoc()
        {
            hocSinhs = new List<HocSinh>();
        }

        public int HocSinhCount
        {
            get { return hocSinhs.Count;}
            
        }
    }
}