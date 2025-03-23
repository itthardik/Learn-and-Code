using FlipazonApi.Models.DTO.RequestDTO;
using Microsoft.AspNetCore.Mvc;

namespace FlipazonApi.Services.Interface
{
    public interface IAuthService
    {
        public Task<JsonResult> Login(AuthRequest loginRequest);
        public Task<JsonResult> SignUp(AuthRequest signUpRequest);
    }
}
