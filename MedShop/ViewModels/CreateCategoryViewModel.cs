using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class CreateCategoryViewModel
    {
        [Required(ErrorMessage = "Ime kategorije je obavezno")]
        public string CategoryName { get; set; }
        public int StoreType { get; set; }
        
    }
}
