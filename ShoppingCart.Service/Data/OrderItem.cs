using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Service.Data
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Required] public Order Order { get; set; }
        [Required] public Item Item { get; set; }
        [Required] public int Count { get; set; }
    }
}