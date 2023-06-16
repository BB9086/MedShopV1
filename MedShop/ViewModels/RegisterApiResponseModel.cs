using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class RegisterApiResponseModel
    {
        public string ErrorMessage { get; set; }
        public String Status => StatusE.ToString();
        public RegisterResultStatus StatusE { get; set; }
    }
    public enum RegisterResultStatus
    {
        Success = 1,
        InvalidInsertValues = 2

    }
}

