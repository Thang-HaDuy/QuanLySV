using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.Areas.HoSoHS.Models
{
    [Table("HoSoHS")]
    public class HoSoHS
    {
        public Guid Id { get; set; }

        [Required]
        public Guid LopHocId {set; get;} 

        [ForeignKey("LopHocId")]
        public LopHoc LopHoc {set; get;}  

        [Required]
        public string HocSinhId {set; get;}

        [ForeignKey("HocSinhId")]
        public AppUser HocSinh {set; get;}  
        public DateTime NgayNhapHoc {set; get;}  

        [Display(Name = "Điểm Tổng Kết")]
        [Range(1,10, ErrorMessage = "{0} dài {1} đến {2}")]
        public float? DiemTongKet {set; get; }

        [Display(Name="Chuỗi định danh (url)", Prompt = "Nhập hoặc để trống tự phát sinh")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        public string? Slug {set; get;}
    }
}
