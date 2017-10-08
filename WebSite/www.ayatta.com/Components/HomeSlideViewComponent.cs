using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Ayatta.Storage;
using Ayatta.Domain;

namespace Ayatta.Web.Components
{
    public class HomeSlideViewComponent : ViewComponent
    {
        private readonly DefaultStorage defaultStorage;
        public HomeSlideViewComponent(DefaultStorage defaultStorage)
        {
            this.defaultStorage = defaultStorage;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            
            var items = defaultStorage.SlideGet(id, true, true);
            return View(items);
        }

       

    }
}
