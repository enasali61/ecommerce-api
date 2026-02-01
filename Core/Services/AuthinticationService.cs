using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Domain.Entities.Identity;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction;
using Shared;
using Shared.DTOS;
using Shared.OrderModels;

namespace Services
{
    public class AuthinticationService(
        UserManager<User> _userManager,
        IOptions<JwtOptions> options,
        IMapper mapper
        ) : IAuthenticationService
    {
        public async Task<bool> ChekEmailIfExist(string email)
        {
            var user = await _userManager.FindByEmailAsync( email );
            return user != null; // user not null = true 
        }

        public async Task<AddressDto> GetUserAddress(string email)
        {
            var user = await _userManager.Users.Include(u  => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new UserNotFoundException(email);
                return mapper.Map<AddressDto>(user.Address);
        }

        public async Task<UserResultDTO> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException(email);
            return new UserResultDTO(user.DisplayName, user.Email!,
                await CreateTokenAsync(user));
            
        }
        public async Task<AddressDto> UpdateUserAddress(AddressDto addressDto, string email)
        {
            var user = await _userManager.Users.Include(u => u.Address)
               .FirstOrDefaultAsync(u => u.Email == email)
               ?? throw new UserNotFoundException(email);

            if (user.Address != null) // update
            {
                user.Address.firstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Street = addressDto.Street;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;

            }
            // set address with new address 
            else
            {
                var userNewAddress = mapper.Map<userAddress>(addressDto);
                user.Address = userNewAddress;
            }
            await _userManager.UpdateAsync(user);
            return mapper.Map<AddressDto>(user.Address);
        }


        public async Task<UserResultDTO> LoginAsync(LoginDto loginDto)
        {
            // check on email & password
            // 1. check if there is user under that email
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnAuthurisedException($"Email {loginDto.Email} does not exist");
            }
            //check if password is correct 
            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                throw new UnAuthurisedException();
            }
            // genrate token & return responce
            return new UserResultDTO(
                user.DisplayName,
                user.Email!,
                await CreateTokenAsync(user)
                );
        }

        public async Task<UserResultDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var user = new User
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                throw new ValidationException(errors);
            }
            return new UserResultDTO(
                user.DisplayName, user.Email, await CreateTokenAsync(user)
                );
        }


        private async Task<string> CreateTokenAsync(User user)
        {
            var JwtOptions = options.Value;
            // create claims
            var authClaims = new List<Claim> {
                new Claim(ClaimTypes.Name,user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email),

            };
            // Add roles to claims if exist 
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // create secret key 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey));
            // create hashing Alghorithem
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // create token
            var token = new JwtSecurityToken
                (
                issuer: JwtOptions.Issuer, //Backend BaseUrl
                audience: JwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddDays(JwtOptions.DurationInDays),
                signingCredentials: cred
                );
            // object member method =. create object from JwtSecurityTokenHandler
            return new JwtSecurityTokenHandler().WriteToken(token);
        }



    }
}
