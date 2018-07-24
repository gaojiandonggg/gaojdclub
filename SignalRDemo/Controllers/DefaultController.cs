using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Common;

namespace SignalRDemo.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IHubContext<ChatHub> _hubContext;
        public DefaultController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }


        public async Task<IActionResult> Index()
        {
            await _hubContext.Clients.All.SendAsync("Notify", $"Home page loaded at: {DateTime.Now}");
            return View();
        }

        // [Route("notification")]
        public IActionResult Notify()
        {
            return View();
        }

    }
}