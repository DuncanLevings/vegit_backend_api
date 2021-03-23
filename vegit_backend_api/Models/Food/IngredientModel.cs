using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace vegit_backend_api.Models.Food
{
    public class IngredientModel : IModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        [Required]
        public int diet_type { get; set; }

        public string diet_name { get; set; }

        public int diet_level { get; set; }

        [StringLength(5000)]
        public string description { get; set; }

        [StringLength(50)]
        public string public_Id { get; set; }

        public int public_Id_Int { get; set; }

        [StringLength(50)]
        public string group { get; set; }

        [StringLength(50)]
        public string sub_group { get; set; }

        [Required]
        [StringLength(50)]
        public string created_date { get; set; }

        [Required]
        [StringLength(50)]
        public string updated_date { get; set; }

        [Required]
        public int data_source { get; set; }
    }
}
