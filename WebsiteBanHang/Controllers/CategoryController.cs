using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;
using WebsiteBanHang.Models;
namespace WebsiteBanHang.Controllers
{
    public class CategoryController : Controller
    {
        QuanLyBanHangEntities objQuanLyBanHangEntities = new QuanLyBanHangEntities();
        // GET: Category
        public ActionResult Index()
        {
            var lstCategory = objQuanLyBanHangEntities.Categories.ToList();
            return View(lstCategory);
        }
        
        public ActionResult ProductCategory (int Id)
        {
            HomeModel objhomemodel = new HomeModel();
            objhomemodel.ListProduct = objQuanLyBanHangEntities.Products.Where(n => n.CategoryId==Id).ToList();
            objhomemodel.ListCategory =objQuanLyBanHangEntities.Categories.ToList();

           
            return View(objhomemodel);
        }
    }
}