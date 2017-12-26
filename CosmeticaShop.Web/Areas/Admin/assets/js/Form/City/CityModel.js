var City = City || {};

(function () {

    if (City.CityModel) {
        console.error("City.CityModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели города
     * @param {     
     * } theParams 
     * @returns {} 
     */
    City.CityModel = function (theParams) {
        theParams = theParams || {};

        this.Id = ko.observable(theParams.Id || "");
        this.Name = ko.observable(theParams.Name || "");

        this.IsEdit = ko.observable(theParams.IsEdit || false);
        return this;
    };

    /**
     * Определяем конструктор
     */
    City.CityModel.prototype.constructor = City.CityModel;

    City.CityModel.prototype.log = function (text) {
        console.log("Ошибка в классе City.CityModel: " + text);
    };

    City.CityModel.prototype.GetData = function () {
        var model = {
            Id: this.Id(),
            Name: this.Name(),
            IsSuccess: false
        }
        if (model.Name === "") {
            model.Message = "Название не может быть пустым";
        } else
            model.IsSuccess = true;
        return model;
    }
})();