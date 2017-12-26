var Product = Product || {};

(function () {

    Product.ProductModel = function (theParams) {
        theParams = theParams || {};
        this.Id = ko.observable(theParams.Id || "");
        this.Name = ko.observable(theParams.Name || "");
        this.Description = ko.observable(theParams.Description || "");
        this.Price = ko.observable(theParams.Price || 0);
        this.BrandName = ko.observable(theParams.BrandName || "");
        this.DiscountPrice = ko.observable(theParams.DiscountPrice || 0);
        this.DiscountPercent = ko.observable(theParams.DiscountPercent || 0);
        this.PhotoUrl = ko.observable(theParams.PhotoUrl || "");
        this.DateCreate = ko.observable(theParams.DateCreate || "");
        this.DateCreateView = ko.observable(theParams.DateCreateView || "");
        this.Content = ko.observable(theParams.Content || "");
        this.IsWish = ko.observable(theParams.IsWish || false);
        this.TagsId = ko.observableArray(theParams.TagsId || []);
        this.CheckWish();
        return this;
    };

    /**
    * Определяем конструктор
    */
    Product.ProductModel.prototype.constructor = Product.ProductModel;

    /**
    * Получить данные модели
    */
    Product.ProductModel.prototype.GetData = function () {
        return {
            Id: this.Id(),
            Name: this.Name(),
            Description: this.Description(),
            Price: this.Price(),
            Discount: this.Discount(),
            PhotoUrl: this.PhotoUrl(),
            TagsId: this.TagsId()
        }
    }

    /**
   * Добавить товар в корзину 
   */
    Product.ProductModel.prototype.AddProductCart = function () {
        var self = this;
        $.post(urlAddProductCart, {
            productId: this.Id(),
            quantity: 1
        }).success(function (res) {
            if (res.IsSuccess) {
                $("#cart-success").modal("show");
                CartHeaderModel.GetCart();
            }
            else {
                console.error(res.Message);
            }
        });
    }
    /**
    * Добавить товар в желаемое 
    */
    Product.ProductModel.prototype.AddProductWish = function (params) {
        var self = this;
        $.post(urlAddProductWish, {
            productId: this.Id()
        }).success(function (res) {
            if (res.IsSuccess) {
                self.IsWish(true);
                $("#wish-success").modal("show");
                if (params && params === "pageWish") {
                    ViewModel.Products.push(self);
                }
            }
            else {
                console.error(res.Message);
            }
        });
    }
    /**
    * Удалить товар из желаемое 
    */
    Product.ProductModel.prototype.DeleteWish = function () {
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
    /**
   * Проверить список желаемого
   */
    Product.ProductModel.prototype.CheckWish = function () {
        var self = this;
        var cookieWish = getCookie("UserWish");
        if (cookieWish) {
            var wish = cookieWish.split("&");
            if (wish && wish[0]) {
                var arr = wish[0].split("=");
                if (arr[1]) {
                    var ids = arr[1].split(",");
                    self.IsWish(ids.some(function(item) {
                        return item === self.Id().toString();
                    }));
                }
            }
        }
    }
})();

// возвращает cookie с именем name, если есть, если нет, то undefined
function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}