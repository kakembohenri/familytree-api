namespace familytree_api.Dtos.Family
{
    public class FamilyInputDto
    {
        public string Name { get; set; } = string.Empty;
        public string Origin { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }= DateTime.Now;
    }
}
