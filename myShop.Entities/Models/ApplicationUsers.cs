using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace myShop.Entities.Models;

public class ApplicationUsers : IdentityUser
{
    [Required]
    public String Name { get; set; }
    public String City { get; set; }

    public String Address { get; set; }

}
