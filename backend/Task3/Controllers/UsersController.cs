using Microsoft.AspNetCore.Mvc;
using Task3.DTOs;
using Task3.Models;

namespace Task3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _db;

        public UsersController(MyDbContext db)
        {
            _db = db;
        }

        // First: Get all users
        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _db.Users.ToList();
            return Ok(users);
        }

        // Second: Get user by ID
        [HttpGet]
        [Route("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }
            return Ok(user);
        }

        // Third: Get user by name
        [HttpGet]
        [Route("GetUserByName/{name}")]
        public IActionResult GetUserByName(string name)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == name);
            if (user == null)
            {
                return NotFound(new { message = $"User with name '{name}' not found." });
            }
            return Ok(user);
        }

        // Fourth: Delete user by ID
        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            _db.Users.Remove(user);
            _db.SaveChanges();
            return Ok(new { message = $"User with ID {id} has been deleted." });
        }

        // Fifth: Add a new user
        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser([FromForm] UsersRequestDTO newUser)
        {


            // Map the DTO to a User entity
            var user = new User
            {
                Username = newUser.Username,
                Email = newUser.Email,
                Password = newUser.Password,
                Phone = newUser.Phone,
                Image = newUser.Image?.FileName
            };

            // Handle image file upload
            if (newUser.Image != null)
            {
                var uploadedFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadedFolder))
                {
                    Directory.CreateDirectory(uploadedFolder);
                }

                var filePath = Path.Combine(uploadedFolder, newUser.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    newUser.Image.CopyTo(stream);
                }
            }

            _db.Users.Add(user); // Add the User entity to the database, not the DTO
            _db.SaveChanges();
            return Ok(new { message = "User added successfully.", user });
        }

        // Sixth: Update an existing user
        [HttpPut]
        [Route("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromForm] UsersRequestDTO updatedUser)
        {
            var existingUser = _db.Users.Find(id);
            if (existingUser == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            // Update user properties
            existingUser.Username = updatedUser.Username ?? existingUser.Username;
            existingUser.Email = updatedUser.Email ?? existingUser.Email;
            existingUser.Password = updatedUser.Password ?? existingUser.Password;
            existingUser.Phone = updatedUser.Phone ?? existingUser.Phone;

            // Handle image file update if a new image is provided
            if (updatedUser.Image != null)
            {
                var uploadedFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadedFolder))
                {
                    Directory.CreateDirectory(uploadedFolder);
                }

                var filePath = Path.Combine(uploadedFolder, updatedUser.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    updatedUser.Image.CopyTo(stream);
                }

                // Update the Image property with the new file name
                existingUser.Image = updatedUser.Image.FileName;
            }

            _db.Users.Update(existingUser);
            _db.SaveChanges();
            return Ok(new { message = "User updated successfully.", user = existingUser });
        }
    }

}
