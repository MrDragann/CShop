using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CosmeticaShop.Data;
using CosmeticaShop.Data.Models;
using CosmeticaShop.IServices;
using CosmeticaShop.IServices.Enums;
using CosmeticaShop.IServices.Models.Product;
using CosmeticaShop.IServices.Models.Wish;
using CosmeticaShop.Services.Static;

namespace CosmeticaShop.Services
{
    public class WishService : IWishService 
    {
        /// <summary>
        /// Получить список желаемого
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <returns></returns>
        public List<WishModel> GetWishs(Guid userId)
        {
            using (var db = new DataContext())
            {
                var wishs = db.WishLists.Include(x=>x.Product).Where(x => x.UserId == userId).ToList();

                var response = wishs.Select(ConvertWishModel).ToList();
                foreach (var wishModel in response)
                {
                    wishModel.Product.PhotoUrl = FileManager.GetPreviewImage(EnumDirectoryType.Product, wishModel.Product.Id.ToString());
                }
                return response;

            }
        }

        #region Конвертирование

        private WishModel ConvertWishModel(WishList m)
        {
            return new WishModel
            {
                UserId = m.UserId,
                DateCreate = m.DateCreate,
                ProductId = m.ProductId,
                Id = m.Id,
                Product = new ProductBaseModel { BrandName = m.Product?.Name,
                    DiscountPercent = m.Product.Discount,
                    Id = m.Product.Id,
                    Price = m.Product.Price,
                    PhotoUrl = m.Product?.PhotoUrl,
                    Name = m.Product?.Name,
                    DiscountPrice = CalculationService.GetDiscountPrice(m.Product.Price, m.Product.Discount)

                }
            };
        }

        #endregion
    }
}
