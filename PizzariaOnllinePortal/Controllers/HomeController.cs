using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using MiddleLayer;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using PizzariaOnlinePortal.ViewModels;
using PizzariaOnlinePortal.Utility;

namespace PizzariaOnlinePortal.Controllers
{
    //[Route("Home")]    
    public class HomeController : Controller
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly string apiBaseUrl;

        public HomeController(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            this._configuration = configuration;
            apiBaseUrl = _configuration.GetValue<string>("WebAPIBaseUrl");
        }
        [AllowAnonymous]
        public async Task<ViewResult> Index()
        {
            IList<HomeListViewModel> homeListView = new List<HomeListViewModel>();
            HomeListViewModel homeViewModel = new HomeListViewModel();
            IEnumerable<Ingredient> listIngredient;
            IEnumerable<Pizza> Pizzas;
            if (SessionHelper.GetObjectFromJson<List<Ingredient>>(HttpContext.Session, "ListOfIngredient") != null)
            {
                listIngredient = SessionHelper.GetObjectFromJson<List<Ingredient>>(HttpContext.Session, "ListOfIngredient");
            }
            else
            {
                // Populate the data from database and set to session
                listIngredient = await GetListofIngredient();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ListOfIngredient", listIngredient);
            }
            if (SessionHelper.GetObjectFromJson<List<Pizza>>(HttpContext.Session, "ListOfPizza") != null)
            {
                Pizzas = SessionHelper.GetObjectFromJson<List<Pizza>>(HttpContext.Session, "ListOfPizza");
            }
            else
            {
                Pizzas = await GetListOfPizza();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "ListOfPizza", Pizzas);
            }
            homeViewModel.Pizzas = Pizzas;
            homeViewModel.ingredients = listIngredient;
            homeListView.Add(homeViewModel);

            return View(homeListView);
        }

        private async Task<List<Ingredient>> GetListofIngredient()
        {
            List<Ingredient> ingredient = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Ingredient/GetIngredient";
                using (var Response = await client.GetAsync(endpoint))
                {
                    // This is list view of all pizza's from database. call the API to get the result                    
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var ingredients = Response.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Pizza list
                        return JsonConvert.DeserializeObject<List<Ingredient>>(ingredients);
                    }
                    else return ingredient;
                }
            };
        }

        private async Task<List<Pizza>> GetListOfPizza()
        {
            List<Pizza> Pizzas = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Pizza/GetPizzas";
                using (var Response = await client.GetAsync(endpoint))
                {
                    // This is list view of all pizza's from database. call the API to get the result                    
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var pizzas = Response.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Pizza list
                        return JsonConvert.DeserializeObject<List<Pizza>>(pizzas);
                    }
                    else return Pizzas;
                }
            }
        }

        //[HttpGet]
        //public ViewResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Create(PizzaCreateViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string uniqeFileName = ProcessUploadedFile(model);
        //        Pizza newPizza = new Pizza()
        //        {
        //            PizzaName = model.PizzaName,
        //            PizzaType = model.PizzaType,
        //            Price = model.Price,
        //            Amount = model.Amount,
        //            ImageUrl = uniqeFileName
        //        };
        //        using (HttpClient client = new HttpClient())
        //        {
        //            StringContent content = new StringContent(JsonConvert.SerializeObject(newPizza), Encoding.UTF8, "application/json");
        //            string endpoint = apiBaseUrl + "/Pizza/AddPizza";
        //            List<Pizza> Pizzas = null;
        //            using (var Response = await client.PostAsync(endpoint, content))
        //            {
        //                // This is list view of all pizza's from database. call the API to get the result                        
        //                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
        //                {
        //                    var pizza = Response.Content.ReadAsStringAsync().Result;
        //                    //Deserializing the response recieved from web api and storing into the  list
        //                    var Pizza = JsonConvert.DeserializeObject<Pizza>(pizza);
        //                    Pizzas = await GetListOfPizza();
        //                    return View("ListPizzas",Pizzas);
        //                }
        //                else
        //                {
        //                    ModelState.Clear();
        //                    ModelState.AddModelError(string.Empty, "Failed to Save Pizza data to database");
        //                    return View();
        //                }
        //            }
        //        }
        //    }
        //    return View();
        //}
        //private string ProcessUploadedFile(PizzaCreateViewModel model)
        //{
        //    string uniqeFileName = null;
        //    if (model.Photo != null)
        //    {
        //        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
        //        uniqeFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
        //        string filepath = Path.Combine(uploadsFolder, uniqeFileName);
        //        using var fileStream = new FileStream(filepath, FileMode.Create);
        //        model.Photo.CopyTo(fileStream);
        //    }

        //    return uniqeFileName;
        //}
        //[HttpGet]
        //public async Task<ViewResult> Edit(int id)
        //{
        //    var EditPizzas = await GetListOfPizza();
        //    var editPizza = EditPizzas.Where(x => x.PizzaId == id).FirstOrDefault();

        //    PizzaEditViewModel pizzaEditViewModel = new PizzaEditViewModel()
        //    {
        //        PizzaName = editPizza.PizzaName,
        //        PizzaType = editPizza.PizzaType,
        //        Price = editPizza.Price,
        //        Amount = editPizza.Amount,
        //        ExistingPhotoPath = editPizza.ImageUrl
        //    };
        //    return View(pizzaEditViewModel);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Edit(PizzaEditViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var EditPizzas = await GetListOfPizza();
        //        var editPizza = EditPizzas.Where(x => x.PizzaId == model.Id).FirstOrDefault();
        //        editPizza.PizzaName = model.PizzaName;
        //        editPizza.PizzaType = model.PizzaType;
        //        editPizza.Price = model.Price;
        //        editPizza.Amount = model.Amount;
        //        if (model.Photo != null)
        //        {
        //            if (model.ExistingPhotoPath != null)
        //            {
        //                string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
        //                    "images", model.ExistingPhotoPath);
        //                System.IO.File.Delete(filePath);
        //            }
        //            editPizza.ImageUrl = ProcessUploadedFile(model);
        //        }
        //        //send the updated pizza object to API
        //        using (HttpClient client = new HttpClient())
        //        {
        //            StringContent content = new StringContent(JsonConvert.SerializeObject(editPizza), Encoding.UTF8, "application/json");
        //            string endpoint = apiBaseUrl + "/Pizza/UpdatePizza";

        //            using (var Response = await client.PutAsync(endpoint, content))
        //            {
        //                // This is list view of all pizza's from database. call the API to get the result
        //                List<Pizza> Pizzas;
        //                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
        //                {
        //                    var pizzas = Response.Content.ReadAsStringAsync().Result;
        //                    //Deserializing the response recieved from web api and storing into the  list
        //                    var Pizza = JsonConvert.DeserializeObject<Pizza>(pizzas);
        //                    Pizzas = await GetListOfPizza();
        //                    return RedirectToAction("listindex");
        //                }
        //                else
        //                {
        //                    ModelState.Clear();
        //                    ModelState.AddModelError(string.Empty, "Failed to Save Pizza data to database");
        //                    return View();
        //                }
        //            }
        //        }
        //    }
        //    return View();
        //}
    }
}
