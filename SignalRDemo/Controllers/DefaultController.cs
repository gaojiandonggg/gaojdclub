using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaoJD.Club.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Common;

namespace SignalRDemo.Controllers
{
    public class DefaultController : Controller
    {

        //private readonly IHubContext<ChatHub> _hubContext;
        private IUserLogic _userLogic;
        public DefaultController(IUserLogic userLogic)
        {
            this._userLogic = userLogic;
        }


        public async Task<IActionResult> Index()
        {
            // await _hubContext.Clients.All.SendAsync("Notify", $"Home page loaded at: {DateTime.Now}");
            return View();
        }

        // [Route("notification")]
        public IActionResult Notify()

        {
            return View();
        }
        // [Route("notification")]
        public IActionResult ChatHub(int id, string groupName)

        {
            ViewBag.groupName = groupName;
            ViewBag.id = id;
            return View();
        }

        public IActionResult UserList(string groupName)
        {
            List<GaoJD.Club.BusinessEntity.User> list = _userLogic.GetAll();
            ViewBag.groupName = groupName;
            return View(list);
        }
    }
}