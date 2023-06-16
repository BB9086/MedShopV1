using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    public interface IOpeningHoursRepository
    {
        OpeningHours GetOpeningHourByDayOfWeekAndStoreId(int dayOfWeek,int storeType);
        bool IsStoreClosed(int dayOfWeek, int storeType);
        OpeningHours InsertOpeningHour(OpeningHours openingHours);
    }
}
