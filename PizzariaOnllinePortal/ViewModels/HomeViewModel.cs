using MiddleLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaOnlinePortal.ViewModels
{
    public class HomeListViewModel
    {
        public IEnumerable<Pizza> Pizzas { get; set; }
        public IEnumerable<Ingredient> ingredients { get; set; }
        
    }
}
