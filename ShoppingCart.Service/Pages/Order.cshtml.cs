using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Service.Data;

namespace ShoppingCart.Service.Pages
{
    public class OrderModel : PageModel
    {
        private readonly ShoppingCart.Service.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public Order Order {get; set; }
        public List<OrderItem> Items { get; set; }

        public OrderModel(ShoppingCart.Service.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet(int id)
        {

            var user = await this.GetUser();
            if (user == null)
            {
                return this.RedirectToPage("/Cart");
            }

            Order order = await this._context.Orders.FirstOrDefaultAsync(o => o.User == user && o.IsPaid && o.Id == id);
            if (order == null)
            {
                return this.RedirectToPage("/Cart");
            }
            this.Order = order;

            this.Items = this._context.OrderItems
                             .Include(x => x.Item)
                             .Where(x => x.Order == this.Order).ToList();
            return Page();
        }


        private async Task<IdentityUser> GetUser()
        {
            IdentityUser user = await this.userManager.GetUserAsync(this.User);
            return user;
        }
    }
}
