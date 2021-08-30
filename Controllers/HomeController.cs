using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheckYourStocks.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading;
using CheckYourStocks.Models.Registration;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace CheckYourStocks.Controllers
{
    public class HomeController : Controller
    {
        private StockContext dbStock;
        private DepositionContext dbDepositon;
        private UserContext dbUser;
        public HomeController(StockContext contextStock, DepositionContext contextDepositon, UserContext user) 
        {
            dbStock = contextStock;
            dbDepositon = contextDepositon;
            dbUser = user;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await dbUser.Users.FirstOrDefaultAsync(u => u.Name == model.Name && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Name);

                    try 
                    {
                       var timer = new CancellationTokenSource();
                       timer.CancelAfter(10000);

                       await LoadQuik();
                    } 
                    catch (TaskCanceledException) 
                    {
                        ErrorViewModel.ErrorLoadQuik = "Время ожидания установки соединения с приложением Quik превысило лимит времени!";
                    }

                    return RedirectToAction("ChartStock", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        private async Task LoadQuik() => await Task.Run(()=> new StreamLoad(dbStock,dbDepositon));
        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/Account/Create.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await dbUser.Users.FirstOrDefaultAsync(u => u.Name == model.Name);
                if (user == null)
                {                    
                    dbUser.Users.Add(new User { Name = model.Name, Password = model.Password });
                    await dbUser.SaveChangesAsync();
                    await Authenticate(model.Name); 
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }

            return View("~/Views/Account/Create.cshtml");
        }
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        public ActionResult Index()
        {
          
            return View("~/Views/Account/Login.cshtml");
        }
        public async Task<ActionResult> ViewAllStock() 
        {
            return View(await dbStock.Stocks.ToListAsync());
        }
        public ActionResult ChartStock(string NameStock)
        {
            string listStocks;
            FilterListStocks filterListStocks = new FilterListStocks();
            try 
            {
                if(string.IsNullOrEmpty(NameStock))
                    NameStock = listMoney.listHistory.First().Key;

                listStocks = Drow(ref filterListStocks, NameStock);
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(ErrorViewModel.ErrorLoadQuik))
                    ViewBag.Message = ErrorViewModel.ErrorLoadQuik;

                if (!string.IsNullOrEmpty(ErrorViewModel.ErrorGetData))
                    ViewBag.Message = ErrorViewModel.ErrorGetData;

                listStocks = Drow(ref filterListStocks);
            }

            ViewBag.listStocks = listStocks;
            return View("ChartStock", filterListStocks);
        }
        private string Drow(ref FilterListStocks listStocks,string NameStock = "Error")
        {
            var listNameStocks = new List<string>();
            IEnumerable<Deposition> deposit;
            if (NameStock.Equals("Error")) 
            {
                NameStock = dbStock.Stocks.First().Name;
                listNameStocks = dbStock.Stocks.Select(stock => stock.Name).ToList();
                List<Deposition> depositions = new List<Deposition>
                {
                    new Deposition(NameStock, 0.0)
                };
                deposit = depositions.Where(deposit => deposit.secCode.Equals(NameStock));
            }
            else
            {
                listNameStocks = listMoney.listHistory.Select(stock => stock.Key).ToList();
                deposit = listMoney.listAweragePrice.Where(deposit => deposit.secCode.Equals(NameStock));
            }

            IQueryable<Stock> stocks = dbStock.Stocks;
            stocks = stocks.Where(stock => stock.Name.Equals(NameStock));
            listStocks = new FilterListStocks
            {
                Stocks = stocks,
                Deposit = deposit.First(),
                NameStocks = new SelectList(listNameStocks, "secCode"),
                Name = NameStock
            };
            return JsonConvert.SerializeObject(listStocks);
        }
    }
}
