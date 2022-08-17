using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;

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
            var LstProduct = objQuanLyBanHangEntities.Products.Where(n => n. CategoryId==Id).ToList();
            return View(LstProduct);
        }
    }
}