using Microsoft.AspNetCore.Mvc;
using Rabbit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rabbit.Controllers
{
	public class RabbitController : Controller
	{
		private RabbitModel model { get; set; }

		public RabbitController(RabbitModel model)
		{
			this.model = model;
		}
		public IActionResult Index()
		{
			model.Recive();
			return View(model);
		}

		public IActionResult Send(string message)
		{
//			model.Send(message);
			return View("Index", model);
		}
	}
}
