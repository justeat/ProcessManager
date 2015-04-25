using System;
using JustSaying.Messaging.MessageHandling;
using OrderProcessing.Events;
using OrderProcessing.Infrastructure;
using OrderProcessing.ProcessManagers;

namespace OrderProcessing
{
    public class OrderProcessRouter:
        IHandler<OrderPlaced>,
        IHandler<PaymentCompleted>
    {
        private readonly IRepository<OrderProcessManager> _repository;

        public OrderProcessRouter(IRepository<OrderProcessManager> repository)
        {
            _repository = repository;
        }

        public bool Handle(OrderPlaced message)
        {
            var pm = _repository.Load(message.OrderId);

            if (pm == null)
            {
                pm = new OrderProcessManager();
            }

            pm.When(message);
            _repository.Save(pm);

            return true;
        }

        public bool Handle(PaymentCompleted message)
        {
            var pm = _repository.Load(message.OrderId);

            if (pm == null)
            {
                throw new Exception("Could not locate Process Manager instance with id " + message.OrderId);
            }

            pm.When(message);
            _repository.Save(pm);

            return true;
        }
    }
}
