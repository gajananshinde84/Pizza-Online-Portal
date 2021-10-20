 
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaOnlinePortal.ViewModels
{
    public class CreateIngredientViewModel
    {
        [Required]
        public string IngredientName { get; set; }
       
        [Required]
        public double Price { get; set; }        
        public IFormFile Photo { get; set; }
    }
}
