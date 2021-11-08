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
            // GET: Home
            public async Task<ActionResult> Index()
            {
                //Hosted web API REST Service base url
                string Baseurl = "https://localhost:5001/";
                List<Car> CarInfo = new List<Car>();
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                     HttpResponseMessage Res = await client.GetAsync("api/Product");
                    //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var Response = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Product list
                         CarInfo = JsonConvert.DeserializeObject<List<Car>>(Response);
                    }
                    //returning the Product list to view
                    return View(CarInfo);
                }
            }

        }
    }
