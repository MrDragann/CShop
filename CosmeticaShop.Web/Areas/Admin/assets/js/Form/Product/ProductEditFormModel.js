var Product = Product || {};

/*********************** Product.ProductEditFormModel **********************************************/
(function () {

    if (Product.ProductEditFormModel) {
        console.error('Product.ProductEditFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы с таблицей модели товара
     */
    Product.ProductEditFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlSaveChanges = theParams.UrlSaveChanges;
        this.UrlBackToList = theParams.UrlBackToList;

        this.Product = new Product.ProductModel(theParams.Model.Product);
        this.Categories = ko.observableArray(theParams.Model.Categories);
        this.Brands = ko.observableArray(theParams.Model.Brands);

        return this;
    };

    /**
     * Определяем конструктор
     */
    Product.ProductEditFormModel.prototype.constructor = Product.ProductEditFormModel;

    Product.ProductEditFormModel.prototype.SaveChanges = function() {
        var self = this;
        var model = self.Product.GetData();
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