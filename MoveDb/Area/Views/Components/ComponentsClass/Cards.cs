using Microsoft.AspNetCore.Mvc;
using MoveDb.Services.Data.Entities;
using MoveDb.Services.IServices;

namespace MoveDb.Area.Views.Components.ComponentsClass {
    public class Cards : ViewComponent {
        private readonly IHomeService _homeService;

        public Cards(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ResultResponse model)
        {
            var detailedRequest = string.Format("{0} released in {1}", model.Name, model.ReleaseDate.ToShortDateString());
            var returnModel = await _homeService.GetMovieSuggestion(detailedRequest);
            return View(returnModel);
        }
    }
}
