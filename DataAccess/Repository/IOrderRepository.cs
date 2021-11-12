using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<OrderObject> GetOrderList();
        OrderObject GetOrderById(int orderId);
        void InsertOrder(OrderObject order);
        void UpdateOrder(OrderObject order);
        void DeleteOrderById(int orderId);
    }
}
