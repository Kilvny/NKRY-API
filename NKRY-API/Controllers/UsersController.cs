
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using NKRY_API.Models;
using static NKRY_API.Utilities.Constants;
using static NKRY_API.Utilities.Cryptography;



namespace NKRY_API.Controllers
{
    [Route("api/[controller]")]
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

        // GET: api/Users
        [HttpHead]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(
            [FromQuery(Name ="department")] string? userDepartment,[FromQuery(Name ="search")] string searchQuery)
        {
          if (_user == null)
          {
              return NotFound();
          }
            var allUsers = _user.GetAll(userDepartment, searchQuery);

            OkObjectResult mappedResponse = Ok(_mapper.Map<IEnumerable<UserDto>>(allUsers
                ));
            return mappedResponse;
        }


        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
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
        public async Task<IActionResult> PutUser(int id, User user)
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(CreateUserDto userDto)
        {
          var user = _mapper.Map<User>(userDto);
            // this if condition is not necessary anymore as DTO doesn't have an Id field
          if (user.Id != 0)
          {
              return Problem("Do not include an 'Id' in the request; it will be auto-generated.", statusCode:422, title: "Unprocessable entity");
          }
            user.CreatedAt = DateTime.UtcNow;
            user.Role = UserRole.user; // default is user
            _user.Create(user);
            await _unitOfWork.Complete();

            var userToReturn = _mapper.Map<UserDto>(user);

            return CreatedAtAction("GetUser", new { id = userToReturn.Id }, userToReturn);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
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

        private bool UserExists(int id)
        {
            return (_user.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
