var Blog = Blog || {};

(function () {

    if (Blog.BlogModel) {
        console.error("Blog.BlogModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели бренда
     * @param {     
     * } theParams 
     * @returns {} 
     */
    Blog.BlogModel = function (theParams) {
        theParams = theParams || {};

        this.Id = ko.observable(theParams.Id || "");
        this.Title = ko.observable(theParams.Title || "");
        this.PhotoUrl = ko.observable(theParams.PhotoUrl || "");
        this.KeyUrl = ko.observable(theParams.KeyUrl || "");
        this.IsActive = ko.observable(theParams.IsActive || false);


        return this;
    };

    /**
     * Определяем конструктор
     */
    Blog.BlogModel.prototype.constructor = Blog.BlogModel;

    Blog.BlogModel.prototype.log = function (text) {
        console.log("Ошибка в классе Blog.BlogModel: " + text);
    };

    Blog.BlogModel.prototype.GetData = function () {
        return {

        }
    }
})();