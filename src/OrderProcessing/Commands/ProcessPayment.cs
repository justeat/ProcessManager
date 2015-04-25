using System;

namespace OrderProcessing.Commands
{
    public class ProcessPayment : Command
    {
        public Guid OrderId { get; set; }
        public Guid RestaurantId { get; set; }
        public decimal Amount { get; set; }
    }
}