var Order = Order || {};

(function () {

    if (Order.OrderHeaderModel) {
        console.error("Order.OrderHeaderModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели шапки заказа
     * @param {     
     * } theParams 
     * @returns {} 
     */
    Order.OrderHeaderModel = function (theParams) {
        theParams = theParams || {};

        this.Id = ko.observable(theParams.Id || "");
        this.DateCreate = ko.observable(moment(theParams.DateCreate).format("L") || "");
        this.Status = ko.observable(theParams.Status || 0);
        this.StatusName = ko.observable(theParams.StatusName || "");
        this.Address = new Address.AddressModel(theParams.Address || {});
        this.Amount = ko.observable(theParams.Amount || 0);
        this.UserId = ko.observable(theParams.UserId || "");
        this.UserName = ko.observable(theParams.UserName || "");

        this.OrderProducts = ko.observableArray(theParams.OrderProducts
            ? theParams.OrderProducts.map(function(item) {
                return new Order.OrderProductModel(item);
            }) : []);

        return this;
    };

    /**
     * Определяем конструктор
     */
    Order.OrderHeaderModel.prototype.constructor = Order.OrderHeaderModel;

    Order.OrderHeaderModel.prototype.log = function (text) {
        console.log("Ошибка в классе Order.OrderHeaderModel: " + text);
    };

    Order.OrderHeaderModel.prototype.GetData = function () {
        var model = {
        };
    }
})();