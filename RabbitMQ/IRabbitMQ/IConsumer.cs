using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.IRabbitMQ
{
    public interface IConsumer
    {
        public T GetMessage<T>();
    }
}
