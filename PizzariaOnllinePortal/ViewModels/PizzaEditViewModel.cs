using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzariaOnlinePortal.ViewModels
{
    public class PizzaEditViewModel : PizzaCreateViewModel
    {
        public int Id { get; set; }
        public string ExistingPhotoPath{ get; set; }
    }
}
