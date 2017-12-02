using System;
using System.Collections.Generic;
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

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            #region User
            
            mb.Entity<User>().Property(_ => _.Email).HasMaxLength(128);
            mb.Entity<User>().Property(_ => _.PasswordHash).HasMaxLength(128);
            mb.Entity<User>().Property(_ => _.FirstName).HasMaxLength(128);
            mb.Entity<User>().Property(_ => _.LastName).HasMaxLength(128);

            mb.Entity<User>().Property(_ => _.Email).IsRequired();
            mb.Entity<User>().Property(_ => _.PasswordHash).IsRequired();

            #endregion

            #region Role

            mb.Entity<Role>().Property(_ => _.Name).HasMaxLength(128);
            mb.Entity<Role>().Property(_ => _.Name).IsRequired();

            #endregion

            #region Product
            
            mb.Entity<Product>().Property(_ => _.Name).HasMaxLength(128);
            mb.Entity<Product>().Property(_ => _.KeyUrl).HasMaxLength(128);
            
            #endregion

            #region Category
            
            mb.Entity<Category>().Property(_ => _.Name).HasMaxLength(128);
            mb.Entity<Category>().Property(_ => _.KeyUrl).HasMaxLength(128);

            mb.Entity<Category>().HasOptional(x => x.Parent).WithMany(x => x.ChildCategories).HasForeignKey(x => x.ParentId);

            #endregion

            #region OrderHeader

            mb.Entity<OrderHeader>().Property(_ => _.Address).HasMaxLength(512);


            #endregion

            base.OnModelCreating(mb);
        }
    }
}
