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
            public ActionResult Index()
            {
            return View();
            }

        }
    }
