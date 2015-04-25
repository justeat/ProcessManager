using System;
using OrderProcessing.ProcessManagers;

namespace OrderProcessing.Infrastructure
{
    public interface IRepository<out T>
    {
        T Load(Guid orderId);
        void Save(OrderProcessManager pm);
    }
}