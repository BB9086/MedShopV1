using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    public class CreateRoleViewModel
    {[Required(ErrorMessage="Ime uloge je obavezno")]
        public string Name { get; set; }
    }
}
