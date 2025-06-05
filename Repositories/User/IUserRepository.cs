using familytree_api.Dtos.User;

namespace familytree_api.Repositories.User
{
    public interface IUserRepository
    {
        Task<Models.User> Create(Models.User body);
        Task<Models.User?> FindByEmail(string Email);
        Task<Models.User?> FindByPhone(string phone);
        Task<Models.User> Update(Models.User body);
        Task<Models.User?> FindById(int id);
        Task Delete(Models.User user);
        Task<UserFilterOutputDto<UserOutputDto>> List(UserFilterDto filter);
    }
}
