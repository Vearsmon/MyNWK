namespace Core.Objects.MyNwkUnitOfWork;

public class UnitOfWorkProvider : IUnitOfWorkProvider
{
    private readonly CoreDbContext coreDbContext;

    public UnitOfWorkProvider(CoreDbContext coreDbContext)
    {
        this.coreDbContext = coreDbContext;
    }

    public IUnitOfWork Get() => new UnitOfWork(coreDbContext);
}