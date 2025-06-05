namespace familytree_api.Dtos.User
{
    public class UserFilterDto
    {
        public int Limit { get; set; } = 10;
        public int Page { get; set; } = 1;
        public int FamilyId { get; set; }
    }
}
