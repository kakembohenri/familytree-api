using familytree_api.Models;

namespace familytree_api.Dtos.Family
{
    public class TreeOutputDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string MiddleName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public int FamilyId { get; set; }
        public string Born { get; set; } = default!;
        public string? Died { get; set; } = default!;
        public int? FatherId { get; set; }
        public int? MotherId { get; set; }
        public bool ShowInTree{ get; set; }
        public List<FamilyMemberOutput> Partners { get; set; } = new();
        public List<Models.Image> Images { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public List<TreeOutputDto> Children { get; set; } = new();
    }
}
