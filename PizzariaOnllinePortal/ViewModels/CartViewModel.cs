using MiddleLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaOnlinePortal.ViewModels
{
    public class CartViewModel
    {
        public Pizza Pizzas { get; set; }
        public Ingredient ingredients { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
