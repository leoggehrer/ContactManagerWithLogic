//@CodeCopy
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Logic.Contracts
{
    public interface IContext : IDisposable
    {
        DbSet<Entities.Contact> ContactSet { get; }

        int SaveChanges();
    }
}