using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Core.RabbitMQ.Models
{
    public class CommissionMessage
    {
        public int TransactionId { get; set; }

        public decimal CommissionAmount { get; set; }
    }
}
