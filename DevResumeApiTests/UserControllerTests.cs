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
            var result = _userController.GetAllUsers();

            // Assert
            var model = Assert.IsAssignableFrom<ActionResult<List<User>>>(result);
            Assert.Equal(2, model.Value.Count);

        }

        [Fact]
        public void GetUserById_ReturnsNotFound_WhenIdNotProvided()
        {
            // Arrange

            // Act
            var result = _userController.GetUserById(null);

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
            var result = _userController.GetUserById(user.Id);

            // Assert
            Assert.IsAssignableFrom<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public void GetUserById_ReturnsSingleUser_WhenUserExist()
        {
            // Arrange
            var singleMockUser = new User() 
            { 
                Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), 
                Email = "emailcompleto@email.com", 
                FirstName = "Mateus", 
                LastName = "Machado", 
                Password = "senha123" 
            };

            _mockUsersList.Object.Add(singleMockUser);

            // Act
            var result = _userController.GetUserById(singleMockUser.Id);

            // Assert
            var model = Assert.IsType<ActionResult<User>>(result);
            Assert.Equal(singleMockUser, model.Value);
        }

        [Fact]
        public void PostUser_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var incompleteUser = new User()
            {
                Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                FirstName = "Jorge",
                LastName = "Silva",
                Password = "outrasenha456"
            };

            _userController.ModelState.AddModelError("Email", "Email field is required!");

            // Act
            var result = _userController.PostUser(incompleteUser);

            // Assert
            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public void PostUser_ReturnsCreatedResponse_WhenValidObjectPassed()
        {
            // Arrange
            var mockUser = new User()
            {
                FirstName = "Carlos",
                LastName = "Souza",
                Email = "novoemail1@email.com",
                Password = "novasenha100"
            };

            // Act
            mockUser.Id = Guid.NewGuid();
            var result = _userController.PostUser(mockUser);

            // Assert
            Assert.IsAssignableFrom<CreatedAtActionResult>(result);
        }

        [Fact]
        public void PostUser_ReturnsResponseHasCreatedItem_WhenValidObjectPassed()
        {
            // Arrange
            var mockUser = new User()
            {
                Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                FirstName = "Carlos",
                LastName = "Souza",
                Email = "novoemail2@email.com",
                Password = "novasenha2"
            };

            // Act
            var result = _userController.PostUser(mockUser) as CreatedAtActionResult;
            var item = result.Value as User;

            // Assert
            Assert.IsType<User>(item);
            Assert.Equal("novoemail2@email.com", item.Email);
        }

        [Fact]
        public void RemoveUser_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var notExistingGuid = Guid.NewGuid();

            // Act
            var result = _userController.DeleteUser(notExistingGuid);

            // Assert
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        [Fact]
        public void RemoveUser_ReturnsOkResult_WhenIdExist()
        {
            // Arrange
            var mockUser = new User()
            {
                Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                FirstName = "Carlos",
                LastName = "Souza",
                Email = "novoemail2@email.com",
                Password = "novasenha2"
            };

            _mockUsersList.Object.Add(mockUser);

            // Act
            var result =_userController.DeleteUser(mockUser.Id);

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public void RemoveUser_RemovesOneItem_WhenIdExist()
        {
            // Arrange
            var mockUser = new List<User>()
            {
                new User()
                {
                    Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    FirstName = "Carlos",
                    LastName = "Souza",
                    Email = "novoemail1@email.com",
                    Password = "novasenha100"
                },

                new User()
                { 
                    Id = new Guid(),
                    FirstName = "Carlos",
                    LastName = "Souza",
                    Email = "novoemail2@email.com",
                    Password = "novasenha2"
                }
            };

            _mockUsersList.Object.AddRange(mockUser);

            // Act
            _userController.DeleteUser(new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"));

            // Assert
            Assert.Single(_userController.GetAllUsers().Value);
        }

        [Fact]
        public void PutUser_ReturnsNotFound_WhenIdAndUserIsNull()
        {
            // Act
            var result = _userController.PutUser(null, null);

            // Assert
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PutUser_ReturnsNotFound_WhenUserIsNull()
        {
            // Arrange
            var mockUserId = Guid.NewGuid();

            // Act
            var result = _userController.PutUser(mockUserId, null);

            // Assert
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PutUser_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var mockUser = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Carlos",
                LastName = "Souza",
                Email = "novoemail1@email.com",
                Password = "novasenha100"
            };

            // Act
            var result = _userController.PutUser(null, mockUser);

            // Assert
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PutUser_ReturnsNotFound_WhenIdNotExist()
        {
            // Arrange
            var mockUser = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Carlos",
                LastName = "Souza",
                Email = "novoemail1@email.com",
                Password = "novasenha100"
            };

            // Act
            var result = _userController.PutUser(mockUser.Id, mockUser);

            // Assert
            Assert.IsAssignableFrom<NotFoundObjectResult>(result);
        }

        [Fact]
        public void PutUser_ReturnsOkResult_WhenIdExist()
        {
            // Arrange
            var mockUser = new User()
            {
                Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                FirstName = "Carlos",
                LastName = "Souza",
                Email = "novoemail1@email.com",
                Password = "novasenha100"
            };

            _mockUsersList.Object.Add(mockUser);

            // Act
            var result = _userController.PutUser(mockUser.Id, mockUser);

            // Assert
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public void PutUser_ReturnsNewUserAfterUpdate_WhenIdExist()
        {
            // Arrange
            var mockUser = new User()
            {
                Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                FirstName = "Carlos",
                LastName = "Souza",
                Email = "novoemail1@email.com",
                Password = "novasenha100"
            };

            _mockUsersList.Object.Add(mockUser);

            var mockUserToUpdate = new User()
            {
                FirstName = "Mateus",
                LastName = "Machado",
                Email = "meuemail2@email.com",
                Password = "123minhasenha"
            };

            // Act
            var result = _userController.PutUser(mockUser.Id, mockUserToUpdate);

            // Assert
            var model = Assert.IsAssignableFrom<OkObjectResult>(result);
            Assert.Equal(mockUserToUpdate.Email, _userController.GetUserById(mockUser.Id).Value.Email);
        }
    }
}
