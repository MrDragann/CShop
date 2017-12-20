var Cart = Cart || {};

(function () {

    Cart.CartViewModel = function (theParams) {
        theParams = theParams || {};
        this.UrlDeleteProduct = theParams.UrlDeleteProduct;
        this.Model = ko.observableArray(theParams.Model ? theParams.Model.map(function (item) { return new Cart.CartModel(item) }) : []);
        this.Amount = ko.observable(theParams.Amount || 0);        
        this.GetAmount();
        return this;
    };

    /**
    * Определяем конструктор
    */
    Cart.CartViewModel.prototype.constructor = Cart.CartViewModel;

    Cart.CartViewModel.prototype.GetAmount = function () {
        var self = this;
        this.Amount(0);
        this.Model().forEach(function (item) {
            self.Amount(self.Amount() + item.AmountPrice());
        });
    };
    /**
     * Оформить заказ
     * @returns {} 
     */
    Cart.CartViewModel.prototype.GetOrder = function () {
        var self = this;
        this.Amount(0);
        this.Model().forEach(function (item) {
            self.Amount(self.Amount() + item.Amount());
        });
    };

    /**
    * Удалить товар из корзины
    * @returns {} 
     */
    Cart.CartViewModel.prototype.DeleteProduct = function (id) {
        var self = this;
        $.post(this.UrlDeleteProduct, {
            productId: id
        }).success(function (res) {
            if (res.IsSuccess) {
                location.reload();
            }

        });
    };
})();
