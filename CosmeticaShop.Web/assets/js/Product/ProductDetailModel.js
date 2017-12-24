var Product = Product || {};

(function () {

    Product.ProductDetailModel = function (theParams) {
        theParams = theParams || {};
        this.Id = ko.observable(theParams.Id || "");
        this.Name = ko.observable(theParams.Name || "");
        this.Description = ko.observable(theParams.Description || "");
        this.Price = ko.observable(theParams.Price || 0);
        this.Discount = ko.observable(theParams.Discount || 0);
        this.PhotoUrl = ko.observable(theParams.PhotoUrl || "");
        this.Photos = ko.observableArray(theParams.Photos || []);
        this.Quantity = ko.observable(theParams.Quantity || 1);
        this.DateCreate = ko.observable(theParams.DateCreate || "");
        this.DateCreateView = ko.observable(theParams.DateCreateView || "");
        this.Content = ko.observable(theParams.Content || "");
        this.BrandName = ko.observable(theParams.BrandName || "");
        this.Reviews = ko.observableArray(theParams.Reviews || []);
        this.IsWish = ko.observable(theParams.IsWish || false);
        this.DiscountPrice = ko.observable(theParams.DiscountPrice || 0);
        this.DiscountPercent = ko.observable(theParams.DiscountPercent || 0);
        this.IsInStock = ko.observable(theParams.IsInStock || false);
        this.CheckWish();
        return this;
    };

    /**
    * Определяем конструктор
    */
    Product.ProductDetailModel.prototype.constructor = Product.ProductDetailModel;

    /**
    * Получить данные модели
    */
    Product.ProductDetailModel.prototype.GetData = function () {
        return {
            Id: this.Id(),
            Name: this.Name(),
            Description: this.Description(),
            Price: this.Price(),
            Discount: this.Discount(),
            PhotoUrl: this.PhotoUrl()
        }
    }

    /**
   * Добавить товар в корзину 
   */
    Product.ProductDetailModel.prototype.AddProductCart = function () {
        var self = this;
        $.post(urlAddProductCart, {
            productId: this.Id(),
            quantity: this.Quantity()
        }).success(function (res) {
            if (res.IsSuccess) {
                $("#cart-success").modal("show");
            }
            else {
                self.ErrorMessage(res.Message);
            }
        });
    }
    /**
    * Добавить товар в желаемое 
    */
    Product.ProductDetailModel.prototype.AddProductWish = function () {
        var self = this;
        $.post(urlAddProductWish, {
            productId: this.Id()
        }).success(function (res) {
            if (res.IsSuccess) {
                self.IsWish(true);
                $("#wish-success").modal("show");
            }
            else {
                self.ErrorMessage(res.Message);
            }
        });
    }
    /**
    * Проверить список желаемого
    */
    Product.ProductDetailModel.prototype.CheckWish = function () {
        var self = this;
        var cookieWish = getCookie("UserWish");
        if (cookieWish) {
            var wish = cookieWish.split("&");
            if (wish && wish[0]) {
                var arr = wish[0].split("=");
                if (arr[1]) {
                    var ids = arr[1].split(",");
                    self.IsWish(ids.some(function (item) {
                        return item === self.Id().toString();
                    }));
                }
            }
        }
    }
    /**
* Удалить товар из желаемое 
*/
    Product.ProductDetailModel.prototype.DeleteWish = function () {
        var self = this;
        $.post(urlDeleteWish, {
            productId: this.Id()
        }).success(function (res) {
            if (res.IsSuccess) {
                self.IsWish(false);
            }
            else {
                console.error(res.Message);
            }
        });
    }
})();
// возвращает cookie с именем name, если есть, если нет, то undefined
function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}
