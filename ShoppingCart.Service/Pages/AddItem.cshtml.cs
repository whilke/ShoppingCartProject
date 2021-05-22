using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Service.Data;

namespace ShoppingCart.Service.Pages
{
    public class AddItemModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public AddItemModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public IList<Item> Item { get;set; }

        public async Task OnGetAsync()
        {
            Item = await _context.Items.ToListAsync();
        }

        public async Task<IActionResult> OnGetBuyAsync(int id)
        {
            IdentityUser user = await this.GetUser();
            if (user == null)
            {
                return this.RedirectToPage("/Cart");
            }

            Order order = await this._context.Orders.FirstOrDefaultAsync(o => o.User == user && !o.IsPaid);
            if (order == null)
            {
                return this.RedirectToPage("/Cart");
            }

            var item = await this._context.Items.FirstOrDefaultAsync(o => o.Id == id);
            if (item == null)
            {
                return this.RedirectToPage("/Cart");
            }

            var orderItem = new OrderItem
            {
                Item = item,
                Order = order,
                Count = 1
            };
            this._context.OrderItems.Add(orderItem);
            await this._context.SaveChangesAsync();
            return this.RedirectToPage("/Cart");
        }

        private async Task<IdentityUser> GetUser()
        {
            IdentityUser user = await this.userManager.GetUserAsync(this.User);
            return user;
        }
    }
}
