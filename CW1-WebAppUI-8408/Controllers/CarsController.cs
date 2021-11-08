using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CW1_WebAppUI_8408.Data;
using CW1_WebAppUI_8408.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace CW1_WebAppUI_8408.Controllers
{
    public class CarsController : Controller
    {
        private readonly CW1_WebAppUI_8408Context _context;
        private string Baseurl = "https://localhost:5001/";

        public CarsController(CW1_WebAppUI_8408Context context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            //Hosted web API REST Service base url
            string Baseurl = "https://localhost:5001/";
            List<Car> CarInfo = new List<Car>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Product");
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    CarInfo = JsonConvert.DeserializeObject<List<Car>>(Response);
                }
                return View(CarInfo);
            }
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string Baseurl = "https://localhost:5001/";
            Car cars = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
            HttpResponseMessage Res = await client.GetAsync("api/Product/" + id);

            if (Res.IsSuccessStatusCode)
            {
                var PrResponse = Res.Content.ReadAsStringAsync().Result;

                cars = JsonConvert.DeserializeObject<Car>(PrResponse);
            }
            else
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
        }

            return View(cars);
        }

        // GET: Cars/Create
        public async Task<IActionResult> CreateAsync()
        {
            List<Category> CarInfo = new List<Category>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Category");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                    CarInfo = JsonConvert.DeserializeObject<List<Category>>(Response);
                }
            }
            ViewData["ProductCategoryId"] = new SelectList(CarInfo, "Id", "Name");
            return View();
        }
                

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,ProductCategoryId")] Car car)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add update logic here
                using (var client = new HttpClient())
                {
                    var randomNumber = new Random();
                    car.Id = randomNumber.Next(150);
                    client.BaseAddress = new Uri(Baseurl);
                    var postTask = await client.PostAsJsonAsync<Car>("api/Product", car);
                    if (postTask.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Category> CarInfo = new List<Category>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Category");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                    CarInfo = JsonConvert.DeserializeObject<List<Category>>(Response);
                }
            }
    

            Car car = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Product/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    car = JsonConvert.DeserializeObject<Car>(Response);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            ViewData["ProductCategoryId"] = new SelectList(CarInfo, "Id", "Name", car.ProductCategoryId);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }


        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,ProductCategoryId")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add update logic here
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Baseurl);
                        HttpResponseMessage Res = await client.GetAsync("api/Product/" + id);
                        Car cars = null;
                        //Checking the response is successful or not which is sent using HttpClient
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api
                            var Response = Res.Content.ReadAsStringAsync().Result;
                            //Deserializing the response recieved from web api and storing into the Product list
                            cars = JsonConvert.DeserializeObject<Car>(Response);
                        }
                        //HTTP POST
                        var postTask = client.PutAsJsonAsync<Car>("api/Product/" + car.Id, car);
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                    }

                    return RedirectToAction("Index");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(car);
        }



        // GET: Cars/Delete/5
        //The function 
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Car car = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Product/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;

                    car = JsonConvert.DeserializeObject<Car>(PrResponse);
                }
            }
                if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string Baseurl = "https://localhost:5001/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.DeleteAsync("api/Product/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }
            }
        }

        private bool CarExists(int id)
        {
            return _context.Car.Any(e => e.Id == id);
        }

    }
}
