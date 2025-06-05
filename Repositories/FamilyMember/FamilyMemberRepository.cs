
using familytree_api.Database;
using familytree_api.Models;
using Microsoft.EntityFrameworkCore;

namespace familytree_api.Repositories.FamilyMember
{
    public class FamilyMemberRepository(
        AppDbContext _context
        ) : IFamilyMemberRepository
    {
        public async Task<Models.FamilyMember> Create(Models.FamilyMember body)
        {
            await _context.FamilyMember.AddAsync(body);
            await _context.SaveChangesAsync();
            return body;
        } 
        
        public async Task<Models.FamilyMember> Update(Models.FamilyMember body)
        {
            _context.FamilyMember.Update(body);
            await _context.SaveChangesAsync();
            return body;
        }

        public async Task<Models.FamilyMember?> Find(int id)
        {
            return await _context.FamilyMember.Where(fm => fm.Id == id)
                .Include(fm => fm.Father!)
                .ThenInclude(fm => fm.User)
                .Include(fm => fm.Mother!)
                .ThenInclude(fm => fm.User)
                .Include(fm => fm.User)
                .Include(fm => fm.Family)
                .FirstOrDefaultAsync();
        }
       

        public async Task<Models.FamilyMember?> FindByUserId(int userId)
        {
            return await _context.FamilyMember
                .Where(fm => fm.UserId == userId)
                .Include(fm => fm.Father!)
                .ThenInclude(fm => fm.User)
                .Include(fm => fm.Mother!)
                .ThenInclude(fm => fm.User)
                .Include(fm => fm.User)
                .Include(fm => fm.Family)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Models.FamilyMember>> GetChildren(int? fatherId, int? motherId)
        {
            var query = _context.FamilyMember
                .Where(fm => fm.FatherId == fatherId);

            if(motherId != null)
            {
                query = query.Where(fm => fm.MotherId == motherId);
            }

            var familyMembers =  await query
                .Include(fm => fm.Father!)
                .ThenInclude(fm => fm.User)
                .Include(fm => fm.Mother!)
                .ThenInclude(fm => fm.User)
                .Where(fm => fm.ShowInTree == true)
                .Include(fm => fm.User)
                .Include(fm => fm.Family)
                .ToListAsync();

            return familyMembers;
        }

        public async Task<Models.FamilyMember?> ShowInTree(int familyId)
        {
            return await _context.FamilyMember
                .Where(fm => fm.ShowInTree == true)
                .Where(fm => fm.FamilyId == familyId)
                .Include(fm => fm.Father!)
                .ThenInclude(fm => fm.User)
                .Include(fm => fm.Mother!)
                .ThenInclude(fm => fm.User)
                .Include(fm => fm.User)
                .Include(fm => fm.Family)
                .FirstOrDefaultAsync();
        }


        public async Task Delete(Models.FamilyMember body)
        {
            _context.FamilyMember.Remove(body);
            await _context.SaveChangesAsync();
        }
      
    }
}
