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
                location.reload();
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
                location.reload();
            }
            else {
                self.ErrorMessage(res.Message);
            }
        });
    }

})();

