﻿using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Restack.Mvc;
using Restack.Refit;
using Restack.WebApp.Models;
using Restack.WebApp.Services;

namespace Restack.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGeoApi _geoApi;

        public HomeController(IRestClient<IGeoApi> geoApiClient)
        {
            _geoApi = geoApiClient.Client;
        }

        public async Task<IActionResult> Index([HttpClientName("github")]HttpClient client)
        {
            var response = await client.GetAsync("https://api.github.com/users/lecaillon");
            return View();
        }

        public async Task<IActionResult> About()
        {
            var regions = await _geoApi.GetRegionsAsync();
            ViewData["Message"] = string.Join(", ", regions.Select(x => x.Nom));

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
