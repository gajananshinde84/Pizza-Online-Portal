using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiddleLayer;
using IRepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaPortalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IRepository<Ingredient> _repository;

        public IngredientController(IRepository<Ingredient> repository)
        {
            this._repository = repository;
        }

        [HttpPost]
        [Route("AddIngredient")]
        public Ingredient AddIngredient(Ingredient ingredient)
        {
            _repository.Add(ingredient);
            _repository.Save();
            return ingredient;
        }
        [Route("GetIngredient")]
        [HttpGet]
        public List<Ingredient> GetIngredient()
        {
            return _repository.Search();
        }
        [Route("UpdateIngredient")]
        [HttpPut]
        public Ingredient UpdateIngredient(Ingredient ingredient)
        {
            return _repository.Update(ingredient);
        }
    }
}
