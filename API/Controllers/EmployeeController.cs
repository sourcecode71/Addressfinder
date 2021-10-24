using API.DTOs;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistance.Context;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Core.Employees;
using System.Security.Claims;

namespace API.Controllers
{

    public class EmployeeController : BaseApiController
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly DataContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeController(DataContext context, UserManager<Employee> userManager, SignInManager<Employee> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetEmployees()
        {
            return await _context.Employees.Include(p => p.Projects).Include(a => a.ProjectActivities).ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(string id)
        {
            return await _context.Employees.Include(p => p.Projects).Include(a => a.ProjectActivities).FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteEmployee(string email)
        {
            Employee employee = await _context.Employees.FirstOrDefaultAsync(x => x.Email == email);

            _context.Remove(employee);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return NotFound("Failed to delete employee!");

            return Ok("Employee deleted successfuly!");
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<EmployeeDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                return CreateEmployeeObject(user);
            }

            return Ok();
        }

        [HttpPost("register")]
        public async Task<ActionResult<EmployeeDto>> Register(RegisterDto registerDto)
        {
            
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                return BadRequest("Email taken");
            }

            var user = new Employee
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                IsAdmin = registerDto.IsAdmin
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
               await CreateRole(user, user.IsAdmin);
               return CreateEmployeeObject(user);
            }

            return BadRequest("Problem registring user");
        }

        [HttpPost("activity")]
        public async Task<ActionResult> AddActivity([FromForm] string employeeEmail, [FromForm] string projectId, 
        [FromForm] double duration, [FromForm] string comment)
        {
            return Ok(await Mediator.Send(new AddActivity.Command { EmployeeEmail = employeeEmail , 
            ProjectId = projectId, Duration = duration, Comment = comment})); 
        }

        [HttpGet("current")]
        public async Task<ActionResult<EmployeeDto>> GetCurrentUser()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            Employee user = await _context.Employees.FirstOrDefaultAsync(x => x.Id == userId);
            return CreateEmployeeObject(user); 
        }

        private EmployeeDto CreateEmployeeObject(Employee user)
        {
            return new EmployeeDto
            {
                Name = user.Name,
                Email = user.Email
            };
        }

        private async Task CreateRole(Employee employee, bool isAdmin)
        {
            if(isAdmin)
            {  
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);
                
                var result1 = await _userManager.AddToRoleAsync(employee, "Admin");
                
                if(!result1.Succeeded) throw new System.Exception("Greska!");
            }
            else
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                await _roleManager.CreateAsync(role);

                var result1 = await _userManager.AddToRoleAsync(employee, "Employee");
                
                if(!result1.Succeeded) throw new System.Exception("Greska!");
            }
        }
    }
}