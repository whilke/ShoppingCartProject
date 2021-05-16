using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCart.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EasyAuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public EasyAuthController(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "EasyAuth")]
        public async Task<RedirectResult> Get(string redirectUrl = null)
        {
            await this.signInManager.UpdateExternalAuthenticationTokensAsync(new ExternalLoginInfo(this.User, "EasyAuth",
                                                                           this.User.Identity.Name,
                                                                           this.User.Identity.Name));
            return this.Redirect(redirectUrl);
        }
    }
}
