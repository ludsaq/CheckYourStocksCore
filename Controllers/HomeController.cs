using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CheckYourStocks.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckYourStocks.Controllers
{
    public class HomeController : Controller
    {
        private StockContext db;
        public HomeController(StockContext context) 
        {
            db = context;
        }

        public async Task<ActionResult> Index() 
        {
            return View(await db.Stocks.ToListAsync());
        }

        public IActionResult Create() 
        {
            return View();
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
