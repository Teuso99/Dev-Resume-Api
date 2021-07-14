using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevResumeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly List<User> _users;

        public UserController(List<User> users)
        {
            _users = users;
        }

        [HttpGet]
        public ActionResult<List<User>> GetAllUsers()
        {
            return _users.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(Guid? id)
        {
            if (id == null)
            {
                return new NotFoundObjectResult("ID not provided!");
            }

            var user = _users.SingleOrDefault(m => m.Id == id);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found!");
            }

            return user;
        }

        [HttpPost]
        public ActionResult PostUser(User user)
        {
            if (! ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            user.Id = Guid.NewGuid();
            _users.Add(user);

            return CreatedAtAction("Get", user);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(Guid id)
        {
            var userToDelete = _users.SingleOrDefault(m => m.Id == id);

            if (userToDelete == null)
            {
                return new NotFoundObjectResult(userToDelete);
            }

            _users.Remove(userToDelete);

            return new OkObjectResult("User " + userToDelete.Email + " deleted");
        }

        [HttpPut("{id}")]
        public ActionResult PutUser(Guid? id, User user)
        {
            if (id == null || user == null)
            {
                return new NotFoundObjectResult("User or UserId is missing!");
            }

            var userToUpdate = _users.FirstOrDefault(m => m.Id == id);
            _users.Remove(userToUpdate);

            if (userToUpdate == null)
            {
                return new NotFoundObjectResult("User with UserId is missing!");
            }

            userToUpdate.Id = (Guid)id;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;
            userToUpdate.Password = user.Password;

            _users.Add(userToUpdate);

            return new OkObjectResult(userToUpdate);
        }
    }
}
