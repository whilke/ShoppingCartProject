using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Service.Data
{
    public class Item
    {
        public int Id { get;set; }
        [Required] public string Name { get; set; }
        [Required] public Double Price { get; set; }
        [Required] public string Url { get; set; }
    }
}