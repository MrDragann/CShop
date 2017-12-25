var Order = Order || {};

(function () {

    Order.OrderHeaderModel = function (theParams) {
        theParams = theParams || {};
        var self = this;
        this.Id = ko.observable(theParams.Id || "");
        this.DateCreate = ko.observable(theParams.DateCreate || "");       
        this.Amount = ko.observable(theParams.Amount || 0);           
        this.City = ko.observable(theParams.Address.City || "");
        this.Country = ko.observable("România");
        this.Phone = ko.observable(theParams.Address.Phone || "");
        this.Address = ko.observable(theParams.Address.Address || "");
        this.Email = ko.observable(theParams.Email || "");
        return this;
    };

    /**
    * Определяем конструктор
    */
    Order.OrderHeaderModel.prototype.constructor = Order.OrderHeaderModel;

    /**
    * Получить данные модели
    */
    Order.OrderHeaderModel.prototype.GetData = function () {
        return {
            Id: this.Id(),
            Email:this.Email(),
            Address: { City: this.City(), Country: this.Country(), Phone: this.Phone(),Address:this.Address()}
        }
    }



})();

