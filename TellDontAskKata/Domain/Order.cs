﻿using System.Collections.Generic;

namespace TellDontAskKata.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public string Currency { get; set; }
        public List<OrderItem> Items { get; set; }
        public decimal Tax { get; set; }
        public OrderStatus Status { get; set; }
    }
}
