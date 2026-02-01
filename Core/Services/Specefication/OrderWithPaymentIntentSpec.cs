using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities.Order_Entity;

namespace Services.Specefication
{
    internal class OrderWithPaymentIntentSpec : Specefication<Order>
    {
        public OrderWithPaymentIntentSpec(string paymentIntentId) : base(o => o.PaymenIntentId == paymentIntentId)
        {
        }
    }
}
