using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class CreateWorkingHoursViewModel
    {
        public int DayOfWeek { get; set; }
        public string OpenFrom { get; set; }
        public string OpenTo { get; set; }
        public bool Closed { get; set; }
        public int StoreId { get; set; }

    }
}
