using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Order_Entity
{
    public enum OrderPaymentStatus
    {
        pending = 0,
        PaymentRecevied = 1,
        PaymentFailed = 2
    }
}
