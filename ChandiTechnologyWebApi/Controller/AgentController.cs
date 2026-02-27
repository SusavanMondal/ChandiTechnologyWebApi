using ChandiTechnologyWebApi.Classes;
using ChandiTechnologyWebApi.Data;
using ChandiTechnologyWebApi.DTO;
using ChandiTechnologyWebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChandiTechnologyWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {

        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;

        private readonly PasswordHasher hasher;
        private readonly TokenGenerator tokenGenerator;


        public AgentController(AppDBContext context, IConfiguration configuration, PasswordHasher hasher, TokenGenerator tokenGenerator)
        {
            _context = context;
            _configuration = configuration;
            this.hasher = hasher;
            this.tokenGenerator = tokenGenerator;
        }



        [HttpPost("login")]


        public async Task<IActionResult> Login([FromBody] LoginDTO request)
        {

            var agent = await _context.Agents.SingleOrDefaultAsync(a => a.Email == request.Email);



            if (agent == null || !hasher.VerifyPassword(request.Password, agent.PasswordHash))
            {

                _context.AuditLogs.Add(new AuditLog { EventType = "LoginFailed", Details = $"Failed login for {request.Email}" });
                await _context.SaveChangesAsync();

                return Unauthorized(new { message = "Invalid email or password" });
            }


            string jwtToken = tokenGenerator.GenerateJwtToken(agent);

            _context.AuditLogs.Add(new AuditLog { EventType = "LoginSuccess", Details = $"Agent {agent.Email} logged in.", AgentID = agent.AgentID });
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = jwtToken,
                Agent = new
                {
                    agent.AgentID,
                    agent.CompanyName,
                    agent.ContactPerson,
                    agent.Email,
                    agent.Phone
                }
            });
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AgentRegistrationRequestDto request)
        {
            bool emailExists = await _context.Agents.AnyAsync(a => a.Email == request.Email);
            if (emailExists)
            {
                return BadRequest(new { message = "An agent with this email already exists." });
            }

            string hashedPassword = hasher.HashPassword(request.Password);

            string generatedApiKey = Guid.NewGuid().ToString("N");


            var newAgent = new Agent
            {
                CompanyName = request.CompanyName,
                ContactPerson = request.ContactPerson,
                Email = request.Email,
                Phone = request.Phone,
                PasswordHash = hashedPassword,
                ApiKey = generatedApiKey,
                RegisteredOn = DateTime.UtcNow
            };

            
            _context.Agents.Add(newAgent);

          
            _context.AuditLogs.Add(new AuditLog
            {
                EventType = "AgentRegistered",
                Details = $"New agent registered: {request.CompanyName} ({request.Email})"
            });

            await _context.SaveChangesAsync();

            
            return Ok(new
            {
                message = "Registration successful.",
                email = newAgent.Email,
                apiKey = newAgent.ApiKey
            });
        }





    }
}
