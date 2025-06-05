
using Microsoft.EntityFrameworkCore.Storage;

namespace familytree_api.Database
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }


        public async Task BeginTransactionAsync()
        {
            _transaction =  await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
    }
}
