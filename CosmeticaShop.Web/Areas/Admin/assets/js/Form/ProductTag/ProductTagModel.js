var ProductTag = ProductTag || {};

(function () {

    if (ProductTag.ProductTagModel) {
        console.error("ProductTag.ProductTagModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели тега товара
     * @param {     
     * } theParams 
     * @returns {} 
     */
    ProductTag.ProductTagModel = function (theParams) {
        theParams = theParams || {};

        this.Id = ko.observable(theParams.Id || "");
        this.Name = ko.observable(theParams.Name || "");

        this.IsEdit = ko.observable(theParams.IsEdit || false);
        return this;
    };

    /**
     * Определяем конструктор
     */
    ProductTag.ProductTagModel.prototype.constructor = ProductTag.ProductTagModel;

    ProductTag.ProductTagModel.prototype.log = function (text) {
        console.log("Ошибка в классе ProductTag.ProductTagModel: " + text);
    };

    ProductTag.ProductTagModel.prototype.GetData = function () {
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