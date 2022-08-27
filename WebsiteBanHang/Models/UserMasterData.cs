using System;
using System.ComponentModel.DataAnnotations;

namespace WebsiteBanHang.Context
{
    internal class UserMasterData
    {
        public int Id { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Name { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Avatar { get; set; }
        [Display(Name = "Tên danh mục")]
        public Nullable<int> CategoryId { get; set; }
        [Display(Name = "Loại sản phẩm")]
        public Nullable<int> TypeId { get; set; }
        [Display(Name = "Tên thương hiệu")]
        public Nullable<int> BrandId { get; set; }
        [Display(Name = "Mô tả ngắn")]
        public string ShortDes { get; set; }
        [Display(Name = "Mô tả đầy đủ")]
        public string FullDescription { get; set; }
        [Display(Name = "Giá")]
        [Required(ErrorMessage = "Giá không được để trống")]
        public Nullable<double> Price { get; set; }
        [Display(Name = "Giá khuyến mãi")]
        public Nullable<double> PriceDiscount { get; set; }
        public string Slug { get; set; }
        public Nullable<bool> Deleted { get; set; }
        [Display(Name = "Hiển thị trang chủ")]
        public Nullable<bool> ShowOnHomePage { get; set; }

        public Nullable<int> DisplayOrder { get; set; }
        [Display(Name = "Ngày tạo")]

        public Nullable<System.DateTime> CreatedOnUtc { get; set; }
        [Display(Name = "Ngày cập nhật")]

        public Nullable<System.DateTime> UpdatedOnUtc { get; set; }


    }
}
}