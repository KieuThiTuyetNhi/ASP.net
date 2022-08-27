using WebsiteBanHang.Controllers;
using System.Linq;
using System.Web.Mvc;
using WebsiteBanHang.Context;

namespace WebsiteBanHang.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        QLBanHangEntities objQuanLyBanHangEntities = new QLBanHangEntities();
        public ActionResult Detail(int Id)
        {

            var objProduct = objQuanLyBanHangEntities.Products.Where(n => n.Id == Id).FirstOrDefault();

            return View(objProduct);
        }
      
    }
}