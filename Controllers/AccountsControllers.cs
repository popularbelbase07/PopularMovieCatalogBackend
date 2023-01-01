using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PopularMovieCatalogBackend.DTOs.Auth;
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

        public AccountsControllers(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("create")]
        public async Task<ActionResult<AuthenticationResponse>> CreateUser([FromBody] UserCredentials userCredentials)
        {
            var user = new IdentityUser {UserName = userCredentials.Email, Email = userCredentials.Email };
            var result = await userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
            {
                BuildTokenForAuthentication(userCredentials);
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
                return BuildTokenForAuthentication(userCredentials);
            }
            else
            { 
                return BadRequest("Incorrect Login !!!!!!");
            }

        }


            private AuthenticationResponse BuildTokenForAuthentication(UserCredentials userCredentials)
            {
            // json web token contains claims 3 parts => header, signature and payload data(contains claim)
            // use minimum necessory identification
            var claims = new List<Claim>() {
                new Claim ("email", userCredentials.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);   

            var expiration = DateTime.UtcNow.AddYears(1);

            // create a token
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires:expiration, signingCredentials: credintials);

            return new AuthenticationResponse()
            {
                //string representation of token and save it
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
            };

        }
    }
}
