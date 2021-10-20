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
    public class PizzaController : ControllerBase
    {
        private readonly IRepository<Pizza> _repository;

        public PizzaController(IRepository<Pizza> repository)
        {
            this._repository = repository;
        }

        [HttpPost]
        [Route("AddPizza")]
        public Pizza AddPizza(Pizza pizza)
        {
            _repository.Add(pizza);
            _repository.Save();
            return pizza;
        }
        [Route("GetPizzas")]
        [HttpGet]
        public List<Pizza> GetPizzas()
        {
            return _repository.Search();
        }
        [Route("UpdatePizza")]
        [HttpPut]
        public Pizza UpdatePizza(Pizza pizza)
        {
            return _repository.Update(pizza);
        }
    }
}
