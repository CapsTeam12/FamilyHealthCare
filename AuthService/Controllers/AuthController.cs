using Data;
using Data.Entities;
using IdentityModel;
using Contract.Constants;
using Microsoft.AspNetCore.Authorization;
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
using Business.IServices;
using Contract.DTOs.AuthService;
using Contract.DTOs.ManagementService;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IBaseRepository<Patient> _patientRepos;

        public AuthController(UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IAuthService authService, IBaseRepository<Patient> patientRepos)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
            _patientRepos = patientRepos;
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
        public async Task<IActionResult> GetUser(string id)
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
        [Route("Roles/Create")]
        public async Task<IActionResult> CreateRole(string name)
        {
            //Check exist
            var roleExists = await _roleManager.RoleExistsAsync(name);
            if (!roleExists)
            {
                var newRole = await _roleManager.CreateAsync(new IdentityRole(name));
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
        public async Task<IActionResult> AddUserToRole(string userName, string roleName)
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

            var result = await _userManager.AddToRoleAsync(user, roleName);
            await _userManager.AddClaimsAsync(user, new Claim[]
                    {
                       new Claim(JwtClaimTypes.Role, roleName),
                    });
            if (result.Succeeded)
            {
                return Ok(new Response { Status = "Success", Message = $"The user has been added to the {roleName}" });
            }
            else
            {
                return BadRequest(new Response { Status = "Failed", Message = $"The user has not been added to the {roleName}" });
            }

        }

        [HttpPost]
        [Route("RemoveUserRole")]
        public async Task<IActionResult> RemoveUserRole(string userName, string roleName)
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

        [HttpPut]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePassworDto)
        {
            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (changePassworDto.UserId == null)
            {
                return Unauthorized();
            }
            var changePassworDtos = await _authService.ChangePasswordAsync(changePassworDto);
            
            return changePassworDtos;
        }

        [HttpPut]
        [Route("update-profile")]
        public async Task<IActionResult> UpdatePatientProfile([FromForm] PatientUpdateDto patientUpdateDto)
        {
            if (patientUpdateDto.AccountId.ToString() == null)
            {
                return Unauthorized();
            }
            var patients = await _patientRepos.Entities.ToListAsync();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                if (user.Id != patientUpdateDto.AccountId)
                {
                    if (patientUpdateDto.Email == user.Email)
                        return NoContent();
                }
                
            }
            foreach (var patient in patients)
            {
                if (patient.AccountId != patientUpdateDto.AccountId)
                {
                    if (patientUpdateDto.Phone == patient.Phone)
                        return NotFound();
                }
            }
            var updatePatientProfile = await _authService.UpdatePatientProfileAsync(patientUpdateDto);

            return Ok(updatePatientProfile);
        }

       
    }
}
