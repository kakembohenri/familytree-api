using familytree_api.Dtos.Family;
using familytree_api.Dtos.User;

namespace familytree_api.Services.User
{
    public interface IUserService
    {
        Task Create(UserInputDto body);
        Task<UserFilterOutputDto<UserOutputDto>> List(UserFilterDto filter);
        Task Update(UserUpdateDto body);
    }
}
