namespace familytree_api.Dtos.Family
{
    public class FamilyMemberOutput
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string MiddleName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public int FamilyId { get; set; }
        public bool ShowInTree { get; set; }
        public string Born { get; set; } = default!;
        public string Gender { get; set; } = string.Empty;
        public string? Died { get; set; } = default!;
        public int? FatherId { get; set; }
        public int? MotherId { get; set; }
        public List<Models.Image> Images { get; set; } = new();

        // Partner information
        public string? Married { get; set; }
        public string? Divorced { get; set; }
        public int PartnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<TreeOutputDto> Children { get; set; } = new();
    }
}
