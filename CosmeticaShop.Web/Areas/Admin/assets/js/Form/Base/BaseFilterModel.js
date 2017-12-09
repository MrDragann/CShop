var Base = Base || {};

(function () {

    if (Base.BaseFilterModel) {
        console.error("Base.BaseFilterModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели фильтра
     * @param {     
     * } theParams 
     * @returns {} 
     */
    Base.BaseFilterModel = function (theParams) {
        theParams = theParams || {};

        this.Term = ko.observable(theParams.Term || "");

        return this;
    };

    /**
     * Определяем конструктор
     */
    Base.BaseFilterModel.prototype.constructor = Base.BaseFilterModel;

    Base.BaseFilterModel.prototype.log = function (text) {
        console.log("Ошибка в классе Base.BaseFilterModel: " + text);
    };

    Base.BaseFilterModel.prototype.GetData = function () {
        return {
            Term: this.Term()
        }
    }
})();