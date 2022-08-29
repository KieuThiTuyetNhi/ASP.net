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
    public class BrandController : Controller
    {
        QLBanHangEntities objQuanLyBanHangEntities = new QLBanHangEntities();
        private object lstBrannd;

        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {

            var lstBrand = new List<Brand>();
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
                lstBrand = objQuanLyBanHangEntities.Brands.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstBrand = objQuanLyBanHangEntities.Brands.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstBrannd = lstBrand.OrderByDescending(n => n.Id).ToList();
            return View(lstBrand.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(Brand objBrand)
        {
            this.LoadData();

            if (ModelState.IsValid)
            {
                try
                {
                    if (objBrand.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                        string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                        fileName = fileName + extension;
                        objBrand.Avatar = fileName;
                        objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items"), fileName));
                    }
                    objBrand.CreatedOnUtc = DateTime.Now;
                    objQuanLyBanHangEntities.Brands.Add(objBrand);
                    objQuanLyBanHangEntities.SaveChanges();


                    return RedirectToAction("Index");

                }
                catch
                {
                    return View(objBrand);
                }
            }
            return View(objBrand);
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            this.LoadData();
            var objBrand = objQuanLyBanHangEntities.Brands.Where(n => n.Id == Id).FirstOrDefault();
            return View(objBrand);
        }
        [HttpGet]
        public ActionResult Delete(int Id)
        {

            var objBrand = objQuanLyBanHangEntities.Brands.Where(n => n.Id == Id).FirstOrDefault();

            return View(objBrand);
        }
        [HttpPost]
        public ActionResult Delete(Brand objBr)
        {

            var objBrand= objQuanLyBanHangEntities.Brands.Where(n => n.Id == objBr.Id).FirstOrDefault();
            objQuanLyBanHangEntities.Brands.Remove(objBrand);
            objQuanLyBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edits(int id)
        {
            this.LoadData();
            Brand Brand = objQuanLyBanHangEntities.Brands.Where(n => n.Id == id).FirstOrDefault();
            return View(Brand);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edits(Brand objBrand, FormCollection form)
        {
            
            if (objBrand.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objBrand.Avatar = fileName;
                objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items"), fileName));
            }
            else
            {
                objBrand.Avatar = form["oldimage"];
                objQuanLyBanHangEntities.Entry(objBrand).State = EntityState.Modified;
                objBrand.UpdateOnUtc = DateTime.Now;
                objQuanLyBanHangEntities.SaveChanges();


            }
            objQuanLyBanHangEntities.Entry(objBrand).State = EntityState.Modified;
            objBrand.UpdateOnUtc = DateTime.Now;
            objQuanLyBanHangEntities.SaveChanges();


            return RedirectToAction("Index");
        }
    }
}