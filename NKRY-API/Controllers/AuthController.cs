using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using NKRY_API.Models;
using NKRY_API.ResourceParameters;
using static NKRY_API.Utilities.Constants;

namespace NKRY_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _user;

        public AuthController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _user = _unitOfWork.User;
        }

        // POST: api/auth/register
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        // TODO: update this to be resitrected for admins after seeding
        [AllowAnonymous] 
        [HttpPost("Register")]
        public async Task<ActionResult<User>> PostUser(CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = _mapper.Map<User>(userDto);
            user.Role = "User"; // default is user
            user.CreatedAt = DateTime.UtcNow;


            var result = await _user.CreateUserAsync(user);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            await _unitOfWork.Complete();

            var userToReturn = _mapper.Map<UserDto>(user);
            string uri = GenerateUri(userToReturn.Id);

            var createdResource = userToReturn;
            var actionName = nameof(UsersController.GetUser);
            var controllerName = "Users";
            var routeValues = new { id = createdResource.Id };

            return CreatedAtAction(actionName, controllerName, routeValues, createdResource);
            //return Created(uri, userToReturn);


        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _user.LoginUserAsync(model);
            
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result.Message);


        }

        [NonAction]
        private string GenerateUri(string id)
        {
            string uri = $"http://localhost:5258/api/users/{id}";
            return uri;
        }
    }
}
