using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SohatNotebook.Authentication.Configuration;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.Entities.Dtos.Incomming.Profile;
using SohatNotebook.DataService;
using SohatNotebook.Configurations.Generic;
using SohatNotebook.Entities.Dtos.Generic;
using SohatNotebook.Entities.DbSet;
using AutoMapper;

namespace sohatNotebook.Api.Controllers.v1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : BaseController
    {
        
        public ProfileController(
            IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            IMapper mapper): base(unitOfWork, userManager, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var result = new Result<User>();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null) 
            {
                var error = PopulateError(400, ErrorMessages.User.UserNotFound, ErrorMessages.Generic.BadRequest);
                result.Error = error;
                result.IsSuccess = false;
                return BadRequest(result);
            }
            var identifier = new Guid(user.Id);
            var profile = await _unitOfWork.Users.GetUserByIdentityId(identifier);
            if (profile == null) 
            {
                var error = PopulateError(400, ErrorMessages.User.UserNotFound, ErrorMessages.Generic.BadRequest);
                result.Error = error;
                result.IsSuccess = false;
                return BadRequest(result);
            }

            result.Content = profile;
            result.IsSuccess = true;
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(ProfileDTO profileDTO)
        {
            Result<User> result = new Result<User>();
            if (!ModelState.IsValid) 
            {
                var error = PopulateError(400, ErrorMessages.Generic.InvalidPayload, ErrorMessages.Generic.BadRequest);
                result.Error = error;
                result.IsSuccess = false;
                return BadRequest(result);
            }
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                var error = PopulateError(400, ErrorMessages.Generic.SomethingWentWrong, ErrorMessages.Generic.BadRequest);
                result.Error = error;
                result.IsSuccess = false;
                return BadRequest(result);
            }
            var identifier = new Guid(user.Id);
            var profile = await _unitOfWork.Users.GetUserByIdentityId(identifier);
            if (profile == null)
            {
                var error = PopulateError(400, ErrorMessages.User.UserNotFound, ErrorMessages.Generic.BadRequest);
                result.Error = error;
                result.IsSuccess = false;
                return BadRequest(result);
            }
            _mapper.Map<ProfileDTO, User>(profileDTO, profile);

            var isUpdated = await _unitOfWork.Users.UpdateUserProfile(profile);
            if (isUpdated == false) 
            {
                var error = PopulateError(400, ErrorMessages.Generic.SomethingWentWrong, ErrorMessages.Generic.BadRequest);

                return BadRequest(error);
            }

            await _unitOfWork.CompleteAsync();

            result.IsSuccess = true;
            result.Content = profile;
            
            return Ok(result);
        }
    }
}
