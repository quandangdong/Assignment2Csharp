using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        IEnumerable<ProductObject> GetProductList();
        ProductObject GetProductById(int productId);
        void InsertProduct(ProductObject product);
        void UpdateProduct(ProductObject product);
        void DeleteProductById(int productId);
        IEnumerable<ProductObject> SearchProduct(int ProductId, string ProductName, decimal UnitPrice, int UnitInStock);
    }
}
