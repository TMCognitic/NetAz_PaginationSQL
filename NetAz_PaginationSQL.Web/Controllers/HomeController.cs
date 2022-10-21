using Microsoft.AspNetCore.Mvc;
using NetAz_PaginationSQL.Web.Models;
using NetAz_PaginationSQL.Web.Models.Queries;
using System.Diagnostics;

namespace NetAz_PaginationSQL.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GetDataQueryHandler _getDataQueryHandler;

        public HomeController(ILogger<HomeController> logger, GetDataQueryHandler getDataQueryHandler)
        {
            _logger = logger;
            _getDataQueryHandler = getDataQueryHandler;
        }

        public IActionResult Index(int? id)
        {
            int currentPage = (id.HasValue) ? id.Value : 1;
            GetDataQuery getDataQuery = new GetDataQuery(currentPage);
            IEnumerable<DataDto> datas = _getDataQueryHandler.Execute(getDataQuery);
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageCount = getDataQuery.PageCount;

            return View(datas);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}