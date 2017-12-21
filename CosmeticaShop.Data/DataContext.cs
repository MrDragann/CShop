using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CosmeticaShop.Data.Models;

namespace CosmeticaShop.Data
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") { }

        /// <summary>
        /// Таблица пользователей
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Таблица ролей
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// Таблица брендов
        /// </summary>
        public DbSet<Brand> Brands { get; set; }

        /// <summary>
        /// Таблица категорий
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Таблица товаров
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Таблица заказов
        /// </summary>
        public DbSet<OrderHeader> OrderHeaders { get; set; }

        /// <summary>
        /// Таблица товаров заказов
        /// </summary>
        public DbSet<OrderProduct> OrderProducts { get; set; }

        /// <summary>
        /// Таблица списка желаемого
        /// </summary>
        public DbSet<WishList> WishLists { get; set; }

        /// <summary>
        /// Таблица страниц сайта
        /// </summary>
        public DbSet<SitePage> SitePages { get; set; }

        /// <summary>
        /// Таблица слайдов
        /// </summary>
        public DbSet<Slider> Sliders { get; set; }

        /// <summary>
        /// Таблица тегов товаров
        /// </summary>
        public DbSet<ProductTag> ProductTags { get; set; }

        /// <summary>
        /// Таблица купонов
        /// </summary>
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            #region User
            
            mb.Entity<User>().Property(_ => _.Email).HasMaxLength(128);
            mb.Entity<User>().Property(_ => _.PasswordHash).HasMaxLength(128);
            mb.Entity<User>().Property(_ => _.FirstName).HasMaxLength(128);
            mb.Entity<User>().Property(_ => _.LastName).HasMaxLength(128);
            mb.Entity<User>().Property(_ => _.PhotoUrl).HasMaxLength(128);

            mb.Entity<User>().Property(_ => _.Email).IsRequired();
            mb.Entity<User>().Property(_ => _.PasswordHash).IsRequired();

            // связь 1к1 с адресом
            mb.Entity<User>().HasRequired(x => x.UserAddress).WithRequiredPrincipal(ad => ad.User);

            #endregion

            #region UserAddress

            mb.Entity<UserAddress>().HasKey(x => x.UserId);
            
            #endregion

            #region Role

            mb.Entity<Role>().HasKey(_=>_.Id);
            mb.Entity<Role>().Property(_ => _.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            mb.Entity<Role>().Property(_ => _.Name).HasMaxLength(128);
            mb.Entity<Role>().Property(_ => _.Name).IsRequired();

            #endregion

            #region Product
            
            mb.Entity<Product>().Property(_ => _.Name).HasMaxLength(128);
            mb.Entity<Product>().Property(_ => _.KeyUrl).HasMaxLength(128);
            mb.Entity<Product>().Property(_ => _.PhotoUrl).HasMaxLength(128);
            mb.Entity<Product>().Property(_ => _.SeoDescription).HasMaxLength(256);
            mb.Entity<Product>().Property(_ => _.SeoKeywords).HasMaxLength(256);

            mb.Entity<Product>().HasOptional(x=>x.Brand).WithMany(x=>x.Products).HasForeignKey(x=>x.BrandId);
            mb.Entity<Product>().HasMany(p => p.Categories).WithMany(c => c.Products)
                .Map(t => t.MapLeftKey("ProductId").MapRightKey("CategoryId").ToTable("ProductCategories"));
            mb.Entity<Product>().HasMany(p => p.ProductTags).WithMany(c => c.Products)
                .Map(t => t.MapLeftKey("ProductId").MapRightKey("ProductTagId").ToTable("ProductProductTags"));

            #endregion

            #region ProductTag

            mb.Entity<ProductTag>().HasKey(_ => _.Id);
            mb.Entity<ProductTag>().Property(_ => _.Name).HasMaxLength(128);
            mb.Entity<ProductTag>().Property(_ => _.Name).IsRequired();

            #endregion

            #region Category

            mb.Entity<Category>().Property(_ => _.Name).HasMaxLength(128);
            mb.Entity<Category>().Property(_ => _.KeyUrl).HasMaxLength(128);

            mb.Entity<Category>().HasOptional(x => x.Parent).WithMany(x => x.ChildCategories).HasForeignKey(x => x.ParentId);

            #endregion

            #region Coupon

            mb.Entity<Coupon>().Property(_ => _.Code).HasMaxLength(32);

            #endregion

            #region Brand

            mb.Entity<Brand>().Property(_ => _.Name).HasMaxLength(128);
            mb.Entity<Brand>().Property(_ => _.KeyUrl).HasMaxLength(128);
            mb.Entity<Brand>().Property(_ => _.PhotoUrl).HasMaxLength(128);
            mb.Entity<Brand>().Property(_ => _.SeoDescription).HasMaxLength(256);
            mb.Entity<Brand>().Property(_ => _.SeoKeywords).HasMaxLength(256);

            #endregion

            #region SitePage

            mb.Entity<SitePage>().HasKey(_ => _.Id);
            mb.Entity<SitePage>().Property(_ => _.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<SitePage>().Property(_ => _.Title).HasMaxLength(128);
            mb.Entity<SitePage>().Property(_ => _.SeoDescription).HasMaxLength(256);
            mb.Entity<SitePage>().Property(_ => _.SeoKeywords).HasMaxLength(256);

            #endregion

            #region Slider

            mb.Entity<Slider>().Property(_ => _.PhotoUrl).HasMaxLength(128);

            #endregion

            #region OrderHeader

            mb.Entity<OrderHeader>().Property(_ => _.Address).HasMaxLength(512);

            mb.Entity<OrderHeader>().HasRequired(x=>x.User).WithMany(x=>x.OrderHeaders).HasForeignKey(x=>x.UserId);
            mb.Entity<OrderHeader>().HasOptional(x=>x.Coupon).WithMany(x=>x.OrderHeaders).HasForeignKey(x=>x.CouponId);

            #endregion

            #region OrderProduct
            
            mb.Entity<OrderProduct>().HasRequired(x => x.Order).WithMany(x => x.OrderProducts).HasForeignKey(x => x.OrderId);
            mb.Entity<OrderProduct>().HasRequired(x => x.Product).WithMany(x => x.OrderProducts).HasForeignKey(x => x.ProductId);

            #endregion

            #region WishList

            mb.Entity<WishList>().HasRequired(x => x.User).WithMany(x => x.WishLists).HasForeignKey(x => x.UserId);
            mb.Entity<WishList>().HasRequired(x => x.Product).WithMany(x => x.WishLists).HasForeignKey(x => x.ProductId);

            #endregion

            #region ProductReview

            mb.Entity<ProductReview>().Property(_ => _.Content).HasMaxLength(1024);

            mb.Entity<ProductReview>().HasRequired(x => x.User).WithMany(x => x.ProductReviews).HasForeignKey(x => x.UserId);
            mb.Entity<ProductReview>().HasRequired(x => x.Product).WithMany(x => x.ProductReviews).HasForeignKey(x => x.ProductId);

            #endregion

            base.OnModelCreating(mb);
        }
    }
}
