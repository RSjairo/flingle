
using API.Data;
using API.DTO;
using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Entities;

using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {


            using var hmac = new HMACSHA512();


            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                Gender = registerDto.Gender,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };


            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }




        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);


            if (user == null) return Unauthorized("Invalid username");


            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
            }

            // Verify the password
            // if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            // {
            //     return Unauthorized("Invalid password");
            // }
            // if (user.PasswordHash != loginDto.Password)
            // {
            //     return Unauthorized("Invalid password");
            // }



            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }




    }




}
