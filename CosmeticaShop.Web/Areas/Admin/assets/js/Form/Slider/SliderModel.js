var Slider = Slider || {};

(function () {

    if (Slider.SliderModel) {
        console.error("Slider.SliderModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели слайдера
     * @param {     
     * } theParams 
     * @returns {} 
     */
    Slider.SliderModel = function (theParams) {
        theParams = theParams || {};

        this.Id = ko.observable(theParams.Id || "");
        this.PhotoUrl = ko.observable(theParams.PhotoUrl || "");
        this.DateCreate = ko.observable(theParams.DateCreate || "");
        this.IsActive = ko.observable(theParams.IsActive || false);


        return this;
    };

    /**
     * Определяем конструктор
     */
    Slider.SliderModel.prototype.constructor = Slider.SliderModel;

    Slider.SliderModel.prototype.log = function (text) {
        console.log("Ошибка в классе Slider.SliderModel: " + text);
    };

    Slider.SliderModel.prototype.GetData = function () {
        return {
            Id: this.Id(),
            IsActive: this.IsActive()
        }
    }
})();