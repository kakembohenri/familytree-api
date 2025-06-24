using System.ComponentModel.DataAnnotations;

namespace familytree_api.Dtos.Family
{
    public class FileInputDto
    {
        [Required(ErrorMessage = "File is required")]
        public IFormFile? File { get; set; }
        public string Type { get; set; } = "other";
        public int FamilyMemberId { get; set; }
        public int OldAvatar { get; set; } = 0;
    }
}
