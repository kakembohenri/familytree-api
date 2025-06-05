using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace familytree_api.Models
{
    [Table("family")]
    public class Family
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("origin")]
        public string Origin { get; set; } = string.Empty ;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
