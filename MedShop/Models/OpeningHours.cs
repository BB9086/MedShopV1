using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    [Table("tblOpeningHours")]
    public class OpeningHours
    {
        [Key]
        public int Id { get; set; }
        public int DayOfWeek { get; set; }
        public string OpenFrom { get; set; }
        public string OpenTo { get; set; }
        public bool Closed { get; set; }
        public int StoreId { get; set; }

        [ForeignKey("StoreId")]
        public Store Store { get; set; }

    }
}
