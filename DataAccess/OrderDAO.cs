using BusinessObject;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess
{
    public class OrderDAO : BaseDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDAO() { }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }

        //---------------------------------------------------
        public IEnumerable<OrderObject> GetOrderList()
        {
            IDataReader dataReader = null;
            string SQLSelect = "SELECT OrderId, MemberId, OrderDate, RequireDate, ShippedDate, Freight FROM Orders";
            var orders = new List<OrderObject>();
            try
            {
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection);
                while (dataReader.Read())
                {
                    orders.Add(new OrderObject
                    {
                        OrderId = dataReader.GetInt32(0),
                        MemberId = dataReader.GetInt32(1),
                        OrderDate = dataReader.GetDateTime(2),
                        RequireDate = dataReader.GetDateTime(3),
                        ShippedDate = dataReader.GetDateTime(4),
                        Freight = dataReader.GetDecimal(5),
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
            return orders;
        }

        //---------------------------------------------------
        public OrderObject GetOrderById(int orderId)
        {
            OrderObject order = null;
            IDataReader dataReader = null;
            string SQLSelect = "SELECT OrderId, MemberId, OrderDate, RequireDate, ShippedDate, Freight FROM Orders " +
                "WHERE OrderId = @OderId";
            try
            {
                var param = dataProvider.CreateParameter("@OderId", 4, orderId, DbType.Int32);
                dataReader = dataProvider.GetDataReader(SQLSelect, CommandType.Text, out connection, param);
                if (dataReader.Read())
                {
                    order = new OrderObject
                    {
                        OrderId = dataReader.GetInt32(0),
                        MemberId = dataReader.GetInt32(1),
                        OrderDate = dataReader.GetDateTime(2),
                        RequireDate = dataReader.GetDateTime(3),
                        ShippedDate = dataReader.GetDateTime(4),
                        Freight = dataReader.GetDecimal(5),
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
            return order;
        }

        //---------------------------------------------------
        public void AddNewOrder(OrderObject order)
        {
            try
            {
                OrderObject ord =  GetOrderById(order.OrderId);
                if (ord == null)
                {
                    string SQLInsert = "INSERT Product values(@OrderId, @MemberId, @OrderDate, @RequireDate, @ShippedDate, @Freight)";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@OrderId", 4, order.OrderId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@MemberId", 40, order.MemberId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@OrderDate", 40, order.OrderDate, DbType.DateTime));
                    parameters.Add(dataProvider.CreateParameter("@RequireDate", 40, order.RequireDate, DbType.DateTime));
                    parameters.Add(dataProvider.CreateParameter("@ShippedDate", 40, order.ShippedDate, DbType.DateTime));
                    parameters.Add(dataProvider.CreateParameter("@Freight", 40, order.Freight, DbType.Decimal));
                    dataProvider.Insert(SQLInsert, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("The Order ID is already exist.");
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
        public void UpdateProduct(OrderObject order)
        {
            try
            {
                OrderObject ord = GetOrderById(order.OrderId);
                if (ord != null)
                {
                    string SQLUpdate = "UPDATE Orders SET " +
                        "OrderId = @OrderId," +
                        "MemberId = @MemberId," +
                        "OrderDate = @OrderDate," +
                        "RequireDate = @RequireDate," +
                        "ShippedDate = @ShippedDate," +
                        "Freight = @Freight " +
                        "WHERE OrderId = @OrderId";
                    var parameters = new List<SqlParameter>();
                    parameters.Add(dataProvider.CreateParameter("@OrderId", 4, order.OrderId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@MemberId", 40, order.MemberId, DbType.Int32));
                    parameters.Add(dataProvider.CreateParameter("@OrderDate", 40, order.OrderDate, DbType.DateTime));
                    parameters.Add(dataProvider.CreateParameter("@RequireDate", 40, order.RequireDate, DbType.DateTime));
                    parameters.Add(dataProvider.CreateParameter("@ShippedDate", 40, order.ShippedDate, DbType.DateTime));
                    parameters.Add(dataProvider.CreateParameter("@Freight", 40, order.Freight, DbType.Decimal));

                    dataProvider.Update(SQLUpdate, CommandType.Text, parameters.ToArray());
                }
                else
                {
                    throw new Exception("Cannot update order!");
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
        public void RemoveOrderById(int orderId)
        {
            try
            {
                OrderObject ord = GetOrderById(orderId);
                if (ord != null)
                {
                    string SQLDelete = "DELETE Orders WHERE OrderId = @OrderId";
                    var param = dataProvider.CreateParameter("@OrderId", 4, orderId, DbType.Int32);
                    dataProvider.Delete(SQLDelete, CommandType.Text, param);
                }
                else
                {
                    throw new Exception("Order does not exist to delete!");
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



    }//end class

} //End namespace
