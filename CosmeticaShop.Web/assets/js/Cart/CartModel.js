var Cart = Cart || {};

(function () {

    Cart.CartModel = function (theParams) {
        theParams = theParams || {};
        var self = this;
        if (!theParams.Product)
            theParams.Product = {};
        this.Id = ko.observable(theParams.Id || ""); 
        this.ProductId = ko.observable(theParams.ProductId || 0);
        this.KeyUrl = ko.observable(theParams.Product.KeyUrl || "");
        this.Name = ko.observable(theParams.Product.Name || "");
        this.Price = ko.observable(theParams.Price || 0);
        this.Amount = ko.observable(theParams.Amount / theParams.Quantity || 0);
        this.BrandName = ko.observable(theParams.Product.BrandName || "");
        this.Decimal = ko.observable(theParams.Decimal || 0);
        this.Discount = ko.observable(theParams.Discount || 0);
        this.PhotoUrl = ko.observable(theParams.Product.PhotoUrl || "");
        this.DateCreate = ko.observable(theParams.DateCreate || "");
        this.DateCreateView = ko.observable(theParams.DateCreateView || "");
        this.Quantity = ko.observable(theParams.Quantity || 1);
        self.AmountPrice = ko.observable(theParams.Amount || 0);
        this.Quantity.subscribe(function (item) {           
            self.AmountPrice(self.Amount() * item);       
            ViewModel.GetAmount();
        });
        return this;
    };

    /**
    * Определяем конструктор
    */
    Cart.CartModel.prototype.constructor = Cart.CartModel;

    /**
    * Получить данные модели
    */
    Cart.CartModel.prototype.GetData = function () {
        return {
            Id: this.Id(),
            ProductId: this.ProductId(),
            Quantity: this.Quantity()
        }
    }



})();

