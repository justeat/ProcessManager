using System;

namespace OrderProcessing.Events
{
    public class PaymentCompleted
    {
        public Guid OrderId { get; set; }
    }
}