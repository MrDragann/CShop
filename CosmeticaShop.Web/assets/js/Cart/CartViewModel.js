﻿var Cart = Cart || {};

(function () {

    Cart.CartViewModel = function (theParams) {
        theParams = theParams || {};
        this.UrlDeleteProduct = theParams.UrlDeleteProduct;
        this.UrlGetCart = theParams.UrlGetCart;
        this.UrlOrder = theParams.UrlOrder;
        this.UrlPreparationOrder = theParams.UrlPreparationOrder;
        this.UrlAcceptCoupon = theParams.UrlAcceptCoupon;
      
        this.Model = ko.observableArray(theParams.Model ? theParams.Model.map(function(item) {
            return new Cart.CartModel(item);
        }) : []);
        if (!theParams.Model)
            theParams.Model = this.GetCart();
        this.Amount = ko.observable(theParams.Amount || 0);
        this.AmountCoupon = ko.observable(theParams.AmountCoupon || 0);
        this.CouponCode = ko.observable(theParams.CouponCode || "");
        this.ErrorMessage = ko.observable("");
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
            self.Amount((self.Amount() + item.AmountPrice()));
        });
    };
    /**
     * Оформить заказ
     * @returns {} 
     */
    Cart.CartViewModel.prototype.PreparationOrder = function () {
        var self = this;

        $.post(this.UrlPreparationOrder, {
            productsOrder: self.Model().map(function (item) { return item.GetData(); }),
            couponCode: this.CouponCode()
        }).success(function (res) {
            if (res.IsSuccess) {
                location.href = self.UrlOrder + "?orderId=" + res.Value;
            } else {
                self.ErrorMessage(res.Message);
            }

        });
    };
    /**
    * Применить купон
    * @returns {} 
    */
    Cart.CartViewModel.prototype.AcceptCoupon = function () {
        var self = this;
        $.post(this.UrlAcceptCoupon, {
            productsOrder: self.Model().map(function (item) { return item.GetData(); }),
            couponCode: this.CouponCode()
        }).success(function (res) {
            if (res.IsSuccess) {
                self.AmountCoupon(res.Value);
                self.ErrorMessage("");
            } else {
                self.ErrorMessage(res.Message);
            }

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
    /**
     * Загрузить корзину
     * @returns {} 
     */
    Cart.CartViewModel.prototype.GetCart = function () {
        var self = this;
        $.post(this.UrlGetCart).success(function (res) {
            self.Model(res.length > 0 ? res.map(function (item) { return new Cart.CartModel(item) }) : []);
            self.GetAmount();
        });
    };
})();
