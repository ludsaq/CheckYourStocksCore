using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheckYourStocks.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CheckYourStocks.Controllers
{
    public class HomeController : Controller
    {
        private StockContext db;
        public HomeController(StockContext context) 
        {
            db = context;
        }

        public ActionResult Index()
        {
            return View("StartPage");
        }
        public IActionResult Menu()
        {
            return View();
        }

        public async Task<ActionResult> ViewAllStock() 
        {
            return View(await db.Stocks.ToListAsync());
        }

        public IActionResult Create() 
        {
            return View();
        }

        public ActionResult ChartStock(string NameStock)
        {
            IQueryable<Stock> stocks = db.Stocks;
            var listNameStocks = stocks.Select(stock => stock.Name).ToList();

            if (String.IsNullOrEmpty(NameStock)) 
            {
                NameStock = stocks.First().Name;
            }

            stocks = stocks.Where(stock => stock.Name.Equals(NameStock));

            FilterListStocks listStocks = new FilterListStocks
            {
                Stocks = stocks,
                NameStocks = new SelectList(listNameStocks,"Name"),
                Name = NameStock
            };

            string json = JsonConvert.SerializeObject(listStocks);
            ViewBag.listStocks = json;
            return View(listStocks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Stock stock) 
        {
            db.Stocks.Add(stock);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
