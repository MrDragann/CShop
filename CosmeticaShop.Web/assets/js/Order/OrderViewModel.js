var Order = Order || {};

(function () {

    Order.OrderViewModel = function (theParams) {
        theParams = theParams || {};
        this.UrlAddOrder = theParams.UrlAddOrder;
        theParams.Model.Email = theParams.Email;
        this.Model = new Order.OrderHeaderModel(theParams.Model || {});
        this.IsSuccess = ko.observable(theParams.IsSuccess || false);
        this.ErrorMessage = ko.observable("");
        this.Cities = ko.observableArray(theParams.Cities || []);
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
    Order.OrderViewModel.prototype.AddOrder = function () {
        var self = this;
        var model = this.Model.GetData();
        var validation = this.Validation(model);
        if (validation) {
            $.post(this.UrlAddOrder,
                {
                    orderId: self.Model.Id(),
                    address: model.Address,
                    email: model.Email
                }).success(function (res) {
                    if (res.IsSuccess) {
                        self.IsSuccess(true);
                    } else {
                        self.ErrorMessage(res.Message);
                    }
                });
        }
    }
    /**
     * Валидация
     * @returns {} 
     */
    Order.OrderViewModel.prototype.Validation = function (model) {
        if (model.Email === "") {
            this.ErrorMessage("Completați e-mailul");
            return false;
        }
        if (model.Address.Country === "" ||
            model.Address.City === "" ||
            model.Address.Phone === "" ||
            model.Address.Address === "") {
            this.ErrorMessage("Completați toate câmpurile");
            return false;
        }
        return true;
    };
})();
