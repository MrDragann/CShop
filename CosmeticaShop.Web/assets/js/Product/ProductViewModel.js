var Product = Product || {};

(function () {

    Product.ProductViewModel = function (theParams) {
        theParams = theParams || {};
        this.Products = ko.observableArray(theParams.Products
            ? theParams.Products.map(function(item) {
                return new Product.ProductModel(item);
            })
            : []);

        return this;
    };

    /**
    * Определяем конструктор
    */
    Product.ProductViewModel.prototype.constructor = Product.ProductViewModel;




})();
