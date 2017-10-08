using System.Linq;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Web.Extensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Ayatta.Web.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly DefaultStorage defaultStorage;
        public MenuViewComponent(DefaultStorage defaultStorage)
        {
            this.defaultStorage = defaultStorage;
        }

        public async Task<IViewComponentResult> InvokeAsync(int moduleId)
        {
            var identity = base.UserClaimsPrincipal.Identity();
            var ms = defaultStorage.UserRoleFuncList(identity.Id).ToModuleList();
            var dic = ms.ToDictionary(x => x.Id, x => x.Pid);
            var hs = new HashSet<int>();
            hs.Add(moduleId);
            var i = moduleId;
            while (i > 0)
            {
                i = dic[i];
                if (i > 0)
                {
                    hs.Add(i);
                }
            }
            var model = new Magic<IList<RBAC.Module>, HashSet<int>>(ms, hs);
            return View(model);
        }
    }
}