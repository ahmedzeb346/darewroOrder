using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Product.Api.Models;
using Product.Api.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace raitmq.api.CustomerUpdateReceiver
{
    public  class CustomerFullNameUpdateReceiver : BackgroundService
    {
        private readonly ICustomerNameUpdateService _customerNameUpdateService;

        private IModel _channel;
        private IConnection _connection;
        
        private readonly string _hostname;
        //private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public CustomerFullNameUpdateReceiver()
        {
            
            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
            };

            _connection = factory.CreateConnection();
           // _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "niftyqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var updateCustomerFullNameModel = JsonConvert.DeserializeObject<AddUser>(content);

                HandleMessage(updateCustomerFullNameModel);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Received += EventingBasicConsumer_Received;
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume("niftyqueue", false, consumer);

            return Task.CompletedTask;
        }

        private void EventingBasicConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            IBasicProperties basicProperties = e.BasicProperties;
            Console.WriteLine("Message received by the event based consumer. Check the debug window for details.");
            Debug.WriteLine(string.Concat("Message received from the exchange ", e.Exchange));
            Debug.WriteLine(string.Concat("Content type: ", basicProperties.ContentType));
            Debug.WriteLine(string.Concat("Consumer tag: ", e.ConsumerTag));
            Debug.WriteLine(string.Concat("Delivery tag: ", e.DeliveryTag));
            Debug.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(e.Body.ToArray())));
            _channel.BasicAck(e.DeliveryTag, false);
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HandleMessage(AddUser updateCustomerFullNameModel)
        {
            _customerNameUpdateService.UpdateCustomerNameInOrders(updateCustomerFullNameModel);
        }


        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
      
    }
}
