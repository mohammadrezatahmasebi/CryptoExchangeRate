using Microsoft.EntityFrameworkCore;

namespace Si24.Infra.FormBuilder.Query.Framework;
public abstract class BaseQueryDbContext : DbContext
{
    protected BaseQueryDbContext()
    {
    }

    protected BaseQueryDbContext(DbContextOptions options) : base(options)
    {
    }


    protected void SaveDomainEvents()
    {

    }
}
