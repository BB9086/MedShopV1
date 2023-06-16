using MedShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedShop.ViewModels
{
    public class UserClaimsViewModel
    {
        public bool IsSelected { get; set; }
        public string ClaimType { get; set; }
    }
}
