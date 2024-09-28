using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SohatNotebook.DataService.Data;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.Entities.Dtos.Incomming;
using SohatNotebook.Entities.DbSet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using SohatNotebook.Entities.Dtos.Generic;
using SohatNotebook.Configurations.Generic;
using AutoMapper;

namespace sohatNotebook.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : BaseController
    {
        public UsersController(IUnitOfWork unitOfWork, 
            UserManager<IdentityUser> userManager,
            IMapper mapper): base(unitOfWork, userManager, mapper) { }

        [HttpGet]
        public async Task<IActionResult> GetUsersV1()
        {
            var users = await _unitOfWork.Users.GetAll();

            var result = new ResultPage<User>();

            if (users != null) 
            {
                result.Content = users.ToList();
                result.IsSuccess = true;
                result.Total = users.Count();
                result.ResultPerPage = users.Count();
                result.CurrentPage = 1;
                return Ok(result);

            }

            result.IsSuccess = false;
            result.Error = PopulateError(404, 
                ErrorMessages.User.UserNotFound, 
                ErrorMessages.Generic.ObjectNotFound);
            return BadRequest(result);

        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        [Route("v{x-version:apiVersion}")]
        public async Task<IActionResult> GetUsersV2()
        {
            return Ok("Ok dokye");
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserDTO user)
        {
            var _user = _mapper.Map<User>(user);
            var result = new Result<User>();
            
            result.IsSuccess = true;
            result.Content = _user;

            await _unitOfWork.Users.Add(_user);
            await _unitOfWork.CompleteAsync();

            return CreatedAtRoute("GetUser", new { id = _user.Id }, result);
        }

        [HttpGet]
        [Route("GetUser", Name = "GetUser")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _unitOfWork.Users.GetById(id);
            var result = new Result<User>();
            
            if(user != null)
            {
                result.Content = user;
                result.IsSuccess = true;
                return Ok(result);  
            }

            result.IsSuccess = false;
            result.Error = PopulateError(404,
                ErrorMessages.User.UserNotFound,
                ErrorMessages.Generic.ObjectNotFound);
            return BadRequest(result);
        }
    }
}
