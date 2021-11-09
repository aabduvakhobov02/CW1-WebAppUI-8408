﻿using System;
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
using System.Net.Http.Json;

namespace CW1_WebAppUI_8408.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CW1_WebAppUI_8408Context _context;

        string Baseurl = "http://ec2-54-226-153-32.compute-1.amazonaws.com:5000/";

        public CategoriesController(CW1_WebAppUI_8408Context context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            string Baseurl = "http://ec2-54-226-153-32.compute-1.amazonaws.com:5000/";
            List<Category> CategoryInfo = new List<Category>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                HttpResponseMessage Res = await client.GetAsync("api/Category");
                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    CategoryInfo = JsonConvert.DeserializeObject<List<Category>>(Response);
                }
                return View(CategoryInfo);
            }
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string Baseurl = "http://ec2-54-226-153-32.compute-1.amazonaws.com:5000/";
            Category cat = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Category/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;

                    cat = JsonConvert.DeserializeObject<Category>(Response);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return View(cat);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Category category)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                var random = new Random();
                category.Id = random.Next(99999);
                HttpResponseMessage Res = await client.PostAsJsonAsync("api/Category/", category);

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

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string Baseurl = "http://ec2-54-226-153-32.compute-1.amazonaws.com:5000/";
            Category cat = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Category/" + id);
               
            if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                   
                    cat = JsonConvert.DeserializeObject<Category>(PrResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return View(cat);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Category category)
        {
            try
            {
                // TODO: Add update logic here
                string Baseurl = "http://ec2-54-226-153-32.compute-1.amazonaws.com:5000/";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    HttpResponseMessage Res = await client.GetAsync("api/Category/" + id);
                    Category cat = null;
                    
                    if (Res.IsSuccessStatusCode)
                    {
                        var PrResponse = Res.Content.ReadAsStringAsync().Result;
                       
                        cat = JsonConvert.DeserializeObject<Category>(PrResponse);
                    }

                    var postTask = client.PutAsJsonAsync<Category>("api/Category/" + category.Id,category);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            string Baseurl = "http://ec2-54-226-153-32.compute-1.amazonaws.com:5000/";
            Category cat = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/Category/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;

                    cat = JsonConvert.DeserializeObject<Category>(PrResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }

            return View(cat);

        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string Baseurl = "http://ec2-54-226-153-32.compute-1.amazonaws.com:5000/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.DeleteAsync("api/Category/" + id);
                
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

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
