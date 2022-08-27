using PagedList;
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
        // GET: Admin/Product
        QLBanHangEntities objQuanLyBanHangEntities = new QLBanHangEntities();

       
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {

            var lstProduct = new List<Product>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstProduct = objQuanLyBanHangEntities.Products.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstProduct = objQuanLyBanHangEntities.Products.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstProduct = lstProduct.OrderByDescending(n => n.Id).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
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
            // lấy dữ diệu thương hiệu dưới db
            var lstBrand = objQuanLyBanHangEntities.Brands.ToList();
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            //convert sang select list dang value, text
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");
            //typeoid
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
            ViewBag.ProductType = objCommon.ToSelectList(dtProductType, "Id", "Name");


        }
        [HttpGet]
        public ActionResult Create()
        {
            this.LoadData();
            return View();

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
                    objQuanLyBanHangEntities.Products.Add(objProduct);
                    objQuanLyBanHangEntities.SaveChanges();


                    return RedirectToAction("Index");

                }
                catch
                {
                    return View(objProduct);
                }
            }
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            this.LoadData();
            var obproduct = objQuanLyBanHangEntities.Products.Where(n => n.Id == Id).FirstOrDefault();
            return View(obproduct);
        }
        [HttpGet]
        public ActionResult Delete(int Id)
        {

            var obproduct = objQuanLyBanHangEntities.Products.Where(n => n.Id == Id).FirstOrDefault();

            return View(obproduct);
        }
        [HttpPost]
        public ActionResult Delete(Product objproduct)
        {

            var obproduct = objQuanLyBanHangEntities.Products.Where(n => n.Id == objproduct.Id).FirstOrDefault();
            objQuanLyBanHangEntities.Products.Remove(obproduct);
            objQuanLyBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            this.LoadData();
            var product = objQuanLyBanHangEntities.Products.Where(n => n.Id == id).FirstOrDefault();
            return View(product);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product objProduct, FormCollection form)
        {
            
            if (objProduct.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objProduct.Avatar = fileName;
                objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items"), fileName));
            }
            else
            {
                objProduct.Avatar = form["oldimage"];
                objQuanLyBanHangEntities.Entry(objProduct).State = EntityState.Modified;
                objProduct.UpdatedOnUtc = DateTime.Now;
                objQuanLyBanHangEntities.SaveChanges();


            }
            objQuanLyBanHangEntities.Entry(objProduct).State = EntityState.Modified;
            objProduct.UpdatedOnUtc = DateTime.Now;
            objQuanLyBanHangEntities.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}