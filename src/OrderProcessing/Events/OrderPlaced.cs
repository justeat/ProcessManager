using System;
using System.Collections.Generic;
using JustSaying.Models;

namespace OrderProcessing.Events
{
    public class OrderPlaced : Message
    {
        public Guid OrderId { get; set; }
        
        public Guid RestaurantId { get; set; }
        public Guid UserId { get; set; }

        public List<OrderItem> Items { get; set; }

        public decimal Amount { get; set; }

        public Address DeliveryAddress { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string PostalCode { get; set; }
    }

    public class OrderItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
