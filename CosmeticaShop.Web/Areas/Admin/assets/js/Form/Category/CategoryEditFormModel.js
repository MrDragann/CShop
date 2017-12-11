var Category = Category || {};

/*********************** Category.CategoryEditFormModel **********************************************/
(function () {

    if (Category.CategoryEditFormModel) {
        console.error('Category.CategoryEditFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы с таблицей модели брендов
     */
    Category.CategoryEditFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlSaveChanges = theParams.UrlSaveChanges;
        this.UrlBackToList = theParams.UrlBackToList;

        this.Category = new Category.CategoryModel(theParams.Model.Category);
        this.Categories = ko.observableArray(theParams.Model.Categories);
        this.Categories().unshift({ Id: 0, Name: "Без родителя" });

        return this;
    };

    /**
     * Определяем конструктор
     */
    Category.CategoryEditFormModel.prototype.constructor = Category.CategoryEditFormModel;

    Category.CategoryEditFormModel.prototype.SaveChanges = function() {
        var self = this;
        var model = self.Category.GetData();
        $.post(self.UrlSaveChanges, { model: model })
            .success(function (res) {
                if (res.IsSuccess) {
                    location.href = self.UrlBackToList;
                } else if (res.Status === Enums.EnumResponseStatus.Exception)
                    console.log("ex:", res.Message);
                else
                    bootbox.alert(res.Message);
            });
    }

})();