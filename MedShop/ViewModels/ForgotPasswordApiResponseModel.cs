using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class ForgotPasswordApiResponseModel
    {
            public String Status => StatusE.ToString();
            public ForgotPasswordResultStatus StatusE { get; set; }
       
       
    }
    public enum ForgotPasswordResultStatus
    {
        Success = 1,
        WrongCredentials = 2

    }
}
