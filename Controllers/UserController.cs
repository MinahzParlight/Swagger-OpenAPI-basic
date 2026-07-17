using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;
using Asp.Versioning;

namespace UserService.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lấy danh sách toàn bộ người dùng.
        /// </summary>
        /// <returns>Danh sách mảng UserModel</returns>
        /// <response code="200">Trả về danh sách thành công</response>
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một người dùng theo ID.
        /// </summary>
        /// <param name="id">Mã định danh của người dùng (1, 2, 3...)</param>
        /// <returns>Một object UserModel chi tiết</returns>
        /// <response code="200">Tìm thấy và trả về dữ liệu</response>
        /// <response code="404">Không tìm thấy người dùng với ID này</response>
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Tạo mới một người dùng vào hệ thống.
        /// </summary>
        /// <param name="user">Object chứa thông tin người dùng cần tạo</param>
        /// <returns>Trạng thái tạo thành công</returns>
        /// <response code="201">Tạo người dùng thành công</response>
        /// <response code="400">Dữ liệu gửi lên không hợp lệ</response>
        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        /// <summary>
        /// Cập nhật thông tin người dùng.
        /// </summary>
        /// <param name="id">Mã định danh của người dùng cần cập nhật</param>
        /// <param name="user">Dữ liệu người dùng mới</param>
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}