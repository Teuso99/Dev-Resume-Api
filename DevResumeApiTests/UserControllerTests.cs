using System;
using Xunit;
using Moq;
using DevResumeApi.Controllers;
using System.Collections.Generic;
using DevResumeApi;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DevResumeApiTests
{
    public class UserControllerTests
    {
        private readonly UserController _userController;
        private readonly Mock<List<User>> _mockUsersList;

        public UserControllerTests()
        {
            _mockUsersList = new Mock<List<User>>();
            _userController = new UserController(_mockUsersList.Object);
        }

        [Fact]
        public void GetUsers_ReturnListOfUsers()
        {
            // Arrange
            var mockUsers = new List<User>
            {
                new User{Email = "email1@email.com"},
                new User{Email = "email2@email.com"}
            };

            _mockUsersList.Object.AddRange(mockUsers);
            // Act
            var result = _userController.Get();

            // Assert
            var model = Assert.IsAssignableFrom<ActionResult<List<User>>>(result);
            Assert.Equal(2, model.Value.Count);

        }

        [Fact]
        public void GetUserById_ReturnsNotFound_WhenIdNotProvided()
        {
            // Arrange

            // Act
            var result = _userController.GetById(null);

            // Assert
            Assert.IsAssignableFrom<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var user = new User() { Id = new Guid("0f8fad5b-d9cb-469f-a165-70867728950e") };

            _mockUsersList.Object.SingleOrDefault(m => m.Id == user.Id);

            // Act
            var result = _userController.GetById(user.Id);

            // Assert
            Assert.IsAssignableFrom<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void GetUserById_ReturnsSingleUser_WhenUserExist()
        {
            // Arrange
            var singleMockUser = new User() { Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), 
                Email = "emailcompleto@email.com", FirstName = "Mateus", LastName = "Machado", 
                Password = "senha123" };

            _mockUsersList.Object.Add(singleMockUser);

            // Act
            var result = _userController.GetById(singleMockUser.Id);

            // Assert
            var model = Assert.IsType<ActionResult<User>>(result);
            Assert.Equal(singleMockUser, model.Value);
        }
    }
}
