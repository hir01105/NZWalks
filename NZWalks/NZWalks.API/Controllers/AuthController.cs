using System;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : Controller
	{
		private readonly IUserRepository userRepository;
        private readonly ITokenHandler tokenHandler;

        public AuthController(IUserRepository userRepository, ITokenHandler tokenHandler)
		{
			this.userRepository = userRepository;
			this.tokenHandler = tokenHandler;
		}


		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> LoginAsync(Models.DTO.LoginRequest loginRequest)
		{
			// Validate the incoming request
			if (loginRequest == null)
			{
				return BadRequest("Username or Password is incorrect");
			}

            // Check if user is authenticated
            // Check username and password
            var isAuthenticated = await userRepository.AuthenticateAsync(loginRequest.Username, loginRequest.Password);

			if (isAuthenticated)
			{
				// Generate a JWT Token
			}

			return BadRequest("Username or Password is incorrect");
		}

		
	}
}

