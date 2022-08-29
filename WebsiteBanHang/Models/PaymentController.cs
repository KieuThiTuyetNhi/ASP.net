using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;

namespace WebsiteBanHang.Models
{
        public class PaymentController : Controller
        {
            QLBanHangEntities objQuanLyBanHangEntities = new QLBanHangEntities();
            // GET: Payment
            public ActionResult Index()
            {
                if (Session["idUser"]==null)
                {
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    // Lấy thông từ giỏ hàng từ biến session
                    var lstCart = (List<CartModel>)Session["cart"];
                    // gán dữ liệu cho Order
                    Order objOrder = new Order();
                    objOrder.Name = "Donhang - " + DateTime.Now.ToString("yyyyMMddHHmmss");
                    objOrder.UserId = int.Parse(Session["idUser"].ToString());
                    objOrder.CreatedOnUtc = DateTime.Now;
                    objOrder.Status = 1;
                    objQuanLyBanHangEntities.Orders.Add(objOrder);
                    //Lưu thông tin  dữ liệu vào bảng order
                    objQuanLyBanHangEntities.SaveChanges();

                    //Lấy OrderId vừa mới tạo  để lưu vào bảng OrderDetail
                    int intOrderId = objOrder.Id;

                    List<OrderDetail> lstOrderDetail = new List<OrderDetail>();

                    foreach (var item in lstCart)
                    {
                        OrderDetail obj = new OrderDetail();
                        obj.Quantity = item.Quantity;
                        obj.OrderId = intOrderId;
                        obj.ProductId = item.Product.Id;
                        lstOrderDetail.Add(obj);
                    }
                    objQuanLyBanHangEntities.OrderDetails.AddRange(lstOrderDetail);
                    objQuanLyBanHangEntities.SaveChanges();
                }
                return View();
            }
        }
    }
