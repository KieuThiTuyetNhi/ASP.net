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
    public class CategoryController : Controller
    {
        // GET: Admin/Product
        QLBanHangEntities objQuanLyBanHangEntities = new QLBanHangEntities();


        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {

            var lstCategory = new List<Category>();
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
                lstCategory = objQuanLyBanHangEntities.Categories.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstCategory = objQuanLyBanHangEntities.Categories.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstCategory = lstCategory.OrderByDescending(n => n.Id).ToList();
            return View(lstCategory.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(Category objCategory)
        {
            this.LoadData();

            if (ModelState.IsValid)
            {
                try
                {
                    if (objCategory.ImageUpload != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                        string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                        fileName = fileName + extension;
                        objCategory.Avatar = fileName;
                        objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items"), fileName));
                    }
                    objCategory.CreatedOnUtc = DateTime.Now;
                    objQuanLyBanHangEntities.Categories.Add(objCategory);
                    objQuanLyBanHangEntities.SaveChanges();


                    return RedirectToAction("Index");

                }
                catch
                {
                    return View(objCategory);
                }
            }
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            this.LoadData();
            var objCategory = objQuanLyBanHangEntities.Categories.Where(n => n.Id == Id).FirstOrDefault();
            return View(objCategory);
        }
        [HttpGet]
        public ActionResult Delete(int Id)
        {

            var objCategory = objQuanLyBanHangEntities.Categories.Where(n => n.Id == Id).FirstOrDefault();

            return View(objCategory);
        }
        [HttpPost]
        public ActionResult Delete(Category objc)
        {

            var objCategory = objQuanLyBanHangEntities.Categories.Where(n => n.Id == objc.Id).FirstOrDefault();
            objQuanLyBanHangEntities.Categories.Remove(objCategory);
            objQuanLyBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            //this.loadData();
            var objCate = objQuanLyBanHangEntities.Categories.Where(n => n.Id == Id).FirstOrDefault();
            return View(objCate);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            //this.loadData();
            if (category.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(category.ImageUpload.FileName);
                //tenhinh
                string extension = Path.GetExtension(category.ImageUpload.FileName);
                //png
                fileName = fileName + extension;
                //tenhinh.png
                category.Avatar = fileName;
                category.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/category"), fileName));
            }

            category.UpdatedOnUtc = DateTime.Now;
            objQuanLyBanHangEntities.Entry(category).State = EntityState.Modified;
            objQuanLyBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
    }

}