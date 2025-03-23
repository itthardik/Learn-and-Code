using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Services.Interface;
using FlipazonApi.Utility;
using Microsoft.AspNetCore.Mvc;

namespace FlipazonApi.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController(IAuthService authService): ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<JsonResult> Login([FromBody] AuthRequest loginRequest)
        {
            try
            {
                return await _authService.Login(loginRequest);

            }
            catch (HttpResponseException ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message}) { StatusCode = 500 };
            }
        }

        [HttpPost("signup")]
        public async Task<JsonResult> SignUp([FromBody]AuthRequest signUpRequest)
        {
            try
            {
                return await _authService.SignUp(signUpRequest);
            }
            catch (HttpResponseException ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = ex.StatusCode };
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ex.Message }) { StatusCode = 500 };
            }
        }


    }
}
