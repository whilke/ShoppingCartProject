using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ShoppingCart.Service.Data
{
    public class Order
    {
        public int Id { get; set; }
        [Required] public DateTime Date { get;set;}
        [Required] public IdentityUser User { get; set; }
        public bool IsPaid { get; set; }
    }
}