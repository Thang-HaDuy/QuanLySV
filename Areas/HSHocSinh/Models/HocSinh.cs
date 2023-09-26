using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.Areas.HSHocSinh.Models
{
    public class HocSinh 
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Phải nhập {0}")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Nhập mật Tên Lớp")]
        public string name { get; set; }
        
        [Column(TypeName = "nvarchar")]
        [StringLength(400)]  
        public string? HomeAdress { get; set; }
      
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }
        
        [Range(1, 100, ErrorMessage = "Giá trị {0}  phải nằm trong khoảng từ {2} đến {1}")]
        public int? SDT { get; set; }

        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Giới Tính")]
        public string? Gender { get; set; }


        public Guid LopHocId {set; get;}
        [ForeignKey("LopHocId")]
        [Display(Name = "Lớp Học")]
        public LopHoc LopHoc {set; get;}


    }
}