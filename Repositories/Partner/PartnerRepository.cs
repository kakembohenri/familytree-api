
using familytree_api.Database;
using familytree_api.Enums;
using Microsoft.EntityFrameworkCore;

namespace familytree_api.Repositories.Partner
{
    public class PartnerRepository(
        AppDbContext _context
        ) : IPartnerRepository
    {
        public async Task<Models.Partner> Create(Models.Partner body)
        {
            await _context.Partner.AddAsync( body );
            await _context.SaveChangesAsync();
            return body;
        }

        public async Task Delete(Models.Partner body)
        {
            _context.Partner.Remove(body);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAll(int familyMemberId)
        {
            List<Models.Partner>  partners = await _context.Partner
     .Where(p => p.HusbandId == familyMemberId || p.WifeId == familyMemberId)
     .ToListAsync();

            if (partners.Count > 0)
            {
                _context.Partner.RemoveRange(partners);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<Models.Partner?> Find(int partnerId)
        {
            return await _context.Partner
                .Where(p => p.Id == partnerId)
     .Include(p => p.Husband!)
         .ThenInclude(h => h.User!)
     .Include(p => p.Wife!)
     .ThenInclude(p => p.User!)
     .SingleOrDefaultAsync();
        }

        public async Task<List<Models.Partner>> Partners(int familyMemberId, PartnerType partnerType)
        {
            IQueryable<Models.Partner> query = _context.Partner
                 .Include(p => p.Husband)
                    .Include(p => p.Wife);

            if (partnerType == PartnerType.Husband)
            {
                query = query.Where(p => p.HusbandId == familyMemberId);
            }else if(partnerType == PartnerType.Wife)
            {
                query = query.Where(p => p.WifeId == familyMemberId);

            }

            return await query
             .ToListAsync();
        }

        public async Task<Models.Partner> Update(Models.Partner body)
        {
            _context.Partner.Update(body);
            await _context.SaveChangesAsync();
            return body;
        }
    }
}
