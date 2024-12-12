using Microsoft.AspNetCore.Mvc;
using MoveDb.Area.Models;
using MoveDb.Services.Data.Entities;
using MoveDb.Services.IServices;
using RestSharp;
using System.Diagnostics;

namespace MoveDb.Area.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(
            IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var getMv = await _homeService.Get();

			return View(getMv);
        }

        [HttpGet("{id:int}/watch")]
        public async Task<IActionResult> Watch(int id)
        {
            var movieDetailResult = await _homeService.GetMovieDetails(id);

            return View(movieDetailResult);
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
