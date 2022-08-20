using WebsiteBanHang.Controllers;
using System.Linq;
using System.Web.Mvc;
using WebsiteBanHang.Context;

namespace WebsiteBanHang.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        QuanLyBanHangEntities objQuanLyBanHangEntities = new QuanLyBanHangEntities();
        public ActionResult Detail(int Id)
        {

            var objProduct = objQuanLyBanHangEntities.Category.Where(n => n.Id == Id).FirstOrDefault();

            return View(objProduct);
        }
        public ActionResult ProInCat(int ID)
        {
            var get = objQuanLyBanHangEntities.Category.Where(s => s.CategoryId == ID).ToList();
            return View(get);
        }
    }
}