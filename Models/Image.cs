using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace familytree_api.Models
{
    [Table("image")]
    public class Image
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Column("path")]
        public string Path { get; set; } = string.Empty;

        [ForeignKey("FamilyMember")]
        [Column("family_member_id")]
        public int FamilyMemberId { get; set; }

        public FamilyMember? FamilyMember { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
