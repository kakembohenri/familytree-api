namespace familytree_api.Dtos.Family
{
    public class FamilyMemberUpdateDto
    {
        public int Id { get; set; }
        public string Born { get; set; } = string.Empty;
        public string? Died { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
