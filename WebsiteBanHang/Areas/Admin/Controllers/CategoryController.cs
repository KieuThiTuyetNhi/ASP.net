
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;
using WebsiteBanHang;
using static WebsiteBanHang.Common;
using PagedList;

namespace asp_Le_Thi_Thanh_Thao.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        QuanLyBanHangEntities objQuanLyBanHangEntities = new QuanLyBanHangEntities();
        // GET: Admin/Category
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

            var lstCat = objQuanLyBanHangEntities.Categories.ToList();

            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstCat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            List<CategoryType> lstCategoryType = new List<CategoryType>();
            CategoryType objCategoryType = new CategoryType();
            objCategoryType.Id = 1;
            objCategoryType.Name = "Danh mục phổ biến";
            lstCategoryType.Add(objCategoryType);



            DataTable dtCategoryType = converter.ToDataTable(lstCategoryType);
            ViewBag.CategoryType = objCommon.ToSelectList(dtCategoryType, "Id", "Name");
        }
        [HttpGet]
        public ActionResult Create()
        {
            //lay du lieu
            this.LoadData();
            return View();
        }

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
                        //tenhinh
                        string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                        //png
                        fileName = fileName + extension;
                        //tenhinh.png
                        objCategory.Avatar = fileName;
                        objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), fileName));
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
        public ActionResult Details(int id)
        {
            var objCategory = objQuanLyBanHangEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objCategory = objQuanLyBanHangEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Delete(Product objcat)
        {
            var objCategory = objQuanLyBanHangEntities.Categories.Where(n => n.Id == objcat.Id).FirstOrDefault();

            objQuanLyBanHangEntities.Categories.Remove(objCategory);
            objQuanLyBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objCategory = objQuanLyBanHangEntities.Categories.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }
        public ActionResult Edit(int id, Product objCategory)
        {
            if (objCategory.ImageUpload != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                //tenhinh
                string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                //png
                fileName = fileName + extension;
                //tenhinh.png
                objCategory.Avatar = fileName;
            }
            objQuanLyBanHangEntities.Entry(objCategory).State = EntityState.Modified;
            objQuanLyBanHangEntities.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}