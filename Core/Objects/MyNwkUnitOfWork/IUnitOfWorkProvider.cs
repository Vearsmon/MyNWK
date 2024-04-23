namespace Core.Objects.MyNwkUnitOfWork;

public interface IUnitOfWorkProvider
{
    public IUnitOfWork Get();
}