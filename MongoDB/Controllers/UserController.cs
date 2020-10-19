using Microsoft.AspNetCore.Mvc;
using MongoDB.Services;

namespace MongoDB.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController:ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public ActionResult GetUsers()
        {
           return Ok(_userService.GetUsers());
        }

        [HttpPost]
        public bool ValidateUser([FromForm] string userName,[FromForm]string password)
        {
           var user= _userService.GetUser(userName);
            if(user==null)
            {
                return false;
            }
           else if(user!=null&&user.password == password)
                return true;
            else
            {
                return false;
            }

        }
    }
}
