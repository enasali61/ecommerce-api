using Shared.DTOS;
using Shared.OrderModels;

namespace Services.Abstraction
{
    public interface IAuthenticationService
    {
        // login and register 
        public Task<UserResultDTO> LoginAsync(LoginDto loginDto);
        public Task<UserResultDTO> RegisterAsync(RegisterDTO registerDTO);

        // get current user
        public Task<UserResultDTO> GetUserByEmail( string email);   
        // check if email alr exist 
        public Task<bool> ChekEmailIfExist(string email);

        // get user address 
        public Task<AddressDto> GetUserAddress(string email);

        // update user address 
        public Task<AddressDto> UpdateUserAddress(AddressDto addressDto,string email);

    }
}
