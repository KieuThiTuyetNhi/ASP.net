using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteBanHang.Context;

namespace WebsiteBanHang.Models
{
    public class ProductDao
    {
        QLBanHangEntities qLBanHangEntities = new QLBanHangEntities();
        public List<Product> SearchByKey(string key)
        {
            return qLBanHangEntities.Products.SqlQuery("Select * From Product Where Name like N'%"+key+"%'").ToList();
        }

    }
}