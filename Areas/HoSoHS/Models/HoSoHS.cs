using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using App.Models;

namespace App.Areas.HoSoHS.Models
{
    public class HoSoHS
    {
        public Guid Id { get; set; }

        [Required]
        public Guid LopHocId {set; get;} 

        [ForeignKey("LopHocId")]
        public LopHoc LopHoc {set; get;}  

        [Required]
        public string UserId {set; get;}

        [ForeignKey("UserId")]
        public AppUser AppUser {set; get;}  
        public DateTime NgayNhapHoc {set; get;} 
        public float? DiemTongKet {set; get; }
    }
}