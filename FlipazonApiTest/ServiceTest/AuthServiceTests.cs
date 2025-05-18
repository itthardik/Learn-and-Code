using FlipazonApi.Models;
using FlipazonApi.Models.DTO.RequestDTO;
using FlipazonApi.Repository.Interface;
using FlipazonApi.Services;
using FlipazonApi.Utility;
using Moq;
using Newtonsoft.Json.Linq;

namespace FlipazonApiTest.ServiceTest
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _authService = new AuthService(_userRepositoryMock.Object);
        }

        [Fact]
        public async Task Login_Successful_ReturnsJsonResult()
        {
            var loginRequest = new AuthRequest { Email = "test@example.com", Password = "pass123" };
            var user = new User("test@example.com", "pass123");

            _userRepositoryMock.Setup(repo => repo.GetUser(loginRequest.Email))
                               .ReturnsAsync(user);

            var result = await _authService.Login(loginRequest);

            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Login Successfull", data["Message"]);
            Assert.Equal(0, data["Data"]?["userId"]);
            Assert.Equal("test@example.com", data["Data"]?["email"]);
        }

        [Fact]
        public async Task Login_WrongPassword_Throws401()
        {
            var loginRequest = new AuthRequest { Email = "test@example.com", Password = "wrong" };
            var user = new User("test@example.com", "correct");

            _userRepositoryMock.Setup(repo => repo.GetUser(loginRequest.Email))
                               .ReturnsAsync(user);

            var ex = await Assert.ThrowsAsync<HttpResponseException>(() => _authService.Login(loginRequest));
            Assert.Equal(401, ex.StatusCode);
            Assert.Equal("Invalid Password", ex.Message);
        }

        [Fact]
        public async Task Login_UserNotFound_Throws404()
        {
            var loginRequest = new AuthRequest { Email = "unknown@example.com", Password = "pass" };

            _userRepositoryMock.Setup(repo => repo.GetUser(loginRequest.Email))
                               .ReturnsAsync((User?)null);

            var ex = await Assert.ThrowsAsync<HttpResponseException>(() => _authService.Login(loginRequest));
            Assert.Equal(404, ex.StatusCode);
            Assert.Equal("User not found", ex.Message);
        }

        [Fact]
        public async Task SignUp_Successful_ReturnsJsonResult()
        {
            var signUpRequest = new AuthRequest { Email = "new@example.com", Password = "pass123" };

            _userRepositoryMock.Setup(repo => repo.GetUser(signUpRequest.Email))
                               .ReturnsAsync((User?)null);

            _userRepositoryMock.Setup(repo => repo.AddUser(signUpRequest.Email, signUpRequest.Password))
                               .ReturnsAsync(1);

            var result = await _authService.SignUp(signUpRequest);

            Assert.NotNull(result.Value);
            var data = JObject.FromObject(result.Value);
            Assert.Equal("Signup successful", data["Message"]);
        }

        [Fact]
        public async Task SignUp_EmailExists_Throws409()
        {
            var signUpRequest = new AuthRequest { Email = "exists@example.com", Password = "pass" };
            var existingUser = new User("exists@example.com", "pass");

            _userRepositoryMock.Setup(repo => repo.GetUser(signUpRequest.Email))
                               .ReturnsAsync(existingUser);

            var ex = await Assert.ThrowsAsync<HttpResponseException>(() => _authService.SignUp(signUpRequest));
            Assert.Equal(409, ex.StatusCode);
            Assert.Equal("Email already exists", ex.Message);
        }
    }
}