var Category = Category || {};

(function () {

    if (Category.CategoryModel) {
        console.error("Category.CategoryModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели категории
     * @param {     
     * } theParams 
     * @returns {} 
     */
    Category.CategoryModel = function (theParams) {
        theParams = theParams || {};

        this.Id = ko.observable(theParams.Id || "");
        this.Name = ko.observable(theParams.Name || "");
        this.ParentId = ko.observable(theParams.ParentId || "");
        this.KeyUrl = ko.observable(theParams.KeyUrl || "");

        this.IsActive = ko.observable(theParams.IsActive || false);

        this.IsClosed = ko.observable(false);

        this.ChildCategories = ko.observableArray((theParams.ChildCategories || [])
            .map(function (item) { return new Category.CategoryModel(item) }));
        return this;
    };

    /**
     * Определяем конструктор
     */
    Category.CategoryModel.prototype.constructor = Category.CategoryModel;

    Category.CategoryModel.prototype.log = function (text) {
        console.log("Ошибка в классе Category.CategoryModel: " + text);
    };

    Category.CategoryModel.prototype.GetData = function () {
        return {
            Id: this.Id(),
            Name: this.Name(),
            ParentId: this.ParentId()===0?"":this.ParentId(),
            KeyUrl: this.KeyUrl(),
            IsActive: this.IsActive()
        }
    }
})();