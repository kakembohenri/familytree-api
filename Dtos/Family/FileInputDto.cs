using System.ComponentModel.DataAnnotations;

namespace familytree_api.Dtos.Family
{
    public class FileInputDto
    {
        [Required(ErrorMessage = "File is required")]
        public IFormFile? File { get; set; }
        public int FamilyMemberId { get; set; }
    }
}
