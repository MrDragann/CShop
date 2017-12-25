var Address = Address || {};

(function () {

    if (Address.AddressModel) {
        console.error("Address.AddressModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели адреса
     * @param {     
     * } theParams 
     * @returns {} 
     */
    Address.AddressModel = function (theParams) {
        theParams = theParams || {};

        this.Country = ko.observable(theParams.Country || "");
        this.City = ko.observable(theParams.City || "");
        this.Address = ko.observable(theParams.Address || "");
        this.Phone = ko.observable(theParams.Phone || "");

        return this;
    };

    /**
     * Определяем конструктор
     */
    Address.AddressModel.prototype.constructor = Address.AddressModel;

    Address.AddressModel.prototype.log = function (text) {
        console.log("Ошибка в классе Address.AddressModel: " + text);
    };

    Address.AddressModel.prototype.GetData = function () {
        return {
            Country: this.Country(),
            City: this.City(),
            Address: this.Address(),
            Phone: this.Phone()
        }
    }
})();