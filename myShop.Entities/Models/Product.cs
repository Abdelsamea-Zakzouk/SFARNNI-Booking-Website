using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace myShop.Entities.Models;

public class Product
{

    public int Id { get; set; }
    [Required]
    [DisplayName("Hotel Name ")]
    public string Name { get; set; }

    public String Description { get; set; }
    [ValidateNever]
    [DisplayName("Hotel Image")]
    public String Image { get; set; }

    [Required]
    public string Location { get; set; }
    [Required]
    public Decimal PricePerNight { get; set; }
    [Required]
    [DisplayName("Category")]
    public int CategoryId { get; set; }
    [ValidateNever]
    [JsonIgnore]
    public virtual Category Category { get; set; }
}
