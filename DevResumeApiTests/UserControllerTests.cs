using System;
using Xunit;
using DevResumeApi;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using Bogus;

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

        private User NewFakerUser()
        {
            var faker = new Faker<User>(locale: "pt_BR")
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName))
                .RuleFor(u => u.Password, f => f.Internet.Password());

            var newUser = faker.Generate();

            return newUser;
        }

        [Fact]
        public async Task GetUsers_ReturnListOfUsers()
        {
            // Arrange
            var request = "/api/User/GetUsers";

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
        public async Task GetUserById_ReturnsOkResult_WhenUserExist()
        {
            // Arrange
            var postRequest = "/api/User/Post";

            var newUser = NewFakerUser();

            var postRequestBody = new StringContent(JsonConvert.SerializeObject(newUser), UnicodeEncoding.UTF8, "application/json");

            // Act
            var postResponse = await _client.PostAsync(postRequest, postRequestBody);
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();

            var userResponse = JsonConvert.DeserializeObject<User>(postResponseBody);

            var getResponse = await _client.GetAsync("/api/User/" + userResponse.Id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
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

            var newUser = NewFakerUser();

            var requestBody = new StringContent(JsonConvert.SerializeObject(newUser), UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(request, requestBody);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenIdDoesNotExist()
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
        public async Task DeleteUser_ReturnsOkResult_WhenIdExist()
        {
            // Arrange
            var postRequest = "/api/User/Post";

            var newUser = NewFakerUser();

            var postRequestBody = new StringContent(JsonConvert.SerializeObject(newUser), UnicodeEncoding.UTF8, "application/json");

            // Act
            var postResponse = await _client.PostAsync(postRequest, postRequestBody);
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();

            var userResponse = JsonConvert.DeserializeObject<User>(postResponseBody);

            var deleteResponse = await _client.DeleteAsync("/api/User/" + userResponse.Id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task PutUser_ReturnsNotFound_WhenIdAndUserIsNull()
        {
            // Arrange
            var request = "/api/User/";
            HttpContent requestBody = null;

            // Act
            var response = await _client.PutAsync(request, requestBody);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutUser_ReturnsBadRequest_WhenUserIsNull()
        {
            // Arrange
            var request = String.Format("/api/User/{0}", Guid.NewGuid());
            var newUser = new User();
            var requestBody = new StringContent(JsonConvert.SerializeObject(newUser), UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(request, requestBody);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PutUser_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var request = "/api/User/";
            var newUser = NewFakerUser();

            var requestBody = new StringContent(JsonConvert.SerializeObject(newUser), UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(request, requestBody);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutUser_ReturnsNotFound_WhenIdNotExist()
        {
            // Arrange
            var request = String.Format("/api/User/{0}", Guid.NewGuid());
            var newUser = NewFakerUser();

            var requestBody = new StringContent(JsonConvert.SerializeObject(newUser), UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync(request, requestBody);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PutUser_ReturnsOkResult_WhenIdExist()
        {
            // Arrange
            var postRequest = "/api/User/Post";
            var newUser = NewFakerUser();

            var postRequestBody = new StringContent(JsonConvert.SerializeObject(newUser), UnicodeEncoding.UTF8, "application/json");

            // Act
            var postResponse = await _client.PostAsync(postRequest, postRequestBody);
            var postResponseBody = await postResponse.Content.ReadAsStringAsync();

            var userResponse = JsonConvert.DeserializeObject<User>(postResponseBody);

            var updatedUser = NewFakerUser();
            updatedUser.Id = userResponse.Id;
            var putRequestBody = new StringContent(JsonConvert.SerializeObject(newUser), UnicodeEncoding.UTF8, "application/json");

            var putResponse = await _client.PutAsync("/api/User/" + userResponse.Id, putRequestBody);

            // Assert
            Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        }
    }
}
