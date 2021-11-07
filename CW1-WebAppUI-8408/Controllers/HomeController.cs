using CW1_WebAppUI_8408.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CW1_WebAppUI_8408.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            //Hosted web API REST Service base url
            string Baseurl = "http://localhost:57719/";
            List<Car> CarInfo = new List<Car>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 HttpResponseMessage Res = await client.GetAsync("api/Product");
            if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                     CarInfo = JsonConvert.DeserializeObject<List<Car>>(PrResponse);
                }
                //returning the Product list to view
                return View(CarInfo);
            }
        }
    }
}
