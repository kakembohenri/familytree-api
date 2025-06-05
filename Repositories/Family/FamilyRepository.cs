
using familytree_api.Database;
using Microsoft.EntityFrameworkCore;

namespace familytree_api.Repositories.Family
{
    public class FamilyRepository(
        AppDbContext _context
        ) : IFamilyRepository
    {

        public async  Task<Models.Family> Create(Models.Family body)
        {
            await _context.Family.AddAsync(body);
            await _context.SaveChangesAsync();
            return body;
        }

        public async Task<Models.Family?> FindById(int id)
        {
            return await _context.Family
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Models.Family> Update(Models.Family body)
        {
            _context.Family.Update(body);
            await _context.SaveChangesAsync();

            return body;
        }
    }
}
