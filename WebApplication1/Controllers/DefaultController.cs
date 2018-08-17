using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication1.Interface;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IOptions<MyOptions> optionsAccessor;
        private readonly ILogger<DefaultController> logger;
        private readonly IFileProvider fileProvider;
        private readonly IPersonRepostitoy personRepostitoy;
        private readonly MyOptions _named_options_1;
        private readonly MyOptions _named_options_2;
        private Action<ILogger, Exception> _indexPageRequested;
        private Action<int, int> _Test;
        public IDirectoryContents DirectoryContents { get; private set; }
        public DefaultController(IOptions<MyOptions> optionsAccessor, IOptionsSnapshot<MyOptions> namedOptionsAccessor, ILogger<DefaultController> logger, IFileProvider fileProvider, IPersonRepostitoy personRepostitoy)
        {
            this.optionsAccessor = optionsAccessor;
            this.logger = logger;
            this.fileProvider = fileProvider;
            this.personRepostitoy = personRepostitoy;
            _named_options_1 = namedOptionsAccessor.Get("named_options_1");
            _named_options_2 = namedOptionsAccessor.Get("named_options_2");


        }
        // GET: Default
        public ActionResult Index()
        {

            string path = Directory.GetCurrentDirectory();
            string target = @"c:\temp";
            Console.WriteLine("The current directory is {0}", path);
            if (!Directory.Exists(target))
            {
                Directory.CreateDirectory(target);
            }

            // Change the current directory.
            Environment.CurrentDirectory = (target);
            if (path.Equals(Directory.GetCurrentDirectory()))
            {
                Console.WriteLine("You are in the temp directory.");
            }
            else
            {
                Console.WriteLine("You are not in the temp directory.");
            }


            DirectoryContents = fileProvider.GetDirectoryContents(string.Empty);
            var aaa = fileProvider.GetDirectoryContents(@"H:\学习\NetCoreTest\githubdoc\WebApplication1\WebApplication1\obj");
            var bbb = fileProvider.GetDirectoryContents("obj");
            var fileInfo = fileProvider.GetFileInfo("wwwroot/js/site.js");
            var ccc = fileProvider.GetFileInfo("wwwroot");
            throw new Exception();
            logger.LogInformation("111", "222");
            logger.LogDebug("Debug", "Debug");
            logger.LogWarning("Warning", "Warning");
            var snapshotOption1 = optionsAccessor.Value.Option1;
            var snapshotOption2 = optionsAccessor.Value.Option2;


            _indexPageRequested = LoggerMessage.Define(
    LogLevel.Information,
    new EventId(1, null),
    "GET request for Index page");
            _Test = AAA;
            AAA(1, 2);
            return View();
        }

        public void AAA(int x, int y)
        {

        }


        public void TestInvoke()
        {
            Action<int, int, int> action = (i, j, x) =>
             {
                 var aa = i + j + x;
             };
            action(1, 2, 3);
            action.Invoke(1, 2, 3);
        }

        public void Test()
        {
            personRepostitoy.Insert(new Person() { });

            personRepostitoy.GetPerson("111");

            MyLazy<Large> aaa = new MyLazy<Large>();

            Large a = aaa.Value;
        }




        // GET: Default/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Default/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Default/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Default/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Default/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Default/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Default/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public string PostInput([FromBody]User_Input user_Input)
        {
            Type t1 = user_Input.GetType();
            PropertyInfo propertyInfo = t1.GetProperty("Email");
            propertyInfo.SetValue(user_Input, "1111");

            return "我是测试";
        }

    }
}