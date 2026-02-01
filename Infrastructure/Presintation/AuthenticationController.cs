using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.DTOS;
using Shared.OrderModels;

namespace Presintation
{

    public class AuthenticationController(IServiceManager serviceManager) : ApiController
    {
        // login & register 
        [HttpPost("Login")] // POST : Baseurl/Authentication/Login
        public async Task<ActionResult<UserResultDTO>> Login(LoginDto loginDto)
        {
            var result = await serviceManager.authenticationService.LoginAsync(loginDto);
            return Ok(result);
        }

        [HttpPost("Register")] // POST : Baseurl/Authentication/Register
        public async Task<ActionResult<UserResultDTO>> Register(RegisterDTO registerDTO)
        {
            var result = await serviceManager.authenticationService.RegisterAsync(registerDTO);
            return Ok(result);
        }
        
        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckIfEmailExist(string email)
        {
            var result = await serviceManager.authenticationService.ChekEmailIfExist(email);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserResultDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.authenticationService.GetUserByEmail(email);
            return Ok(result);
        }
       
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = await serviceManager.authenticationService.GetUserAddress(email);
            return Ok(address);
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await serviceManager.authenticationService.UpdateUserAddress(address,email);
            return Ok(result);
        }

    }
}
