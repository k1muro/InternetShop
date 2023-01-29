using InternetShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;

namespace InternetShop.Controllers
{
    public class HomeController : Controller
    {
        public ShopContext db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ShopContext db, ILogger<HomeController> logger)
        {
            this.db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.products = db.Products.AsNoTracking().ToList();
            string UserLoggedIn = Request.Cookies["UserLoggedIn"];
            User user = null;
            if (UserLoggedIn != null && UserLoggedIn != "")
            {
                user = new User();
                user = JsonSerializer.Deserialize<User>(UserLoggedIn);
                ViewBag.user = user;
            }



            return View();
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
        [HttpPost]
        public async Task<IActionResult> checking(string login, string password)
        {
            User user = db.Users.Where(o => o.Login == login && o.Pass == password).AsNoTracking().FirstOrDefault();
            if (user != null)
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddMinutes(30);
                options.IsEssential = true;
                options.Path = "/";

                string str = JsonSerializer.Serialize(user);

                HttpContext.Response.Cookies.Append("UserLoggedIn", str, options);
                return Redirect("Index");
                //return RedirectToAction("Index", "Main");
            }
            else return View("Registration");
        }
        public IActionResult Registration()
        {

            return View();
        }
        public IActionResult CRUD()
        {
            string UserLoggedIn = Request.Cookies["UserLoggedIn"];
            User user = null;
            user = JsonSerializer.Deserialize<User>(UserLoggedIn);
            if (user.RoleId == 1)
            {
                ViewBag.Products = db.Products.AsNoTracking().ToList();
                ViewBag.Category = db.Categories.AsNoTracking().ToList();
                return View();
            }
            else
            {
                return Redirect("ErrorPage");
            }
        }
        async public Task<IActionResult> AddPage()
        {
            string UserLoggedIn = Request.Cookies["UserLoggedIn"];
            User user = null;
            user = JsonSerializer.Deserialize<User>(UserLoggedIn);
            if (user.Id == 1)
            {
                ViewBag.Category = db.Categories.AsNoTracking().ToList();
                return View();
            }
            else
            {
                return Redirect("ErrorPage");
            }
        }

        [HttpPost]
        async public Task<IActionResult> Save(string ProductName, int Amount,string Info,int Price,int CategoryId)
        {
            Product prod = new Product() { ProductName = ProductName, Amount = Amount, Info = Info, Price=Price,CategoryId = CategoryId };
            db.Products.Add(prod);
            await db.SaveChangesAsync();

            return Redirect("CRUD");
        }

        [HttpPost]
        async public Task<IActionResult> EditPage(int id)
        {
            ViewBag.Product = db.Products.Where(o => o.Id == id).AsNoTracking().FirstOrDefault();
            ViewBag.Category = db.Categories.AsNoTracking().ToList();
            return View();
        }
       
        public IActionResult InfoPage(int id)
        {
            ViewBag.Product = db.Products.Where(o => o.Id == id).AsNoTracking().FirstOrDefault();
            ViewBag.Category = db.Categories.AsNoTracking().ToList();
            return View();
        }
        

        [HttpPost]
        async public Task<IActionResult> SaveEdit(int Id, string ProductName,int Amount,string Info,int Price, int CategoryId)
        {
            Product product = db.Products.Where(o => o.Id == Id).AsNoTracking().FirstOrDefault();
            product.ProductName = ProductName;
            product.Amount = Amount;
            product.Info = Info;
            product.Price = Price;
            product.CategoryId = CategoryId;
            db.Update(product);
            await db.SaveChangesAsync();

            return Redirect("CRUD");
        }

        [HttpPost]
        async public Task<IActionResult> Delete(int id)
        {
            Product delete = db.Products.Where(o => o.Id == id).AsNoTracking().FirstOrDefault();
            db.Remove(delete);
            await db.SaveChangesAsync();
            return Redirect("CRUD");
           
        }
    }
}