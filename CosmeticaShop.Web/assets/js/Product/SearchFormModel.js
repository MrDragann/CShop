var Product = Product || {};

(function () {

    Product.SearchFormModel = function (theParams) {
        theParams = theParams || {};
        var self = this;
        this.UrlProduct = theParams.UrlProduct;
        this.Search = ko.observable(theParams.Search || "");
        return this;
    };

    /**
    * Определяем конструктор
    */
    Product.SearchFormModel.prototype.constructor = Product.SearchFormModel;

    Product.SearchFormModel.prototype.GetSearch = function () {
        location.href = this.UrlProduct + "?Search=" + this.Search();
    };


})();
