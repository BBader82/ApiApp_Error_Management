using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiApp_Male.helper;
using ApiApp_Male.Models.RequestDTO;
using ApiApp_Male.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiApp_Male.Controllers
{
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository userRep;
        private readonly IErrorClass error;

        public AuthenticationController(IUserRepository UserRep,IErrorClass error)
        {
            userRep = UserRep;
            this.error = error;
        }

        [HttpPost("Registration")]
        public IActionResult Registration(UserAddDTO UserDTO)
        {

            var AuthRes = userRep.Registration(UserDTO);
            if (AuthRes.Success)
                return Ok(AuthRes);

            error.LoadError(AuthRes.ErrorCode);
            ModelState.AddModelError("UserName", error.ErrorMessage);
            return ValidationProblem();


        }
        [HttpPost("Login")]
        public IActionResult Login(UserLoginDTO UserDTO)
        {

            var AuthRes = userRep.UserLogin(UserDTO);
            if (AuthRes.Success)
                return Ok(AuthRes);

            error.LoadError(AuthRes.ErrorCode);
            ModelState.AddModelError(error.ErrorProp, error.ErrorMessage);
            return ValidationProblem();


        }
    }
}
