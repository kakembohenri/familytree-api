namespace familytree_api.Dtos.Partner
{
    public class PartnerUpdateDto
    {
        public int Id { get; set; }
        public string Divorced { get; set; } = string.Empty;
        public string Married { get; set; } = string.Empty;
    }
}
