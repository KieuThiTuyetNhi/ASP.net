using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities objQuanLyBanHangEntities = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListCategory = objQuanLyBanHangEntities.Categories.ToList();
            objHomeModel.ListProduct = objQuanLyBanHangEntities.Products.ToList();
            return View(objHomeModel);
        }

        public ActionResult About()  
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }   

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}