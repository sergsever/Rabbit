using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitReciever
{
	public class RabbitReciever : DefaultBasicConsumer
	{
		public RabbitReciever(IModel channel) : base(channel)
		{
		}
		

		
		public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
		{
			string message = Encoding.UTF8.GetString(body.ToArray());
			Console.WriteLine("custom consumer: handle msg: " + message);
			base.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties, body);
		}
	}
}
