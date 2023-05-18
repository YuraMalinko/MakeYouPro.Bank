
using RabbitMQ;

Publisher<List<string>> publisher = new Publisher<List<string>> { Value = { "dent,nocs,coda","sad","asd"} };

publisher.SendMessege();