using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baithuchanh2.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
