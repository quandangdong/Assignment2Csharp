using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public void DeleteOrderById(int orderId) => OrderDAO.Instance.RemoveOrderById(orderId);

        public OrderObject GetOrderById(int orderId) => OrderDAO.Instance.GetOrderById(orderId);

        public IEnumerable<OrderObject> GetOrderList() => OrderDAO.Instance.GetOrderList();

        public void InsertOrder(OrderObject order) => OrderDAO.Instance.AddNewOrder(order);

        public void UpdateOrder(OrderObject order) => OrderDAO.Instance.UpdateProduct(order);
    }
}
