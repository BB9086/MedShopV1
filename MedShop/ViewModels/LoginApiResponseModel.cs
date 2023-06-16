using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class LoginApiResponseModel
    {
        public string Cookie { get; set; }
       
        public String Status => StatusE.ToString();
        public LoginResultStatus StatusE { get; set; }
    }
    public enum LoginResultStatus
    {
        Success = 1,
        WrongCredentials = 2,
        EmailNotConfirmed=3

    }
}
