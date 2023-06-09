﻿using ErciyesSozluk.Common.BlazorSozluk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ErciyesSozluk.Common.Infrastructure
{
    public static class QueueFactory
    {
        public static void SendMessageToExchange(string exchangeName, string exchangeType, string queueName, object obj)
        {
            var channel = CreateBasicConsumer()
                .EnsureExchange(exchangeName, exchangeType)
                .EnsureQueue(queueName, exchangeName)
                .Model;

            //JSON olarak gelen mesaj serialize edilerek string'e cevrilir
            //sonrasinda encode edilip bytearray'e cevrilir
            //BODY'ye gelen obj'ler eventlerdir
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj));

            channel.BasicPublish(exchange: exchangeName, routingKey: queueName, basicProperties: null, body: body);
        }

        public static EventingBasicConsumer CreateBasicConsumer()
        {
            var factory = new ConnectionFactory()
            {
                HostName = SozlukConstants.RabbitMQHost,
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            return new EventingBasicConsumer(channel);
        }

        public static EventingBasicConsumer EnsureExchange(this EventingBasicConsumer consumer, string exchangeName, string exchangeType = SozlukConstants.DefaultExchangeType)
        {
            consumer.Model.ExchangeDeclare(exchange: exchangeName, type: exchangeType, durable: false, autoDelete: false);
            return consumer;
        }

        public static EventingBasicConsumer EnsureQueue(this EventingBasicConsumer consumer, string queueName, string exchangeName)
        {
            consumer.Model.QueueDeclare(queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                null);

            consumer.Model.QueueBind(queueName, exchangeName, queueName);
            return consumer;
        }

        public static EventingBasicConsumer Receive<T>(this EventingBasicConsumer consumer, Action<T> action)
        {
            //rabbitmq join işlemi
            consumer.Received += (m, eventArgs) =>
            {
                //rabbitmq'dan kayıt geldiğinde yapılacak işlemler
                //data json olarak okunur
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //okunan json ilgili tipe cast edilir
                var model = JsonSerializer.Deserialize<T>(message);
                action(model);
                consumer.Model.BasicAck(eventArgs.DeliveryTag, false);
            };

            return consumer;
        }

        public static EventingBasicConsumer StartConsuming(this EventingBasicConsumer consumer, string queueName)
        {
            consumer.Model.BasicConsume(queue: queueName,
                autoAck: false,
                consumer: consumer);

            return consumer;
        }
    }
}
