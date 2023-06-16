using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class OpeningHoursForStoreId
    {
        public int StoreId { get; set; }
        public List<CreateWorkingHoursViewModel> listOfWorkingHours { get; set; }
    }
}
