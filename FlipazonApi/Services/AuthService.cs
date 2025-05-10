using FlipazonApi.Models;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Repository.Interface;
using FlipazonApi.Services.Interface;
using FlipazonApi.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace FlipazonApi.Services
{
    public class AuthService(IUserRepository userRepository) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<JsonResult> Login(AuthRequest loginRequest)
        {
            User foundUser = await _userRepository.GetUser(loginRequest.Email)??
                                throw new HttpResponseException(404, "User not found");

            if (foundUser.Password != loginRequest.Password)
                throw new HttpResponseException(401,"Invalid Password");

            return new JsonResult(new { Message = "Login Successfull", Data = new { userId = foundUser.Id , email = foundUser.Email } });
        }

        public async Task<JsonResult> SignUp(AuthRequest signUpRequest)
        {
            User? existingUser = await _userRepository.GetUser(signUpRequest.Email);
             
            if (existingUser != null)
                throw new HttpResponseException(409,"Email already exists");

            await _userRepository.AddUser(signUpRequest.Email, signUpRequest.Password);

            return new JsonResult(new { Message = "Signup successful" });
        }
    }
}
