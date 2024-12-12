using Microsoft.AspNetCore.Mvc;

namespace MoveDb.Area.Views.Components.ComponentsClass
{
    public class Sidebar : ViewComponent
    {
        public Sidebar()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
