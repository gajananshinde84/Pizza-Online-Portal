 
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaOnlinePortal.ViewModels
{
    public class PizzaCreateViewModel
    {
        [Required]
        public string PizzaName { get; set; }
        [Required]        
        public string PizzaType { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double Amount { get; set; }
        public IFormFile Photo { get; set; }
    }
}
