using familytree_api.Dtos.Family;

namespace familytree_api.Dtos.Partner
{
    public class PartnerInputDto: FamilyMemberInputDto
    {
        public int HusbandId { get; set; }
        public int WifeId { get; set; }
        public string Married { get; set; } = string.Empty;
        public string? Divorced { get; set; }

    }
}
