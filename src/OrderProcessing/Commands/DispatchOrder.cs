using System;
using System.Collections.Generic;
using OrderProcessing.Events;

namespace OrderProcessing.Commands
{
    public class DispatchOrder : Command
    {
        public Guid OrderId { get; set; }
        public Guid RestaurantId { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal Amount { get; set; }
        public Address DeliveryAddress { get; set; }
    }
}