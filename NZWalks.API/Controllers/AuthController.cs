using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly ITokenRepository tokenRepository;

		public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
			this.userManager = userManager;
			this.tokenRepository = tokenRepository;
		}

		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
		{
			var identityUser = new IdentityUser
			{
				UserName = registerDTO.UserName,
				Email = registerDTO.UserName
			};

			var identityResult = await userManager.CreateAsync(identityUser, registerDTO.Password);

			if (identityResult.Succeeded)
			{
				if (registerDTO.Roles != null && registerDTO.Roles.Any())
				{
					identityResult = await userManager.AddToRolesAsync(identityUser, registerDTO.Roles);

					if (identityResult.Succeeded)
					{
						return Ok("User Created Succesfuly, please login.");
					}
				}
			}

			return BadRequest("Somthing went wrong");
		}

		[HttpPost]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
		{
			var user = await userManager.FindByEmailAsync(loginDTO.UserName);

			if (user != null)
			{
				var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginDTO.Password);

				if (checkPasswordResult)
				{
					var roles = await userManager.GetRolesAsync(user);

					if(roles != null)
					{
						var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());

						var response = new LoginResponseDTO
						{
							JwtToken = jwtToken
						};

						return Ok(response);
					}
				}
			}

			return BadRequest("email or password incorrect");
		}
	}
}
