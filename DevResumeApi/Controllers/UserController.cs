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
    }
}
