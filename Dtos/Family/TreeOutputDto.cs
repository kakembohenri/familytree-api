using familytree_api.Models;

namespace familytree_api.Dtos.Family
{
    public class TreeOutputDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string MiddleName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Gender { get; set; } = default!;
        public Image? Avatar { get; set; }
        public int FamilyId { get; set; }
        public string Born { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Died { get; set; } = default!;
        public int? FatherId { get; set; }
        public int? MotherId { get; set; }
        public string Occupation { get; set; } = default!;
        public string PlaceOfBirth { get; set; } = default!;
        public string Bio { get; set; } = default!;
        public bool ShowInTree{ get; set; }
        public bool Expanded{ get; set; }
        public List<FamilyMemberOutput> Partners { get; set; } = new();
        public List<Models.Image> Images { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public List<TreeOutputDto> Children { get; set; } = new();
    }
}
