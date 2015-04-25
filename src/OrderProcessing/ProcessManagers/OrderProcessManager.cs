using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.CloudFront;
using Amazon.ElastiCache.Model;
using OrderProcessing.Commands;
using OrderProcessing.Events;

namespace OrderProcessing.ProcessManagers
{
    public class OrderProcessManager
    {
        public enum OrderProcessState
        {
            NotStarted,
            OrderPlaced,
            PaymentCompleted,
            OrderDispatched,
            OrderDelivered
        }

        public OrderProcessManager()
        {
            State = OrderProcessState.NotStarted;
        }

        public Guid Id { get; set; }
        public OrderProcessState State { get; set; }
        public int Version { get; set; }

        public List<Command> CommandsToSend { get; private set; } 

        public Guid RestaurantId { get; set; }
        public Guid UserId { get; set; }
        public List<OrderItem> Items { get; private set; }
        public Address DeliveryAddress { get; set; }
        public decimal Amount { get; set; }


        public void When(OrderPlaced @event)
        {
            switch (State)
            {
                case OrderProcessState.NotStarted:
                    State = OrderProcessState.OrderPlaced;
                    Id = @event.OrderId;
                    Items = @event.Items;
                    RestaurantId = @event.RestaurantId;
                    UserId = @event.UserId;
                    DeliveryAddress = @event.DeliveryAddress;
                    Amount = @event.Amount;

                    SendCommand(new ProcessPayment
                    {
                        OrderId = @event.OrderId,
                        RestaurantId = @event.RestaurantId,
                        Amount = @event.Amount
                    });

                    break;
                // idempotence - same message sent twice
                case OrderProcessState.OrderPlaced:
                    break;
                default:
                    throw new InvalidOperationException("Invalid state for this message");
            }
        }

        private void SendCommand(Command command)
        {
            CommandsToSend.Add(command);
        }

        public void When(PaymentCompleted @event)
        {
            switch (State)
            {
                case OrderProcessState.OrderPlaced:
                    State = OrderProcessState.PaymentCompleted;

                    SendCommand(new DispatchOrder
                    {
                        OrderId = Id,
                        RestaurantId = RestaurantId,
                        Items = Items.ToList(),
                        Amount = Amount,
                        DeliveryAddress = DeliveryAddress
                    });
                    break;
                // idempotence - same message sent twice
                case OrderProcessState.PaymentCompleted:
                    break;
                default:
                    throw new InvalidOperationException("Invalid state for this message");
            }
        }

        public void When(OrderDispatched @event)
        {
        }

        public void When(OrderDelivered @event)
        {
        }
    }
}