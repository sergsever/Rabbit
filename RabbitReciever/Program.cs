using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitReciever
{
	class Program
	{
		const string QUEUE = "TEXT";
		const string EXCHANGE = "amq.direct";

		static void GetMessage(IModel model)
		{
			BasicGetResult data = model.BasicGet(QUEUE, true);
			string message = Encoding.UTF8.GetString(data.Body.ToArray());
			Console.WriteLine("Basic Get: " + message);

		}

		static void ConsumeMessage(object param)
		{
			IModel model = (IModel)param;


			/*			
										var consumer = new EventingBasicConsumer(model);

										consumer.Received += (model, msg) =>
										{
											Console.WriteLine($"Rabbit:consumer:{msg.DeliveryTag}");
											var buffer = msg.Body.ToArray();
											string message = Encoding.UTF8.GetString(buffer);
											Console.WriteLine($"Rabbit: Recieve message:{message}");



										};
			*/
			RabbitReciever reciever = new RabbitReciever(model);
			model.BasicQos(0, 1, false);
			string tag = model.BasicConsume(QUEUE, false, reciever);
			Console.WriteLine($"consumer tag: {tag}");
			Console.ReadLine();
			




		}
		static void Main(string[] args)
	{
		try
		{
			ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
			factory.DispatchConsumersAsync = false;
			IConnection connection = factory.CreateConnection();
			IModel model = connection.CreateModel();

							model.QueueDeclare(QUEUE, true, false, false, null);
				//								model.QueueBind(QUEUE, EXCHANGE, "");
				/*
									var customConsumer = new RabbitReciever(channel);

								Debug.WriteLine("Rabbit: start consume");

								model.BasicConsume(QUEUE, true, consumer);
				*/
				//				GetMessage(model);
				//				Thread thconsumer = new Thread(ConsumeMessage);
				ConsumeMessage(model);
		
			}
			catch (Exception e)
			{
				Debug.WriteLine($"Rabbit: Exception: {e.Message}");
			}

		}
	}
}
