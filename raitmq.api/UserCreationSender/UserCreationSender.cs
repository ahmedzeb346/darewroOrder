using Newtonsoft.Json;
using Product.Api.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace raitmq.api.UserCreationSender
{
    public class UserCreationSender: IUserCreationSender
    {
        public void Send(string firstName)
        {

            var factory = new ConnectionFactory 
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "niftyqueue",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var command = new AddUser
                {
                    FirstName = firstName
                };
                string message = JsonConvert.SerializeObject(command);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                        routingKey: "niftyqueue",
                                        basicProperties: null,
                                        body: body);
            }
        }
    }
}
