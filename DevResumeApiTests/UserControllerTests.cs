using System;
using Xunit;
using Moq;
using DevResumeApi.Controllers;
using System.Collections.Generic;
using DevResumeApi;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DevResumeApi.Models;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace DevResumeApiTests
{
    public class UserControllerTests
    {
        private readonly HttpClient _client;

        public UserControllerTests()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        public async Task GetUsers_ReturnListOfUsers()
        {
            // Arrange
            var request = "/api/User/GetAll";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenIdNotProvided()
        {
            // Arrange
            var request = "/api/User/";

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetUserById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = "/api/User/" + id;

            // Act
            var response = await _client.GetAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void GetUserById_ReturnsSingleUser_WhenUserExist()
        {
            // Arrange

            // Act

            // Assert

        }

        [Fact]
        public async Task PostUser_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var request = "/api/User/Post";
            var newUser = new User();
            var requestBody = new StringContent(JsonConvert.SerializeObject(newUser), UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(request, requestBody);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PostUser_ReturnsCreatedResponse_WhenValidObjectPassed()
        {
            // Arrange
            var request = "/api/User/Post";
            var newUser = new User()
            {
                FirstName = "Jorge",
                LastName = "Silva",
                Email = "jsemail@email.com",
                Password = "outrasenha456"
            };
            var requestBody = new StringContent(JsonConvert.SerializeObject(newUser), UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(request, requestBody);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task RemoveUser_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = "/api/User/" + id;

            // Act
            var response = await _client.DeleteAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public void RemoveUser_ReturnsOkResult_WhenIdExist()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public void RemoveUser_RemovesOneItem_WhenIdExist()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void PutUser_ReturnsNotFound_WhenIdAndUserIsNull()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void PutUser_ReturnsNotFound_WhenUserIsNull()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void PutUser_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void PutUser_ReturnsNotFound_WhenIdNotExist()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void PutUser_ReturnsOkResult_WhenIdExist()
        {
            // Arrange

            // Act

            // Assert

        }

        [Fact]
        public void PutUser_ReturnsNewUserAfterUpdate_WhenIdExist()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
