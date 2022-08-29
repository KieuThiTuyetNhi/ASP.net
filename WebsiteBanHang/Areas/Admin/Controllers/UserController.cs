using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstUser = new List<User>();
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
                lstUser = objQuanLyBanHangEntities.Users.Where(n => n.FirstName.Contains(SearchString)).ToList();
            }
            else
            {
                lstUser = objQuanLyBanHangEntities.Users.ToList();
            }
            ViewBag.CurrentFilter = SearchString;
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            lstUser = lstUser.OrderByDescending(n => n.Id).ToList();
            return View(lstUser.ToPagedList(pageNumber, pageSize));
        }

      

        [HttpGet]
        public ActionResult Create()
        {
          
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = objQuanLyBanHangEntities.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _user.IsAdmin = false; //IsAdmin = false = người dùng
                    objQuanLyBanHangEntities.Configuration.ValidateOnSaveEnabled = false;
                    // add user
                    objQuanLyBanHangEntities.Users.Add(_user);
                    //lưu thông tin lại
                    objQuanLyBanHangEntities.SaveChanges();
                    // trả về trang chủ
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại";


                    return View();
                }

            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var User = objQuanLyBanHangEntities.Users.Where(n => n.Id == id).FirstOrDefault();
            return View(User);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var User = objQuanLyBanHangEntities.Users.Where(n => n.Id == id).FirstOrDefault();
            return View(User);
        }
        [HttpPost]
        public ActionResult Delete(User objuser)
        {
            var objUser = objQuanLyBanHangEntities.Users.Where(n => n.Id == objuser.Id).FirstOrDefault();
            objQuanLyBanHangEntities.Users.Remove(objUser);
            objQuanLyBanHangEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
          

            var User = objQuanLyBanHangEntities.Users.Where(n => n.Id == id).FirstOrDefault();
            return View(User);
        }
        // mã hóa pass word
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, User objUser)
        {
            objQuanLyBanHangEntities.Entry(objUser).State = EntityState.Modified;
            objQuanLyBanHangEntities.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}