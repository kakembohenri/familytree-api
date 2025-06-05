using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace familytree_api.Models
{
    [Table("family_member")]
    public class FamilyMember
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("User")]
        [Column("user_id")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [ForeignKey("Family")]
        [Column("family_id")]
        public int FamilyId { get; set; }
         public Family? Family { get; set; }

        [Column("born")]
        public string Born { get; set; } = string.Empty;

        [Column("died")]
        public string? Died { get; set; }

        [ForeignKey("Father")]
        [Column("father_id")]
        public int? FatherId { get; set; }
        public FamilyMember? Father { get; set; }

        [ForeignKey("Mother")]
        [Column("mother_id")]
        public int? MotherId { get; set; }
        public FamilyMember? Mother { get; set; }

        [Column("show_in_tree")]
        public bool ShowInTree { get; set; }

        [Column("gender")]
        public string? Gender { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

    }
}
