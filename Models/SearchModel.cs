using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models
{
    public class SearchModel
    {
        public string action { get; set; }
        public string Controller { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }
}