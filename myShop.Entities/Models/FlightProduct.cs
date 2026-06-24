using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace myShop.Entities.Models
{
    public class FlightProduct
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Flight Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [DisplayName("Departure Location")]
        public string LocationFrom { get; set; }

        [Required]
        [DisplayName("Destination Location")]
        public string LocationTo { get; set; }

        [Required]
        [DisplayName("Ticket Price")]
        public decimal TicketPrice { get; set; }

        [ValidateNever]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}
