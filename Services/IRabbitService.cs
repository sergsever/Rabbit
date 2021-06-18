using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rabbit.Services
{
	public interface IRabbitService<MsgType>
	{
		void Send(MsgType message);
		MsgType Receive();
	}
}
