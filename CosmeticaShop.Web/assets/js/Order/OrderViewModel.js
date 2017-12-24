var Order = Order || {};

(function () {

    Order.OrderViewModel = function (theParams) {
        theParams = theParams || {};
        this.UrlAddOrder = theParams.UrlAddOrder;
        this.Model = new Order.OrderHeaderModel(theParams.Model || {});
        this.IsSuccess = ko.observable(theParams.IsSuccess || false);
        return this;
    };

    /**
    * Определяем конструктор
    */
    Order.OrderViewModel.prototype.constructor = Order.OrderViewModel;

    /**
     * Сделать заказ
     * @returns {} 
     */
    Order.OrderViewModel.prototype.AddOrder = function() {
        var self = this;
        var model = this.Model.GetData();
        $.post(this.UrlAddOrder, {
            orderId: self.Model.Id(),
            address: model.Address
        }).success(function(res) {
            if (res.IsSuccess) {
                self.IsSuccess(true);
            }

        });
    }
})();
