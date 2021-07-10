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
        public ActionResult<List<User>> Get()
        {
            return _users.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<User> GetById(Guid? id)
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
    }
}
