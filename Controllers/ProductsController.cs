using Baithuchanh2.Data;
using Baithuchanh2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Baithuchanh2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Products>>> GetProducts()
        {
            string query = "SELECT Id, name, description, price, quantity, created_at, updated_at FROM Products"; // Chọn đầy đủ các cột
            var products = await _context.Products.FromSqlRaw(query).ToListAsync();
            return Ok(products);
        }
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Products>> GetProduct(int id)
        {
            string query = "SELECT * FROM Products WHERE Id = @Id";
            var idParameter = new SqlParameter("@Id", id);  // Parameter bảo vệ SQL injection
            var product = await _context.Products.FromSqlRaw(query, idParameter).FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Products product)
        {
            // Kiểm tra dữ liệu đầu vào
            if (id != product.Id)
            {
                return BadRequest(new { message = "ID in URL does not match ID in product object." });
            }

            if (product == null || string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0 || product.Quantity < 0)
            {
                return BadRequest(new { message = "Invalid product data. Ensure all required fields are valid." });
            }

            // Truy vấn SQL để cập nhật sản phẩm
            string query = @"
        UPDATE Products 
        SET 
            Name = @Name, 
            Description = @Description, 
            Price = @Price, 
            Quantity = @Quantity, 
            updated_at = @Updated_at
        WHERE Id = @Id";

            var parameters = new[]
            {
        new SqlParameter("@Name", product.Name),
        new SqlParameter("@Description", product.Description ?? (object)DBNull.Value), // Cho phép null
        new SqlParameter("@Price", product.Price),
        new SqlParameter("@Quantity", product.Quantity),
        new SqlParameter("@Updated_at", DateTime.UtcNow), // UTC để nhất quán múi giờ
        new SqlParameter("@Id", id)
    };

            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(query, parameters);

                if (result == 0) // Nếu không có bản ghi nào bị cập nhật
                {
                    return NotFound(new { message = "Product not found." });
                }
            }
            catch (Exception ex)
            {
                // Log lỗi (ví dụ: dùng Serilog hoặc ILogger)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred.", details = ex.Message });
            }

            return Ok(new { message = "Cập nhật sản phẩm thành công!" });
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Products>> PostProduct(Products product)
        {
            string query = "INSERT INTO Products (name, description, price, quantity, created_at, updated_at) " +
                           "VALUES (@Name, @Description, @Price, @Quantity, @Created_at, @Updated_at); " +
                           "SELECT CAST(SCOPE_IDENTITY() AS INT)"; // SCOPE_IDENTITY() để lấy ID mới tạo

            var parameters = new[]
            {
                new SqlParameter("@Name", product.Name),
                new SqlParameter("@Description", product.Description),
                new SqlParameter("@Price", product.Price),
                new SqlParameter("@Quantity", product.Quantity),
                new SqlParameter("@Created_at", DateTime.Now),
                new SqlParameter("@Updated_at", DateTime.Now)
            };

            var newId = await _context.Database.ExecuteSqlRawAsync(query, parameters);
            product.Id = newId;  // Gán ID mới trả về cho sản phẩm

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            string query = "DELETE FROM Products WHERE Id = @Id";
            var idParameter = new SqlParameter("@Id", id);

            var result = await _context.Database.ExecuteSqlRawAsync(query, idParameter);

            if (result == 0) // Nếu không có bản ghi nào bị xóa
            {
                return NotFound(new { message = "Product not found." });
            }

            // Trả về thông báo thành công
            return Ok(new { message = "Product deleted successfully." });
        }

        private bool ProductExists(int id)
        {
            string query = "SELECT COUNT(1) FROM Products WHERE Id = @Id";
            var idParameter = new SqlParameter("@Id", id);
            var count = _context.Products.FromSqlRaw(query, idParameter).CountAsync().Result;
            return count > 0;
        }

    }
}
