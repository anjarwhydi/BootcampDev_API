using API.Repository.Interface;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository repository;

        public AccountsController(IAccountRepository repository)
        {
            this.repository = repository;
        }

        private ActionResult CreateResponse(HttpStatusCode statusCode, string message, object data = null)
        {
            if (data == null)
            {
                var responseDataNull = new JsonResult(new
                {
                    status_code = (int)statusCode,
                    message,
                });

                return responseDataNull;

            }

            var response = new JsonResult(new
            {
                status_code = (int)statusCode,
                message,
                data
            });

            return response;
        }

        //[HttpPost("login")]
        //public ActionResult login(string email, string password)
        //{
        //    try
        //    {
        //        if (email == null && password == null)
        //        {
        //            return CreateResponse(HttpStatusCode.BadRequest, "Email and password cannot be empty!");
        //        }
        //        var result= repository.login(email, password);
        //        if (result == false)
        //        {
        //            return CreateResponse(HttpStatusCode.Unauthorized, "Email or Password is wrong.");
        //        }
        //        return CreateResponse(HttpStatusCode.OK, "login successfully", email);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return CreateResponse(HttpStatusCode.BadRequest, ex.Message);
        //    }
        //}

        [HttpPost("SendEmail")]
        public ActionResult SendEmail(string email)
        {
            try
            {
                if (email == null)
                {
                    return CreateResponse(HttpStatusCode.BadRequest, "Email cannot be empty!");
                }
                var result = repository.SendEmail(email);
                if (result == false)
                {
                    return CreateResponse(HttpStatusCode.BadRequest, "Email not found!");
                }
                return CreateResponse(HttpStatusCode.OK, "OTP has been sent!", email);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        //[HttpPost("changepassword")]
        //public ActionResult ChangePasswordUsingOTP(string email, string otp, string newPassword, string checkPassword)
        //{
        //    try
        //    {
        //        repository.ChangePasswordUsingOTP(email, otp, newPassword, checkPassword);
        //        return CreateResponse(HttpStatusCode.OK, "Password has been changed!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return CreateResponse(HttpStatusCode.BadRequest, ex.Message);
        //    }
        //}

        [HttpPost("login")]
        public ActionResult Login(LoginVM login)
        {
            try
            {
                if (login.email == null && login.password == null)
                {
                    return CreateResponse(HttpStatusCode.BadRequest, "Email and password cannot be empty!");
                }
                int result = repository.Login(login);
                if (result == -1)
                {
                    return CreateResponse(HttpStatusCode.Unauthorized, "Email is wrong.");
                }
                if (result == 0)
                {
                    return CreateResponse(HttpStatusCode.Unauthorized, "Password is wrong.");
                }
                return CreateResponse(HttpStatusCode.OK, "Login successfully", login.email);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost("changepassword")]
        public ActionResult ChangePassword(ChangePasswordVM change)
        {
            try
            {
                repository.ChangePasswordVM(change);
                return CreateResponse(HttpStatusCode.OK, "Password has been changed!");
            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }

}
