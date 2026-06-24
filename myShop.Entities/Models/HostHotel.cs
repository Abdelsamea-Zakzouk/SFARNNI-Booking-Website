using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using myShop.Entities.Models;

namespace myShop.Entities.ViewModels
{
    public class HostHotel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public virtual Product Product { get; set; }  // Make navigation property virtual

        [Range(1, 15, ErrorMessage = "You must enter a value between 1 and 15")]
        public int Count { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public virtual ApplicationUsers ApplicationUsers { get; set; }  // Make navigation property virtual

    
    }
}

