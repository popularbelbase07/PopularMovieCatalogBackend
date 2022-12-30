using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PopularMovieCatalogBackend.DTOs.Auth;

namespace PopularMovieCatalogBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsControllers : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountsControllers(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<AuthenticationResponse>> Create([FormBody] UserCredentials userCredentials)

    }
}
