using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Service.Data;

namespace ShoppingCart.Service.Pages
{
    public class CartModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public CartModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;

        }

        public Order Order { get;set; }
        public List<OrderItem> Items { get; set; }


        //View current cart
        public async Task OnGet()
        {
            IdentityUser user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return;
            }

            this.Order = await this.context.Orders.FirstOrDefaultAsync(x => x.User == user && !x.IsPaid) 
                         ?? await this.CreateNewOrder(user);

            this.Items = this.context.OrderItems
                             .Include(x=>x.Item)
                             .Where(x => x.Order == this.Order).ToList();
        }

        //Delete the entire cart
        public async Task<IActionResult> OnGetDelete()
        {
            IdentityUser user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.RedirectToPage("/Cart");
            }

            this.Order = await this.context.Orders.FirstOrDefaultAsync(x => x.User == user && !x.IsPaid)
                         ?? await this.CreateNewOrder(user);

            if (this.Order == null)
            {
                return this.RedirectToPage("/Cart");
            }

            IQueryable<OrderItem> orderItems = this.context.OrderItems.Where(x => x.Order == this.Order);

            this.context.OrderItems.RemoveRange(orderItems);
            this.context.Orders.Remove(this.Order);
            await this.context.SaveChangesAsync();

            return this.RedirectToPage("/Cart");
        }

        //Remove an item from the current cart
        public async Task<IActionResult> OnGetRemoveAsync(int id)
        {
            IdentityUser user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.RedirectToPage("/Cart");
            }

            this.Order = await this.context.Orders.FirstOrDefaultAsync(x => x.User == user && !x.IsPaid)
                         ?? await this.CreateNewOrder(user);

            if (this.Order == null)
            {
                return this.RedirectToPage("/Cart");
            }

            OrderItem orderItem = await this.context.OrderItems.FirstOrDefaultAsync(x => x.Order == this.Order && x.Id == id);

            if (orderItem == null)
            {
                return this.RedirectToPage("/Cart");
            }

            this.context.OrderItems.Remove(orderItem);
            await this.context.SaveChangesAsync();
            return this.RedirectToPage("/Cart");
        }

        //mark the order as purchased (TEMP) and redirect to the url display page
        public async Task<IActionResult> OnGetPurchaseAsync()
        {
            IdentityUser user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.RedirectToPage("/Cart");
            }

            this.Order = await this.context.Orders.FirstOrDefaultAsync(x => x.User == user && !x.IsPaid)
                         ?? await this.CreateNewOrder(user);

            if (this.Order == null)
            {
                return this.RedirectToPage("/Cart");
            }

            this.Order.IsPaid = true;
            this.context.Orders.Update(this.Order);
            await this.context.SaveChangesAsync();
            return this.RedirectToPage("/Order", new {id = this.Order.Id});
        }

        //Create a new order if the user doesn't have an active one
        private async Task<Order> CreateNewOrder(IdentityUser user)
        {
            var order = new Order
            {
                User = user,
                IsPaid = false,
            };
            this.context.Orders.Add(order);
            await this.context.SaveChangesAsync();
            return order;
        }
    }
}
