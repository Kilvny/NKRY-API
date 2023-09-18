﻿using AutoMapper;
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
        [HttpPost("Register")]
        public async Task<ActionResult<User>> PostUser(CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = _mapper.Map<User>(userDto);
            user.Role = UserRole.user; // default is user
            user.CreatedAt = DateTime.UtcNow;


            var result = await _user.CreateUserAsync(user);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            await _unitOfWork.Complete();

            var userToReturn = _mapper.Map<UserDto>(user);
            string uri = GenerateUri(userToReturn.Id);

            //return CreatedAtAction(nameof(UsersController.GetUser), new { id = userToReturn.Id }, userToReturn);
            return Created(uri, userToReturn);


        }

        [NonAction]
        private string GenerateUri(string id)
        {
            string uri = $"http://localhost:5258/api/users/{id}";
            return uri;
        }
    }
}
