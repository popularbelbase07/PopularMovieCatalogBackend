using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PopularMovieCatalogBackend.DTOs.Auth;
using PopularMovieCatalogBackend.DTOs.Genre;
using PopularMovieCatalogBackend.Helpers.Pagination;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PopularMovieCatalogBackend.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountsControllers : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AccountsControllers(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("listUsers")]
        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme, Policy ="IsAdmin")]
        public async Task<ActionResult<List<UserDTO>>> GetListUsers([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Users.AsQueryable();
            await HttpContext.InsertParametsrPaginationInHeader(queryable);
            var users = await queryable.OrderBy(x => x.Email).Paginate(paginationDTO).ToListAsync();
            return mapper.Map<List<UserDTO>>(users);
        }

        [HttpPost("makeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task <ActionResult> MakeAdmin([FromBody] string userId){
        
            var user = await userManager.FindByIdAsync(userId);
            await userManager.AddClaimAsync(user, new Claim("role", "admin"));
            return NoContent(); 
        }


        [HttpPost("removeAdmin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
        public async Task<ActionResult> RemoveAdmin([FromBody] string userId)
        {

            var user = await userManager.FindByIdAsync(userId);
            await userManager.RemoveClaimAsync(user, new Claim("role", "admin"));
            return NoContent();
        }




        [HttpPost("create")]
        public async Task<ActionResult<AuthenticationResponse>> CreateUser([FromBody] UserCredentials userCredentials)
        {
            var user = new IdentityUser {UserName = userCredentials.Email, Email = userCredentials.Email };
            var result = await userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
            {
                await BuildTokenForAuthentication(userCredentials);
            }
            else
            {
                return BadRequest(result.Errors);
            }

            return Ok();

        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserCredentials userCredentials)
        {
            var result = await signInManager.PasswordSignInAsync(userCredentials.Email,
                userCredentials.Password, isPersistent: false, lockoutOnFailure: false);

            if(result.Succeeded) {
                return await BuildTokenForAuthentication(userCredentials);
            }
            else
            { 
                return BadRequest("Incorrect Login !!!!!!");
            }

        }
            private async Task<AuthenticationResponse> BuildTokenForAuthentication(UserCredentials userCredentials)
            {
            // json web token contains claims 3 parts => header, signature and payload data(contains claim)
            // use minimum necessory identification
            var claims = new List<Claim>() {
                new Claim ("email", userCredentials.Email)
            };

            var user = await userManager.FindByNameAsync(userCredentials.Email);
            var claimsDB = await userManager.GetClaimsAsync(user);
            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMonths(6);

            // create a token
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires:expiration, signingCredentials: creds);
            return new AuthenticationResponse()
            {
                //string representation of token and save it
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
            };

        }
    }
}
