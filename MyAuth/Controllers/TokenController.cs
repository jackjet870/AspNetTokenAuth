using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyAuth.Models;
using MyAuth.Service;

namespace MyAuth.Controllers
{
    public class TokenController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("Token")]
        public string GetAuthToken(LogonModel model)
        {
            if (ModelState.IsValid)
            {
                var registerUserId = GetUserByLogon(model).Id;
                var token = TokenService.CreateToken(registerUserId);
                return token;
            }
            return null;
        }

        public User GetUserByLogon(LogonModel model)
        {
            using (var db = new Db())
            {
                return db.Users.FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.UserPass);
            }
            return null;
        }
    }
}