using familytree_api.Database;
using familytree_api.Dtos.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace familytree_api.Repositories.User
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;

        }

        public async Task<UserFilterOutputDto<UserOutputDto>> List(UserFilterDto filter)
        {
            int skip = (filter.Page - 1) * filter.Limit;
            int count = _context.FamilyMember.Where(fm => fm.FamilyId == filter.FamilyId).Count();

                var passwordHasher = new PasswordHasher<Models.User>();

            List<UserOutputDto> familyMembers = await _context.FamilyMember
                .Where(fm => fm.FamilyId == filter.FamilyId)
                .Include(fm => fm.User)
                .Skip(skip)
                .Take(filter.Limit)
                .Select(fms => new UserOutputDto
                {
                        Id = fms.Id,
                        FamilyId = fms.FamilyId,
                        FirstName= fms.User != null ? fms.User.FirstName ?? string.Empty : string.Empty,
                        MiddleName = fms.User != null ? fms.User.MiddleName ?? string.Empty : string.Empty,
                        LastName = fms.User != null ? fms.User.LastName ?? string.Empty: string.Empty,
                        Gender = fms.Gender ?? "",
                        Phone = fms.User !=null ? fms.User.PhoneNumber ?? string.Empty : string.Empty,
                        Email = fms.User !=null ? fms.User.Email ?? string.Empty : string.Empty,
                        CreatedAt = fms.CreatedAt,

                })
                .ToListAsync();

            UserFilterOutputDto<UserOutputDto> result = new UserFilterOutputDto<UserOutputDto>()
            {
                List = familyMembers,
                Limit = filter.Limit,
                CurrentPage = filter.Page,
                //Pages = count
                Pages = count == 0 ? 1 : (int)Math.Ceiling((double)count / filter.Limit)

            };

            return result;
        }

        public async Task<Models.User> Create(Models.User body)
        {
            await _context.Users.AddAsync(body);
            await _context.SaveChangesAsync();
            return body;
        }

        public async Task<Models.User?> FindByEmail(string Email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == Email);
        }

        public async Task<Models.User?> FindByPhone(string phone)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phone);
        }

        public async Task<Models.User> Update(Models.User body)
        {
            _context.Users.Update(body);
            await _context.SaveChangesAsync();

            return body;
        }

        public async Task<Models.User?> FindById(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Delete(Models.User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
