using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebsiteBanHang.Context;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Context
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    namespace Example01.Models
    {
        public class PartialMedataType
        {
            [MetadataType(typeof(ProductMasterData))]
            public partial class Product
            {
                [NotMapped]

                public System.Web.HttpPostedFileBase ImageUpload { get; set; }
                public int Id { get; internal set; }
            }

            [MetadataType(typeof(ProductMasterData))]
            public partial class Category
            {
                [NotMapped]

                public System.Web.HttpPostedFileBase ImageUpload { get; set; }
            }
            public partial class Brand
            {
                [NotMapped]

                public System.Web.HttpPostedFileBase ImageUpload { get; set; }
            }
        }
    }
}