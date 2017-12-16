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
        this.UrlUploadPhotos = theParams.UrlUploadPhotos;
        

        this.Product = new Product.ProductModel(theParams.Model.Product);
        this.Categories = ko.observableArray(theParams.Model.Categories);
        this.Tags = ko.observableArray(theParams.Model.Tags);
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
                    var formData = new FormData();
                    //var photoFiles = document.getElementById('PhotoFiles').files;
                    //if (photoFiles) {
                    //    photoFiles.forEach(function (file, i) {
                    //        formData.append("photoFiles" + i, file);
                    //    });
                    //}
                    
                    var preview = document.getElementById("PhotoFile").files[0];
                    if (preview) {
                        formData.append("photoFile", preview);
                    }
                    formData.append("productId", res.Value);
                    $.ajax({
                        type: "POST",
                        url: self.UrlUploadPhotos,
                        contentType: false,
                        processData: false,
                        data: formData,
                        success: function (result) {
                            bootbox.alert(res.Message, e => location.href = self.UrlBackToList);
                        },
                        error: function(err) {
                            console.log(err);
                        }
                    });
                } else if (res.Status === Enums.EnumResponseStatus.Exception)
                    console.log("ex:", res.Message);
                else
                    bootbox.alert(res.Message);
            })
        .fail(function(res) {
                console.log('res:', res);
            });
    }

})();