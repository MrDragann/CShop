var Brand = Brand || {};

(function () {

    if (Brand.BrandModel) {
        console.error("Brand.BrandModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели бренда
     * @param {     
     * } theParams 
     * @returns {} 
     */
    Brand.BrandModel = function (theParams) {
        theParams = theParams || {};

        this.Id = ko.observable(theParams.Id || "");
        this.Name = ko.observable(theParams.Name || "");
        this.PhotoUrl = ko.observable(theParams.PhotoUrl || "");
        this.IsActive = ko.observable(theParams.IsActive || false);


        return this;
    };

    /**
     * Определяем конструктор
     */
    Brand.BrandModel.prototype.constructor = Brand.BrandModel;

    Brand.BrandModel.prototype.log = function (text) {
        console.log("Ошибка в классе Brand.BrandModel: " + text);
    };

    Brand.BrandModel.prototype.GetData = function () {
        return {
            Id: this.Id(),
            Name: this.Name(),
            PhotoUrl: this.PhotoUrl(),
            IsActive: this.IsActive()
        }
    }
})();