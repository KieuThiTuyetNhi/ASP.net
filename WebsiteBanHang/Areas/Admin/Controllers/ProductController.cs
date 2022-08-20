using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;
using static WebsiteBanHang.Common;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        QuanLyBanHangEntities objQuanLyBanHangEntities = new QuanLyBanHangEntities();
        private object dbobj;


        // GET: Admin/Product
        public ActionResult Index()
        {
            var lstProduct = objQuanLyBanHangEntities.Category.ToList();

            return View(lstProduct);
        }
        public ActionResult Details (int Id)
        {
            var objProduct = objQuanLyBanHangEntities.Category. Where(n=>n.Id== Id).FirstOrDefault();
            return View(objProduct);
        }
        void LoadData()
        {
            Common objCommon = new Common();
            // lấy dữ liệu dưới db
            var lstCat = objQuanLyBanHangEntities.Categories.ToList();
            //convert  sang select list dạng value,text
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            //// lấy dữ diệu thương hiệu dưới db
            var lstBrand = objQuanLyBanHangEntities.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            //convert sang select list dang value, text
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            //typeid
            List<ProductType> lstProductType = new List<ProductType>();
            ProductType objProductType = new ProductType();
            objProductType.Id = 01;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.Id = 02;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);

            DataTable dtProductType = converter.ToDataTable(lstProductType);
            //Convert sang select list dang values, text
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");


        }
        [HttpGet]
        public ActionResult Create()
        {
            this.LoadData();
            return View();

        }

        private dynamic ToSelectList(DataTable dtCategory, string v1, string v2)
        {
            throw new NotImplementedException();
        }

        // sua loi sckeditorjs
        [ValidateInput(false)]
        //end
        [HttpPost]
        public ActionResult Create(Product objProduct)
        {

            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    if (objProduct.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                        string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                        fileName = fileName + extension;
                        objProduct.Avatar = fileName;
                        objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items"), fileName));
                    }
                    objProduct.CreatedOnUtc = DateTime.Now;
                    objQuanLyBanHangEntities.Category.Add(objProduct);
                    objQuanLyBanHangEntities.SaveChanges();


                    return RedirectToAction("Index");

                }
                catch
                {
                    return View();
                }
            }
            return View(objProduct);
        }
        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var objProduct = objQuanLyBanHangEntities.Category.Where(n => n.Id== Id).FirstOrDefault();
            return View(objProduct);
        }
        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = objQuanLyBanHangEntities.Category.Where(n => n.Id== objPro.Id).FirstOrDefault();
            objQuanLyBanHangEntities.Category.Remove(objProduct);
            objQuanLyBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            this.LoadData();
            var product = objQuanLyBanHangEntities.Category.Where(n => n.Id == id).FirstOrDefault();
            return View(product);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(Product objProduct)
        {

            if (objProduct.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objProduct.Avatar = fileName;
                objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items"), fileName));
            }
            objQuanLyBanHangEntities.Entry(objProduct).State = EntityState.Modified;
            objProduct.UpdatedOnUtc = DateTime.Now;
            objQuanLyBanHangEntities.SaveChanges();


            return RedirectToAction("Index");
        }


    }
}