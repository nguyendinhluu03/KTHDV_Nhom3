using Baithuchanh2.Data;
using Baithuchanh2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Baithuchanh2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderItemsController : ControllerBase
    {
        private readonly DataContext _context;

        public OrderItemsController(DataContext context)
        {
            _context = context;
        }

        // Lấy danh sách tất cả các mặt hàng
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItems>>> GetOrderItems()
        {
            return await _context.OrderItems.ToListAsync();
        }

        // Lấy thông tin chi tiết một mặt hàng
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItems>> GetOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);

            if (orderItem == null)
                return NotFound();

            return orderItem;
        }

        // Tạo một mặt hàng mới
        [HttpPost]
        public async Task<ActionResult<OrderItems>> CreateOrderItem(OrderItems orderItem)
        {
            // Tính toán total_price dựa trên quantity và unit_price
            orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;

            // Kiểm tra xem order_id có tồn tại không
            //var orderExists = await _context.Orders.AnyAsync(o => o.Id == orderItem.OrderId);
            //if (!orderExists)
            //    return BadRequest("Order ID does not exist.");

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.Id }, orderItem);
        }

        // Cập nhật một mặt hàng
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, OrderItems orderItem)
        {
            if (id != orderItem.Id)
                return BadRequest();

            // Tính toán lại total_price
            orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;

            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // Xóa một mặt hàng
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
                return NotFound();

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id);
        }
    }
}
