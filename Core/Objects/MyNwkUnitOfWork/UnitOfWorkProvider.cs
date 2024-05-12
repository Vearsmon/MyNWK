namespace Core.Objects.MyNwkUnitOfWork;

public class UnitOfWorkProvider : IUnitOfWorkProvider
{
    private readonly Func<CoreDbContext> coreDbContextGenerator;

    public UnitOfWorkProvider(Func<CoreDbContext> coreDbContextGenerator)
    {
        this.coreDbContextGenerator = coreDbContextGenerator;
    }

    public IUnitOfWork Get() => new UnitOfWork(coreDbContextGenerator());
}