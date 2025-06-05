using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace familytree_api.Models
{
    [Table("partner")]
    public class Partner
    {
        [Key]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Husband")]
        [Column("husband_id")]
        public int HusbandId { get; set; }
        
        public FamilyMember? Husband { get; set; }

        [ForeignKey("Wife")]
        [Column("wife_id")]
        public int WifeId { get; set; }

        public FamilyMember? Wife { get; set; }

        [Column("married")]
        public string Married { get; set; } = string.Empty;

        [Column("divorced")]
        public string? Divorced { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

    }
}
