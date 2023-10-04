using API.Repository;
using API.Repository.Interface;
using API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository repository;

        public EmployeesController(IEmployeeRepository repository)
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

        //private ActionResult CreateResponse(HttpStatusCode statusCode, string message, object data = null)
        //{
        //    var responseObj = new
        //    {
        //        status_code = (int)statusCode,
        //        message,
        //        data
        //    };

        //    return StatusCode((int)statusCode, responseObj);
        //}

        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                var employees = repository.Get();
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", employees);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpGet("{NIK}")]
        public ActionResult Get(string NIK)
        {
            try
            {
                var employee = repository.GetData(NIK);
                if (employee == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                return CreateResponse(HttpStatusCode.OK, "Data has been found!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpPut]
        public ActionResult Update(Employee employee)
        {
            try
            {
                repository.Update(employee);
                return CreateResponse(HttpStatusCode.OK, "Data has been updated!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        //[HttpPost]
        //public ActionResult Insert(Employee employee)
        //{
        //    try
        //    {
        //        repository.Insert(employee);
        //        return CreateResponse(HttpStatusCode.Created, "Data has been inserted!", employee);
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return CreateResponse(HttpStatusCode.Conflict, ex.Message);
        //    }
        //}

        [HttpDelete("{NIK}")]
        public ActionResult Delete(string NIK)
        {
            try
            {
                var employee = repository.Get(NIK);
                if (employee == null)
                {
                    return CreateResponse(HttpStatusCode.NotFound, "Data not found.");
                }
                repository.Delete(NIK);
                return CreateResponse(HttpStatusCode.OK, "Data has been deleted!", employee);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpPost("Register")]
        public ActionResult Insert(RegisterVM register)
        {
            try
            {
                repository.Insert(register);
                return CreateResponse(HttpStatusCode.Created, "Data has been inserted!", register);
            }
            catch (ArgumentException ex)
            {
                return CreateResponse(HttpStatusCode.Conflict, ex.Message);
            }
        }
    }
}
