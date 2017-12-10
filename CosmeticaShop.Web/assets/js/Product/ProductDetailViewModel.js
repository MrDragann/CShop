var Product = Product || {};

(function () {

    Product.ProductDetailViewModel = function (theParams) {
        theParams = theParams || {};
        this.Model = ko.observable(new Product.ProductDetailModel(theParams.Model));
        this.SelectQuantity = ko.observableArray(this.GetSelectQuantity());
        return this;
    };

    /**
    * Определяем конструктор
    */
    Product.ProductDetailViewModel.prototype.constructor = Product.ProductDetailViewModel;

    Product.ProductDetailViewModel.prototype.GetSelectQuantity = function() {
        var arr = [];
        for (var i = 1; i <= 10; i++) {
            arr.push(i);
        }
        return arr;
    }


})();
