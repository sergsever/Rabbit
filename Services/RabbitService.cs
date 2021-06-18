using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit.Services
{
	public class RabbitService : IRabbitService<string>
	{
		private IConnection connection { get; set; }
		private const string QUEUE = "TEXT";
		private const string EXCHANGE = "TextExchange";
		private const string TEXTKEY = "keyfortextexchange";
		public RabbitService()
		{
			ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
			connection = factory.CreateConnection();
		}

		public void Send(string message)
		{
			using (var channel = connection.CreateModel())
			{
				channel.ExchangeDeclare(EXCHANGE, ExchangeType.Direct);
				try
				{
					channel.QueueDeclare(QUEUE, true, false, false, null);
//					channel.BasicQos(100, 100, true);

					//			channel.QueueDeclarePassive(QUEUE);
				}
				catch (Exception e)
				{
					Debug.WriteLine($"Exception: {e.Message}");
				}

//				channel.ExchangeDeclare(EXCHANGE, ExchangeType.Direct);

				byte[] buffer = Encoding.UTF8.GetBytes(message);
				var properties = channel.CreateBasicProperties();

				channel.BasicPublish(EXCHANGE, TEXTKEY, properties, buffer);
				Debug.WriteLine($"Rabbit: published: {message}");
			}

		}

		public string Receive()
		{
			string message = "";

			using (var channel = connection.CreateModel())
			{
				try
				{
					
					channel.QueueDeclarePassive(QUEUE);
/*
					var customConsumer = new RabbitReciever(channel);
					string tag = channel.BasicConsume(QUEUE, false, customConsumer);
					Debug.WriteLine($"consumer tag: {tag}");
*/
					var consumer = new AsyncEventingBasicConsumer(channel);

					consumer.Received += async(model, msg) =>
					{
						Debug.WriteLine($"Rabbit:consumer:{msg.DeliveryTag}");
						var buffer = msg.Body.ToArray();
						message = Encoding.UTF8.GetString(buffer);
						Debug.WriteLine($"Rabbit: Recieve message:{message}");
						await Task.Yield();


					};

					Debug.WriteLine("Rabbit: start consume");

					channel.BasicConsume(QUEUE, true, consumer);
				}
				catch(Exception e)
				{
					Debug.WriteLine($"Rabbit: Exception: {e.Message}");
				}
			}

			return message;
		}

		private void Consumer_Received(object sender, BasicDeliverEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
