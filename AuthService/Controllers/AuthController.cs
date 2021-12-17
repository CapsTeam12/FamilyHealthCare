﻿using Data;
using Data.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;

        public AuthController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, ApplicationDbContext db, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _db = db;
        }

        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> Register([FromBody] RegisterModel model)
        //{
        //    var userExists = await _userManager.FindByNameAsync(model.Username);
        //    if (userExists != null)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
        //    User user = new User();
        //    user.Email = model.Email;
        //    user.SecurityStamp = Guid.NewGuid().ToString();
        //    user.UserName = model.Username;
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (result.Succeeded)
        //    {
        //        await _userManager.AddToRoleAsync(user, UserRoles.User);
        //        await _userManager.AddClaimsAsync(user, new Claim[]
        //        {
        //            new Claim(JwtClaimTypes.Email, model.Email),
        //            new Claim(JwtClaimTypes.Name, model.Username),
        //            new Claim(JwtClaimTypes.FamilyName, model.FirstName),
        //            new Claim(JwtClaimTypes.GivenName, model.LastName),
        //        });

        //        return Ok(new Response { Status = "Success", Message = "Registration successful" });

        //    }
        //    else
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Failed", Message = result.Errors.ToString() });
        //    }

        //}

        //[HttpPost]
        //[Route("register-admin")]
        //public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        //{
        //    var userExists = await _userManager.FindByNameAsync(model.Username);
        //    if (userExists != null)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
        //    User user = new User();
        //    user.Email = model.Email;
        //    user.SecurityStamp = Guid.NewGuid().ToString();
        //    user.UserName = model.Username;

        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (!result.Succeeded)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "registration failed!" });
        //    }
        //    if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole<int>(UserRoles.Admin));
        //    }

        //    if (!await _roleManager.RoleExistsAsync(UserRoles.User))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole<int>(UserRoles.User));
        //    }

        //    if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        //    {
        //        await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        //    }
        //    return Ok(new Response { Status = "Success", Message = "registration successful" });

        //}

        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login([FromBody] LoginModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.Username);
        //    if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        //    {
        //        var userRoles = await _userManager.GetRolesAsync(user);

        //        var authClaims = await GetAllValidClaims(user);
            
        //        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

        //        var token = new JwtSecurityToken(
        //            issuer: _configuration["JWT:ValidIssuer"],
        //            audience: _configuration["JWT:ValidAudience"],
        //            expires: DateTime.Now.AddHours(3),
        //            claims: authClaims,
        //            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //            );
        //        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
        //    }

        //    return Unauthorized();
        //}

        private async Task<List<Claim>> GetAllValidClaims(User user)
        {
            var _options = new IdentityOptions();

            var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

            var userClaims = await _userManager.GetClaimsAsync(user); // Get claims of user 
            authClaims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user); // Get the user role 
            foreach (var userRole in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(userRole);
                if(role != null)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach(var roleClaim in roleClaims)
                    {
                        authClaims.Add(roleClaim);
                    }
                }
            }
            return authClaims;
        }

        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet]
        [Route("Users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPost]
        [Route("Roles/Create")]
        public async Task<IActionResult> CreateRole(string name)
        {
            //Check exist
            var roleExists = await _roleManager.RoleExistsAsync(name);
            if (!roleExists)
            {
                var newRole = await _roleManager.CreateAsync(new IdentityRole<int>(name));
                if (newRole.Succeeded) // Check if create successful
                {
                    return Ok(new Response { Status = "Success", Message = $"The role {name} has been created" });
                }
                else
                {
                    return BadRequest(new Response { Status = "Failed", Message = $"The role {name} has not been created" });
                }
            }
            return BadRequest(new Response { Status = "Failed", Message = "Role already exist" });
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string userName,string roleName)
        {
            //Check if the user exist
            var user = await _userManager.FindByNameAsync(userName);

            if(user == null)
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The user with {userName} does not exist" });
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The role with {roleName} does not exist" });
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new Response { Status = "Success", Message = $"The user has been added to the {roleName}" });
            }
            else
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The user has not been added to the {roleName}" });
            }

        }

        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string userName)
        {
            //Check if the user exist
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The user with {userName} does not exist" });
            }

            // return the roles
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);

        }

        [HttpPost]
        [Route("RemoveUserRole")]
        public async Task<IActionResult> RemoveUserRole(string userName,string roleName)
        {
            //Check if the user exist
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The user with {userName} does not exist" });
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The role with {roleName} does not exist" });
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok(new Response { Status = "Success", Message = $"User {userName} has been remove from role {roleName}" });
            }
            else
            {
                return BadRequest(new Response { Status = "Failed", Message = $"Can not remove user {userName} from role {roleName}" });
            }
        }


    }
}
