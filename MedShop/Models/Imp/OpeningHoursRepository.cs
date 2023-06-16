using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models.Imp
{
    public class OpeningHoursRepository : IOpeningHoursRepository
    {
        private readonly CoreDBContext context;

        public OpeningHoursRepository(CoreDBContext context)
        {
            this.context = context;
        }
        public OpeningHours GetOpeningHourByDayOfWeekAndStoreId(int dayOfWeek, int storeType)
        {
            return context.OpeningHourss.FirstOrDefault(x => x.DayOfWeek == dayOfWeek && x.StoreId == storeType);
        }

        public OpeningHours InsertOpeningHour(OpeningHours openingHours)
        {
            context.OpeningHourss.Add(openingHours);
            context.SaveChanges();
            return openingHours;
        }

        public bool IsStoreClosed(int dayOfWeek, int storeType)
        {
            return context.OpeningHourss.FirstOrDefault(x => x.DayOfWeek == dayOfWeek && x.StoreId == storeType).Closed;
        }
    }
}
