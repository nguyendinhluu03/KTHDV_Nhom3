using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Baithuchanh2.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }  // ID tự động tăng

        //	[Required]
        //[MaxLength(255)]
        public string Name { get; set; }  // Tên sản phẩm

        public string Description { get; set; }  // Mô tả sản phẩm

        //[Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }  // Giá sản phẩm

        public int Quantity { get; set; }  // Số lượng sản phẩm trong kho

        public DateTime Created_at { get; set; } = DateTime.Now;  // Ngày sản phẩm được tạo

        public DateTime Updated_at { get; set; } = DateTime.Now;  // Ngày sản phẩm được cập nhật lần cuối
    }
}
