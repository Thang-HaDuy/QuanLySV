using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Areas.HoSoHS.Models;
using Microsoft.AspNetCore.Identity;

namespace App.Models 
{
    public class AppUser: IdentityUser 
    {
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


        public ICollection<HoSoHS> HoSoHS { get; set; }


        public int LopHocCount
        {
            get { return HoSoHS.Count; }
        }
    }
}
