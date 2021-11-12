using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public void DeleteProductById(int productId) => ProductDAO.Instance.RemoveProductById(productId);

        public ProductObject GetProductById(int productId) => ProductDAO.Instance.GetPoductById(productId);

        public IEnumerable<ProductObject> GetProductList() => ProductDAO.Instance.GetProductList();

        public void InsertProduct(ProductObject product) => ProductDAO.Instance.AddNewProduct(product);

        public IEnumerable<ProductObject> SearchProduct(int ProductId, string ProductName, decimal UnitPrice, int UnitInStock) => ProductDAO.Instance.SearchProduct(ProductId, ProductName, UnitPrice, UnitInStock);

        public void UpdateProduct(ProductObject product) => ProductDAO.Instance.UpdateProduct(product);
    }
}
