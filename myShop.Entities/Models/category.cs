using System;
using System.ComponentModel.DataAnnotations;

namespace myShop.Entities.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public String Description { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public virtual ICollection<Product>? Products { get; set; }
}
