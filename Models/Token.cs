using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace familytree_api.Models
{
    [Table("token")]
    public class Token
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }

        public User? User { get; set; }

        [Column("code")]
        public string Code { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

    }
}
