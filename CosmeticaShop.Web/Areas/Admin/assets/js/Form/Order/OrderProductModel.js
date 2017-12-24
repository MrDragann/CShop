var Order = Order || {};

(function () {

    if (Order.OrderProductModel) {
        console.error("Order.OrderProductModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели шапки заказа
     * @param {     
     * } theParams 
     * @returns {} 
     */
    Order.OrderProductModel = function (theParams) {
        theParams = theParams || {};

        this.Id = ko.observable(theParams.Id || "");
        this.ProductId = ko.observable(theParams.ProductId || "");
        this.ProductName = ko.observable(theParams.ProductName || "");
        this.PhotoUrl = ko.observable(theParams.PhotoUrl || "");
        this.Quantity = ko.observable(theParams.Quantity || 0);
        this.Price = ko.observable(theParams.Price || 0);
        this.Discount = ko.observable(theParams.Discount || 0);
        this.Amount = ko.observable(theParams.Amount || 0);

        return this;
    };

    /**
     * Определяем конструктор
     */
    Order.OrderProductModel.prototype.constructor = Order.OrderProductModel;

    Order.OrderProductModel.prototype.log = function (text) {
        console.log("Ошибка в классе Order.OrderProductModel: " + text);
    };

    Order.OrderProductModel.prototype.GetData = function () {
        var model = {
            Id: this.Id(),
            ProductId: this.ProductId(),
            Quantity: this.Quantity(),
            Price: this.Price(),
            Discount: this.Discount(),
            Amount: this.Amount(),
        };
    }
})();