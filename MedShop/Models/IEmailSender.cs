using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.Models
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
