using Microsoft.AspNetCore.Mvc;
using MiddleLayer;
using PizzariaOnlinePortal.Utility;
using PizzariaOnlinePortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaOnlinePortal.Controllers
{
    public class CartController : Controller
    {

        public ViewResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");
            //ViewBag.cart = cart;
            if (cart != null)
            {
                double PizzaTot = 0;
                double IngredientTot = 0;
                PizzaTot = cart.Sum(item => item.Pizzas != null ? item.Pizzas.Price * item.Quantity : 0);
                IngredientTot = cart.Sum(item => item.ingredients != null ? item.ingredients.Price * item.Quantity : 0);
                ViewBag.total = PizzaTot + IngredientTot;
            }
            else
            {
                cart = new List<CartViewModel>();
                ViewBag.total = 0;
            }
            return View(cart);
        }

        public IActionResult Buy(int id)
        {
            List<Pizza> listPizza = null;
            List<Ingredient> listIngredient = null;
            List<CartViewModel> cart = new List<CartViewModel>();
            if (SessionHelper.GetObjectFromJson<List<Pizza>>(HttpContext.Session, "ListOfPizza") != null)
            {
                listPizza = SessionHelper.GetObjectFromJson<List<Pizza>>(HttpContext.Session, "ListOfPizza");
            }
            if (SessionHelper.GetObjectFromJson<List<Ingredient>>(HttpContext.Session, "ListOfIngredient") != null)
            {
                listIngredient = SessionHelper.GetObjectFromJson<List<Ingredient>>(HttpContext.Session, "ListOfIngredient");
            }
            if (listPizza.Count > 0 && listIngredient.Count > 0)
            {
                if (SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart") == null)
                {
                    CartViewModel cartViewModel = new CartViewModel()
                    {
                        Pizzas = listPizza.Where(x => x.PizzaId == id).FirstOrDefault(),
                        Quantity = 1
                    };
                    cart.Add(cartViewModel);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                else
                {
                    cart = SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");
                    int indexForPizza = isExist(id);
                    // int indexForIngredient = isExistForIngredient(id);
                    if (indexForPizza != -1)
                    {
                        cart[indexForPizza].Quantity++;
                    }
                    else
                    {
                        CartViewModel cartViewModel = new CartViewModel()
                        {
                            Pizzas = listPizza.Where(x => x.PizzaId == id).FirstOrDefault(),
                            Quantity = 1
                        };
                        cart.Add(cartViewModel);
                    }
                    //int indexForIngredient = isExistForIngredient(id);
                    //if (indexForIngredient != -1)
                    //{
                    //    cart[indexForIngredient].Quantity++;
                    //}
                    //else
                    //{                         
                    //    cart.Add(new CartViewModel { ingredients = listIngredient.Where(x => x.IngredientId == id).FirstOrDefault(), Quantity = 1 });
                    //}
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
            }
            return RedirectToAction("index");
        }
        public IActionResult BuyIngredient(int id)
        {

            List<Ingredient> listIngredient = null;
            List<CartViewModel> cart = new List<CartViewModel>();

            if (SessionHelper.GetObjectFromJson<List<Ingredient>>(HttpContext.Session, "ListOfIngredient") != null)
            {
                listIngredient = SessionHelper.GetObjectFromJson<List<Ingredient>>(HttpContext.Session, "ListOfIngredient");
            }
            if (listIngredient.Count > 0)
            {
                if (SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart") == null)
                {
                    CartViewModel cartViewModel = new CartViewModel()
                    {
                        ingredients = listIngredient.Where(x => x.IngredientId == id).FirstOrDefault(),
                        Quantity = 1
                    };
                    cart.Add(cartViewModel);
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
                else
                {
                    cart = SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");
                    int indexForIngredient = isExistForIngredient(id);
                    if (indexForIngredient != -1)
                    {
                        cart[indexForIngredient].Quantity++;
                    }
                    else
                    {
                        CartViewModel cartViewModel = new CartViewModel()
                        {
                            ingredients = listIngredient.Where(x => x.IngredientId == id).FirstOrDefault(),
                            Quantity = 1
                        };
                        cart.Add(cartViewModel);
                    }
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
                }
            }
            return RedirectToAction("index");
        }

        public IActionResult Remove(int id,string Type)
        {
            List<CartViewModel> cart = SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");
            int index;
            if (Type == "PIZZA")
            {
                index = isExist(id);
            }
            else
            {
                index = isExistForIngredient(id);
            }                
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<CartViewModel> cart = SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Pizzas.PizzaId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
        private int isExistForIngredient(int id)
        {
            List<CartViewModel> cart = SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].ingredients != null && cart[i].ingredients.IngredientId.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Checkout()
        {
            if (ModelState.IsValid)
            {
                if (SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart") != null)
                {
                    var cartViewModel = SessionHelper.GetObjectFromJson<List<CartViewModel>>(HttpContext.Session, "cart");
                }
            }
        }
    }
}
