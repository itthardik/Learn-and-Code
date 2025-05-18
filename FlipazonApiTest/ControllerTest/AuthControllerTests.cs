using FlipazonApi.Controllers;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Services.Interface;
using FlipazonApi.Utility;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;

namespace FlipazonApiTest.ControllerTest
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _authController;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _authController = new AuthController(_authServiceMock.Object);
        }

        [Fact]
        public async Task Login_ReturnsSuccessJsonResult()
        {
            var loginRequest = new AuthRequest { Email = "user@test.com", Password = "pass" };
            var expectedResponse = new JsonResult(new { Message = "Login Successfull", Data = new { UserId = 1, loginRequest.Email } });

            _authServiceMock.Setup(s => s.Login(loginRequest)).ReturnsAsync(expectedResponse);

            var result = await _authController.Login(loginRequest);

            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Login Successfull", data["Message"]);
            Assert.Equal("user@test.com", data["Data"]?["Email"]);
        }

        [Fact]
        public async Task Login_HttpResponseException_ReturnsErrorJsonResult()
        {
            var loginRequest = new AuthRequest { Email = "user@test.com", Password = "wrong" };
            var exception = new HttpResponseException(401, "Invalid credentials");

            _authServiceMock.Setup(s => s.Login(loginRequest)).ThrowsAsync(exception);

            var result = await _authController.Login(loginRequest);

            Assert.Equal(401, result.StatusCode);
            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Invalid credentials", data["Message"]);
        }

        [Fact]
        public async Task Login_UnexpectedException_Returns500JsonResult()
        {
            var loginRequest = new AuthRequest { Email = "user@test.com", Password = "pass" };

            _authServiceMock.Setup(s => s.Login(loginRequest)).ThrowsAsync(new Exception("Something went wrong"));

            var result = await _authController.Login(loginRequest);

            Assert.Equal(500, result.StatusCode);
            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Something went wrong", data["Message"]);
        }

        [Fact]
        public async Task SignUp_ReturnsSuccessJsonResult()
        {
            var signUpRequest = new AuthRequest { Email = "new@test.com", Password = "pass" };
            var expectedResponse = new JsonResult(new { Message = "Signup successful" });

            _authServiceMock.Setup(s => s.SignUp(signUpRequest)).ReturnsAsync(expectedResponse);

            var result = await _authController.SignUp(signUpRequest);

            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Signup successful", data["Message"]);
        }

        [Fact]
        public async Task SignUp_HttpResponseException_ReturnsErrorJsonResult()
        {
            var signUpRequest = new AuthRequest { Email = "existing@test.com", Password = "pass" };
            var exception = new HttpResponseException(409, "Email already exists");

            _authServiceMock.Setup(s => s.SignUp(signUpRequest)).ThrowsAsync(exception);

            var result = await _authController.SignUp(signUpRequest);

            Assert.Equal(409, result.StatusCode);
            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Email already exists", data["Message"]);
        }
    }
}
