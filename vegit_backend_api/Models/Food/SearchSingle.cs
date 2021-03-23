using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vegit_backend_api.Models.Food
{
    public class SearchSingle
    {
        [Required]
        [StringLength(255)]
        public string name { get; set; }
    }
}

