using Rabbit.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rabbit.Models
{
	public class RabbitModel
	{
		private IRabbitService<string> service { get; set; }

		public RabbitModel(IRabbitService<string> service)
		{
			this.service = service;
		}

		public string Message { get; set; }
		public void Send(string message)
		{
			service.Send(message);
		}

		public void Recive()
		{
			Message = service.Receive();
		}


	}
}
