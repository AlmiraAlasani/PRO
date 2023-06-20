using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using PRO.Controllers;
using PRO.DTOs;
using PRO.Models;

namespace ProTesting
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<IConfiguration> _configurationMock;

        public AuthControllerTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _controller = new AuthController(_configurationMock.Object);
        }

        [Fact]
        public void Register_ReturnsOkResultWithUser()
        {
            // Arrange
            var userDTO = new UserDTO
            {
                Username = "testuser",
                PasswordHash = "testpassword"
            };

            // Act
            var result = _controller.Register(userDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUser = Assert.IsType<User>(okResult.Value);
            Assert.Equal(userDTO.Username, returnedUser.Username);
            Assert.NotNull(returnedUser.PasswordHash);
        }

        [Fact]
        public void Login_WithValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var userDTO = new UserDTO
            {
                Username = "testuser",
                PasswordHash = "testpassword"
            };

            AuthController.user = new User
            {
                Username = userDTO.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.PasswordHash)
            };

            // Act
            var result = _controller.Login(userDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedToken = Assert.IsType<string>(okResult.Value);
            Assert.NotEmpty(returnedToken);
        }

        [Fact]
        public void Login_WithInvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var userDTO = new UserDTO
            {
                Username = "testuser",
                PasswordHash = "wrongpassword"
            };

            AuthController.user = new User
            {
                Username = "testuser",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("testpassword")
            };

            // Act
            var result = _controller.Login(userDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

       

    }
}
