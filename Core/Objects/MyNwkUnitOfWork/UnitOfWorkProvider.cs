namespace Core.Objects.MyNwkUnitOfWork;

public class UnitOfWorkProvider : IUnitOfWorkProvider
{
    private readonly MyNwkDbContext myNwkDbContext;

    public UnitOfWorkProvider(MyNwkDbContext myNwkDbContext)
    {
        this.myNwkDbContext = myNwkDbContext;
    }

    public IUnitOfWork Get() => new UnitOfWork(myNwkDbContext);
}