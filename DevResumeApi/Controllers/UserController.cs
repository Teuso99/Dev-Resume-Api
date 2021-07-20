using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevResumeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevResumeApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(Guid? id)
        {
            if (id == null)
            {
                return new NotFoundObjectResult("ID not provided!");
            }

            var user = _context.Users.SingleOrDefault(m => m.Id == id);

            if (user == null)
            {
                return new NotFoundObjectResult("User not found!");
            }

            return user;
        }

        [HttpPost]
        public async Task<ActionResult> PostUser(User user)
        {
            if (! ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            user.Id = Guid.NewGuid();
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAllUsers",  user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            var userToDelete = await _context.Users.FindAsync(id);

            if (userToDelete == null)
            {
                return new NotFoundObjectResult(userToDelete);
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return new OkObjectResult("User " + userToDelete.Email + " deleted");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(Guid? id, User user)
        {
            if (id == null || user == null)
            {
                return new NotFoundObjectResult("User or UserId is missing!");
            }

            var userToUpdate = await _context.Users.FindAsync(id);

            if (userToUpdate == null)
            {
                return new NotFoundObjectResult("User with UserId is missing!");
            }

            _context.Users.Remove(userToUpdate);
            await _context.SaveChangesAsync();
            
            userToUpdate.Id = (Guid)id;
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;
            userToUpdate.Password = user.Password;

            _context.Users.Add(userToUpdate);
            await _context.SaveChangesAsync();

            return new OkObjectResult(userToUpdate);
        }
    }
}
