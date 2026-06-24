using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using myShop.Entities.Models;

namespace myShop.Entities.ViewModels
{
    public class FlightProductVM
    {
        public FlightProduct FlightProduct { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
