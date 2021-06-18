using RabbitMQ.Client;
using System;
using System.Diagnostics;
using System.Text;

namespace RabbitSender
{
	class Program
	{
		static void Main(string[] args)
		{
			const string QUEUE = "TEXT";
			const string EXCHANGE = "amq.direct";
			try
			{
				string message = "Test message";
				ConnectionFactory factory = new ConnectionFactory();
				factory.HostName = "localhost";
				factory.UserName = "guest";
				factory.Password = "guest";
				factory.VirtualHost = "/";
				factory.Port = 5672;
				factory.HandshakeContinuationTimeout = new TimeSpan(0, 0, 60);
				factory.RequestedHeartbeat = new TimeSpan(0, 0, 60);
				IConnection connection = factory.CreateConnection();
				IModel model = connection.CreateModel();
//				model.ExchangeDeclare(EXCHANGE, ExchangeType.Direct, false, false, null);
				model.QueueDeclare(QUEUE, true, false, false, null);
				model.QueueBind(QUEUE, EXCHANGE, "");
				byte[] buffer = Encoding.UTF8.GetBytes(message);
				IBasicProperties properties = model.CreateBasicProperties();
				properties.Persistent = true;
				model.BasicPublish(EXCHANGE, "", properties, buffer);
				Console.WriteLine("Sended");
				model.Close();
				connection.Close();





			}
			catch(Exception e)
			{
				Debug.WriteLine($"Exception: {e.Message}");
			}

			Console.WriteLine("Rabbit");
		}
	}
}
