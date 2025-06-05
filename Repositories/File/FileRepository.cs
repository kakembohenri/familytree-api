
using familytree_api.Database;
using Microsoft.EntityFrameworkCore;

namespace familytree_api.Repositories.File
{
    public class FileRepository(
        AppDbContext _context
        
        ) : IFileRepository
    {
        public async Task<Models.Image> Create(Models.Image body)
        {
           await _context.Image.AddAsync( body );
           await _context.SaveChangesAsync();
            return body;
        }

        public async Task Delete(int id)
        {
            Models.Image? image = await _context.Image.SingleOrDefaultAsync(i => i.Id == id);

            if (image != null)
            {
                _context.Image.Remove(image);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAll(int familyMemberId)
        {
            List<Models.Image> images = await _context.Image.ToListAsync();

            if (images.Count > 0)
            {
                _context.Image.RemoveRange(images);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Models.Image?> Find(int id)
        {
           return await _context.Image
                .AsNoTracking()
                .SingleOrDefaultAsync(i => i.Id == id);

        }

        public async Task<List<Models.Image>> List(int fmailyMemberId)
        {
            return await _context.Image
                .AsNoTracking()
                .Where(i => i.FamilyMemberId == fmailyMemberId).ToListAsync();
        }
    }
}
