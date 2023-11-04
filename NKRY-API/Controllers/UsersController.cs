
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using NKRY_API.Models;
using NKRY_API.ResourceParameters;
using System.Security.Claims;
using static NKRY_API.Utilities.Constants;
using static NKRY_API.Utilities.Cryptography;



namespace NKRY_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _user;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _user = _unitOfWork.User;
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        [HttpGet]
        [Route("api/Tokens")]
        public IActionResult TestEndPointForUserOnly()
        {
            var currentUserRole = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role)?.Value;
            return Ok($"You're Authorized as {currentUserRole}");
        }

        // GET: api/Users
        [HttpHead]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] UsersResourceParameters usersResourceParameters)
        {
          if (_user == null)
          {
              return NotFound();
          }

            var allUsers = _user.GetAll(usersResourceParameters);

            OkObjectResult mappedResponse = Ok(_mapper.Map<IEnumerable<UserDto>>(allUsers
                ));
            return mappedResponse;
        }


        // GET: api/Users/5
        [HttpGet("{id}")]
        [ActionName("GetUser")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
          if (_user == null)
          {
              return NotFound();
          }
            var user = _user.GetById(id);

            if (user == null)
            {
                return NotFound();
            }
            OkObjectResult mappedResponse = Ok(_mapper.Map<UserDto>(user));
            return mappedResponse;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _user.Update(user);

            try
            {
                await _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (_user == null)
            {
                return NotFound();
            }
            var user = _user.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            _user.Delete(user);
            await _unitOfWork.Complete();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return (_user.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
