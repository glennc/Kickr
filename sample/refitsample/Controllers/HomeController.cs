using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kickr.Refit;
using Microsoft.AspNetCore.Mvc;
using refitsample.Models;
using refitsample.Services;

namespace refitsample.Controllers
{
    public class HomeController : Controller
    {
        private IConferencePlannerApi _conferencePlanner;

        public HomeController(IRestClient<IConferencePlannerApi> conferenceClient)
        {
            _conferencePlanner = conferenceClient.Client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
			var val = await _conferencePlanner.GetSessionsAsync();
            ViewData["Message"] = val.ToString();

            return View();
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
