using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;
using WebsiteBanHang.Models;
namespace WebsiteBanHang.Controllers
{
    public class UserController : Controller
    {
        QLBanHangEntities objQuanLyBanHangEntities = new QLBanHangEntities();
        // GET: Category
        public ActionResult Index()
        {
            var lstUser = objQuanLyBanHangEntities.Users.ToList();
            return View(lstUser);
        }

        public ActionResult ProductCategory(int Id)
        {
            HomeModel objhomemodel = new HomeModel();
            objhomemodel.ListUser = objQuanLyBanHangEntities.Users.Where(n => n.UserId==Id).ToList();
            objhomemodel.ListUser =objQuanLyBanHangEntities.Users.ToList();


            return View(objhomemodel);
        }
    }
}