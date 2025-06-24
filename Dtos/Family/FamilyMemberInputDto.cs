namespace familytree_api.Dtos.Family
{
    public class FamilyMemberInputDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email{ get; set; }
        public int FamilyId { get; set; }
        public string Born { get; set; } = string.Empty;
        public string PlaceOfBirth { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? Died { get; set; }
        public int? FatherId { get; set; }
        public int? MotherId { get; set; }
        public bool ShowInTree { get; set; } = true;
        public string Gender { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
