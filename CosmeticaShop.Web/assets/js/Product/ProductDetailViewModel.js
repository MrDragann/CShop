var Product = Product || {};

(function () {

    Product.ProductDetailViewModel = function (theParams) {
        theParams = theParams || {};
        this.UrlAddReview = theParams.UrlAddReview;
        this.Model = ko.observable(new Product.ProductDetailModel(theParams.Model));
        this.SelectQuantity = ko.observableArray(this.GetSelectQuantity());
        this.Review = ko.observable(theParams.Review || "");
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

    /**
     * Добавить отзыв
     * @returns {} 
     */
    Product.ProductDetailViewModel.prototype.AddReview = function() {
        var self = this;
        $.post(this.UrlAddReview, { message: this.Review(),productId: this.Model().Id()})
            .success(function (res) {
                console.log(res);
            });
    };

})();
