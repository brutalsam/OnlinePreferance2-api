using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlinePreferance2_api.Database;
using OnlinePreferance2_api.Model.Auth;
using OnlinePreferance2_api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebAPI.Model;
using ConfigurationManager = OnlinePreferance2_api.Configuration.ConfigurationManager;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly PreferanceDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthenticationController(UserManager<ApplicationUser> userManager, PreferanceDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<JWTTokenResponse>> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser == null)
            {
                return BadRequest("Bad credentials");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }
            var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (userInDb is null)
                return Unauthorized();

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userInDb.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };


            authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.User));

            var accessToken = TokenService.CreateToken(authClaims);
            var refreshToken = TokenService.GenerateRefreshToken();


            _ = int.TryParse(ConfigurationManager.AppSetting["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            userInDb.RefreshToken = refreshToken;
            userInDb.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            await _userManager.UpdateAsync(userInDb);

            await _context.SaveChangesAsync();
            return Ok(new JWTTokenResponse
            {
                Username = userInDb.UserName,
                Email = userInDb.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken,
            });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = TokenService.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = TokenService.CreateToken(principal.Claims.ToList());
            var newRefreshToken = TokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            var result = await _userManager.CreateAsync(
                new ApplicationUser { UserName = request.Username, Email = request.Email, SecurityStamp = Guid.NewGuid().ToString() },
                request.Password
            );
            if (result.Succeeded)
            {
                request.Password = "";
                return CreatedAtAction(nameof(Register), new { email = request.Email }, request);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }
    }
}
