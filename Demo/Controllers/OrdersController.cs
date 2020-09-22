using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Demo.Models;
using Webdiyer.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Demo.Controllers
{
    public class OrdersController : Controller
    {
        IHostingEnvironment Env;
        int pageSize = 5; //default page size

        public OrdersController(IHostingEnvironment env)
        {
            Env = env;
        }

        public IActionResult Index(int pageIndex = 1)
        {
            var model = GetPagedOrders(pageIndex, pageSize);
            return View(model);
        }

        public IActionResult Ajax(int pageIndex = 1)
        {
            var model = GetPagedOrders(pageIndex, pageSize);
            string xrh = Request.Headers["X-Requested-With"];
            if (!string.IsNullOrEmpty(xrh) && xrh.Equals("XMLHttpRequest", System.StringComparison.OrdinalIgnoreCase))
            {
                return PartialView("_OrderList", model);
            }
            return View(model);
        }

        public IActionResult Bootstrap(int id = 1)
        {
            var model = GetPagedOrders(id, pageSize);
            return View(model);
        }

        public IActionResult PageIndexBox(int id = 1)
        {
            var model = GetPagedOrders(id, pageSize);
            return View(model);
        }

        public IActionResult Search(string companyName,int id = 1)
        {
            var model = GetPagedOrders(id, pageSize, companyName);
            return View(model);
        }

        public IActionResult AjaxSearch(string companyName,int id = 1)
        {
            var model = GetPagedOrders(id, pageSize, companyName);
            if(Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_PagedData", model);
            }
            return View(model);
        }

        [NonAction]
        private  IPagedList<Order> GetPagedOrders(int pageIndex, int pageSize,string companyName=null)
        {
            var path = Path.Combine(Env.WebRootPath, "orders.json");
            var ods = Newtonsoft.Json.JsonConvert.DeserializeObject<Order[]>(System.IO.File.ReadAllText(path));
            if (!string.IsNullOrWhiteSpace(companyName))
            {
                return ods.Where(o=>o.CompanyName.Contains(companyName)).OrderBy(o => o.OrderId).ToPagedList(pageIndex, pageSize);
            }
            return ods.OrderBy(o => o.OrderId).ToPagedList(pageIndex, pageSize);
        }


        
    }
}
