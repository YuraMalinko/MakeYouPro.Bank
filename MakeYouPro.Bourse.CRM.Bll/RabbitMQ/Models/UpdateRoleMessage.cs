using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Bll.RabbitMQ.Models
{
    public class UpdateRoleMessage
    {
        public int Id { get; set; }

        public int Status { get; set; }
    }
}
