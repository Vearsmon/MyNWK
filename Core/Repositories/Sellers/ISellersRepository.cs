namespace Core.Repositories.Sellers;

public interface ISellersRepository
{
    IQueryable<SellerEntity> GetSellerEntities();
    SellerEntity GetSellerEntityById(int id);
    void SaveSellerEntity(SellerEntity entity);
    void DeleteSellerEntityById(int id);
}