using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SohatNotebook.DataService.IConfiguration;
using SohatNotebook.Entities.Dtos.Errors;

namespace sohatNotebook.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BaseController : ControllerBase
    {
        public IUnitOfWork _unitOfWork;
        public UserManager<IdentityUser> _userManager;
        public IMapper _mapper;
        public BaseController(
            IUnitOfWork unitOfWork, 
            UserManager<IdentityUser> userManager,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        internal Error PopulateError(int code, string message, string type)
        {
            return new Error
            {
                Code = code,
                Message = message,
                Type = type
            };
        }
    }
}
