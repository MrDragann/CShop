var Product = Product || {};

(function () {

    Product.ProductModel = function (theParams) {
        theParams = theParams || {};
        this.Id = ko.observable(theParams.Id || "");
        this.Name = ko.observable(theParams.Name || "");
        this.Description = ko.observable(theParams.Description || "");
        this.Price = ko.observable(theParams.Price || 0);
        this.Discount = ko.observable(theParams.Discount || 0);
        this.PhotoUrl = ko.observable(theParams.PhotoUrl || "");
        this.DateCreate = ko.observable(theParams.DateCreate || "");
        this.DateCreateView = ko.observable(theParams.DateCreateView || "");
        this.Content = ko.observable(theParams.Content || "");
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
            PhotoUrl: this.PhotoUrl()          
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
    Product.ProductModel.prototype.AddProductWish = function () {
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

