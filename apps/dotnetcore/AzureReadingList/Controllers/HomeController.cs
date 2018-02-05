using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AzureReadingList.Models;
using AzureReadingCore.Models;

namespace AzureReadingList.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {   
            //code to get the ipaddress...going to the list until other features ready.
            //var myIpAddress = HostInfo.Instance.HostIpAddress;
            //ViewData["IpAddress"] = myIpAddress;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "This button will create the default content in your new CosmosDB collection.";

            return View();
        }

        public async Task<IActionResult> Deploy()
        {
            HttpHelper startupHelper = new HttpHelper("api/Books/StartUp");
            string startupHelperResponse = await startupHelper.GetResponse();

            return RedirectToAction("Index", "ReadingList");
        }

        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "ReadingList");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
