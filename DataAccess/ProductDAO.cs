using BusinessObject;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class ProductDAO : BaseDAO
    {
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        private ProductDAO() { }
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        //---------------------------------------------------
        public IEnumerable<ProductObject> GetProductList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "SELECT ProductId, CategoryId, ProductName, Weight, UnitPrice, UnitsInStock FROM Product";
            var products = new List<ProductObject>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    products.Add(new ProductObject
                    {
                        ProductId = dataReader.GetInt32(0),
                        CategoryId = dataReader.GetInt32(1),
                        ProductName = dataReader.GetString(2),
                        Weight = dataReader.GetString(3),
                        UnitPrice = dataReader.GetDecimal(4),
                        UnitInStock = dataReader.GetInt32(5),
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return products;
        }

        //---------------------------------------------------
        public ProductObject GetPoductById(int productId)
        {
            ProductObject product= null;
            IDataReader dataReader = null;
            string SQLSelect = "SELECT ProductId, CategoryId, ProductName, Weight, UnitPrice, UnitsInStock FROM Product " +
                "WHERE ProductId = @productId";
            try
            {
                var param = dataProvider.CreateParameter("@productId", 4, productId, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    product = new ProductObject
                    {
                        ProductId = dataReader.GetInt32(0),
                        CategoryId = dataReader.GetInt32(1),
                        ProductName = dataReader.GetString(2),
                        Weight = dataReader.GetString(3),
                        UnitPrice = dataReader.GetDecimal(4),
                        UnitInStock = dataReader.GetInt32(5),
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return product;
        }

        //---------------------------------------------------
        public void AddNewProduct(ProductObject product)
        {
            try
            {
                ProductObject pro = GetPoductById(product.ProductId);
                if (pro == null)
                {
                    string SQLInsert = "INSERT Product values(@ProductId, @CategoryId, @ProductName, " +
                        "@Weight, @UnitPrice, @UnitsInStock)";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@ProductId", 4, product.ProductId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@CategoryId", 40, product.CategoryId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@ProductName", 40, product.ProductName, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Weight", 40, product.Weight, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@UnitPrice", 40, product.UnitPrice, DbType.Decimal));
                    parameters.Add(dataProvider.CreateParameter("@UnitsInStock", 40, product.UnitInStock, DbType.Double));
                    dataProvider.Insert(SQLInsert, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("The Member ID is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        //---------------------------------------------------
        public void UpdateProduct(ProductObject product)
        {
            try
            {
                ProductObject pro = GetPoductById(product.ProductId);
                if (pro != null)
                {
                    string SQLUpdate = "UPDATE Product SET " +
                        "ProductId = @ProductId," +
                        "CategoryId = @CategoryId," +
                        "ProductName = @ProductName," +
                        "Weight = @Weight," +
                        "UnitPrice = @UnitPrice," +
                        "UnitsInStock = @UnitStock " +
                        "WHERE ProductId = @ProductId";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@ProductId", 4, product.ProductId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@CategoryId", 40, product.CategoryId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@ProductName", 40, product.ProductName, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@Weight", 40, product.Weight, DbType.String));
                    parameters.Add(dataProvider.CreateParameter("@UnitPrice", 40, product.UnitPrice, DbType.Decimal));
                    parameters.Add(dataProvider.CreateParameter("@UnitStock", 40, product.UnitInStock, DbType.Double));
                   
                    dataProvider.Update(SQLUpdate, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Cannot update product!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        //---------------------------------------------------
        public void RemoveProductById(int productId)
        {
            try
            {
                ProductObject pro= GetPoductById(productId);
                if (pro != null)
                {
                    string SQLDelete = "DELETE Product WHERE ProductId = @ProductId";
                    var param = dataProvider.CreateParameter("@ProductId", 4, productId, DbType.Int32);
                    dataProvider.Delete(SQLDelete, CommandType.Text, param);
                }
                else
                {
                    throw new Exception("Product does not exist to delete!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        //---------------------------------------------------
        public MemberObject GetMemberByEmailAndPassword(string email, string password)
        {
            MemberObject member = null;
            IDataReader dataReader = null;
            string SQLSelect = "SELECT MemberId, Email, CompanyName, City, Country, Password " +
                " FROM Member WHERE Email = @Email AND Password = @Password";
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@Email", 40, email, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@Password", 40, password, DbType.String));
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, parameters.ToArray());

                if (dataReader.Read())
                {
                    member = new MemberObject
                    {
                        MemberId = dataReader.GetInt32(0),
                        Email = dataReader.GetString(1),
                        CompanyName = dataReader.GetString(2),
                        City = dataReader.GetString(3),
                        Country = dataReader.GetString(4),
                        Password = dataReader.GetString(5)
                    };
                }
            }
            catch (Exception ex)
            {
                member = null;
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return member;
        }

        //---------------------------------------------------
        public IEnumerable<ProductObject> SearchProduct(int ProductId, string ProductName, decimal UnitPrice, int UnitInStock)
        {
            IDataReader dataReader = null;
            var products = new List<ProductObject>();
            string SQLSelect = "SELECT ProductId, CategoryId, ProductName, Weight, UnitPrice, UnitsInStock FROM Product " +
                "WHERE ProductId = @productId AND ProductName = @ProductName AND UnitPrice = @UnitPrice AND UnitsInStock = @UnitsInStock";
            try
            {
                var parameters = new List<SqlParameter>();
                parameters.Add(dataProvider.CreateParameter("@ProductId", 4, ProductId, DbType.Int32));
                parameters.Add(dataProvider.CreateParameter("@ProductName", 40, ProductName, DbType.String));
                parameters.Add(dataProvider.CreateParameter("@UnitPrice", 40, UnitPrice, DbType.Decimal));
                parameters.Add(dataProvider.CreateParameter("@UnitsInStock", 40, UnitInStock, DbType.Double));
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, parameters.ToArray());

                if (dataReader.Read())
                {
                    products.Add(new ProductObject
                    {
                        ProductId = dataReader.GetInt32(0),
                        CategoryId = dataReader.GetInt32(1),
                        ProductName = dataReader.GetString(2),
                        Weight = dataReader.GetString(3),
                        UnitPrice = dataReader.GetDecimal(4),
                        UnitInStock = dataReader.GetInt32(5),
                    });
                }
            }
            catch (Exception ex)
            {
                products = null;
                throw new Exception(ex.Message);
            }
            finally
            {
                dataReader.Close();
                CloseConnection();
            }
            return products;
        }



    }//end class

} //End namespace
