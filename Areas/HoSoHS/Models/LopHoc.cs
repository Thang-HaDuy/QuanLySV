using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.HoSoHS.Models
{
    public class LopHoc
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Nhập mật Tên Lớp")]
        public string name { get; set; }

        [Display(Name="Chuỗi định danh (url)", Prompt = "Nhập hoặc để trống tự phát sinh theo Name")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        public string? Slug {set; get;}
        public ICollection<HoSoHS> HoSoHS { get; set; }

        public LopHoc()
        {
            HoSoHS = new List<HoSoHS>();
        }

        public int HocSinhCount
        {
            get { return HoSoHS.Count;}
            
        }
    }
}