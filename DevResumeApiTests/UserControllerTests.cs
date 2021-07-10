using System;
using Xunit;
using Moq;
using DevResumeApi.Controllers;
using System.Collections.Generic;
using DevResumeApi;
using Microsoft.AspNetCore.Mvc;

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


    }
}
