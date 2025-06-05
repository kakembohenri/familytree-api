
namespace familytree_api.Dtos.User
{
    public class UserFilterOutputDto<T>
    {
        public int Pages { get; set; }
        public int Limit { get; set; }
        public List<T> List { get; set; } = new List<T>();
        public int CurrentPage { get; set; } = 1;
    }
}
