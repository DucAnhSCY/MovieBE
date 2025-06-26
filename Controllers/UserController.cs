using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieBE.Models2;
using System.Security.Cryptography;
using System.Text;

namespace MovieBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DBContextTest _context;

        public UserController(DBContextTest context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            try
            {
                var users = await _context.WebUsers
                    .Include(u => u.Role)
                    .Select(u => new
                    {
                        u.WebUserId,
                        u.FirstName,
                        u.LastName,
                        u.EmailId,
                        u.Age,
                        u.PhoneNumber,
                        u.RoleId,
                        u.IsActive,
                        u.CreatedDate,
                        u.LastLogin,
                        Role = u.Role != null ? new
                        {
                            u.Role.RoleId,
                            u.Role.RoleName
                        } : null
                    })
                    .ToListAsync();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving users.", error = ex.Message });
            }
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUser(string id)
        {
            try
            {
                var user = await _context.WebUsers
                    .Include(u => u.Role)
                    .Where(u => u.WebUserId == id)
                    .Select(u => new
                    {
                        u.WebUserId,
                        u.FirstName,
                        u.LastName,
                        u.EmailId,
                        u.Age,
                        u.PhoneNumber,
                        u.RoleId,
                        u.IsActive,
                        u.CreatedDate,
                        u.LastLogin,
                        Role = u.Role != null ? new
                        {
                            u.Role.RoleId,
                            u.Role.RoleName
                        } : null
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(new { message = $"User with ID {id} not found." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the user.", error = ex.Message });
            }
        }

        // POST: api/User/login
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    return BadRequest(new { message = "Email and password are required." });
                }

                var user = await _context.WebUsers
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.EmailId == loginRequest.Email);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid email or password." });
                }

                // Verify password (assuming password is hashed)
                if (!VerifyPassword(loginRequest.Password, user.PasswordHash))
                {
                    return Unauthorized(new { message = "Invalid email or password." });
                }

                if (user.IsActive == false)
                {
                    return Unauthorized(new { message = "User account is inactive." });
                }

                // Update last login
                user.LastLogin = DateTime.Now;
                await _context.SaveChangesAsync();

                // Return user info (excluding password)
                var userResponse = new
                {
                    user.WebUserId,
                    user.FirstName,
                    user.LastName,
                    user.EmailId,
                    user.PhoneNumber,
                    user.RoleId,
                    Role = user.Role != null ? new
                    {
                        user.Role.RoleId,
                        user.Role.RoleName
                    } : null,
                    message = "Login successful"
                };

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login.", error = ex.Message });
            }
        }

        // POST: api/User/register
        [HttpPost("register")]
        public async Task<ActionResult<object>> Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrEmpty(registerRequest.Email) || 
                    string.IsNullOrEmpty(registerRequest.Password) ||
                    string.IsNullOrEmpty(registerRequest.PhoneNumber))
                {
                    return BadRequest(new { message = "Email, password, and phone number are required." });
                }

                // Check if user already exists
                var existingUser = await _context.WebUsers
                    .FirstOrDefaultAsync(u => u.EmailId == registerRequest.Email);

                if (existingUser != null)
                {
                    return Conflict(new { message = "User with this email already exists." });
                }

                // Generate user ID
                var userId = GenerateUserId();

                // Create new user
                var newUser = new WebUser
                {
                    WebUserId = userId,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    EmailId = registerRequest.Email,
                    PasswordHash = HashPassword(registerRequest.Password),
                    Age = registerRequest.Age,
                    PhoneNumber = registerRequest.PhoneNumber,
                    RoleId = "USER1", // Default role
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                _context.WebUsers.Add(newUser);
                await _context.SaveChangesAsync();

                // Return success response (excluding password)
                var userResponse = new
                {
                    newUser.WebUserId,
                    newUser.FirstName,
                    newUser.LastName,
                    newUser.EmailId,
                    newUser.PhoneNumber,
                    message = "Registration successful"
                };

                return CreatedAtAction("GetUser", new { id = newUser.WebUserId }, userResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during registration.", error = ex.Message });
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, [FromBody] UpdateUserRequest updateRequest)
        {
            try
            {
                var user = await _context.WebUsers.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = $"User with ID {id} not found." });
                }

                // Update user properties
                if (!string.IsNullOrEmpty(updateRequest.FirstName))
                    user.FirstName = updateRequest.FirstName;
                
                if (!string.IsNullOrEmpty(updateRequest.LastName))
                    user.LastName = updateRequest.LastName;
                
                if (updateRequest.Age.HasValue)
                    user.Age = updateRequest.Age;
                
                if (!string.IsNullOrEmpty(updateRequest.PhoneNumber))
                    user.PhoneNumber = updateRequest.PhoneNumber;
                
                if (!string.IsNullOrEmpty(updateRequest.RoleId))
                    user.RoleId = updateRequest.RoleId;
                
                if (updateRequest.IsActive.HasValue)
                    user.IsActive = updateRequest.IsActive;

                await _context.SaveChangesAsync();
                return Ok(new { message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the user.", error = ex.Message });
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _context.WebUsers.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = $"User with ID {id} not found." });
                }

                _context.WebUsers.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the user.", error = ex.Message });
            }
        }

        private bool UserExists(string id)
        {
            return _context.WebUsers.Any(e => e.WebUserId == id);
        }

        private string GenerateUserId()
        {
            // Generate a 5-character user ID
            var random = new Random();
            var userId = $"U{random.Next(1000, 9999)}";
            
            // Ensure uniqueness
            while (_context.WebUsers.Any(u => u.WebUserId == userId))
            {
                userId = $"U{random.Next(1000, 9999)}";
            }
            
            return userId;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }
    }

    // DTOs for requests
    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? Age { get; set; }
        public string PhoneNumber { get; set; } = null!;
    }

    public class UpdateUserRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}
