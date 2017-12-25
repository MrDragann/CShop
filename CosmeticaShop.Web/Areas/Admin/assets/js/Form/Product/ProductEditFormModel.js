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
        this.UrlMoveToEdit = theParams.UrlMoveToEdit;
        this.UrlUploadPhotos = theParams.UrlUploadPhotos;
        this.UrlDeletePhoto = theParams.UrlDeletePhoto;

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

    Product.ProductEditFormModel.prototype.CollagePhotos = function () {
        var time = 600;
        setTimeout(function () {
            $('.Collage').removeWhitespace().collagePlus(
                {
                    'fadeSpeed': 2000,
                    'targetHeight': 200,
                    'effect': 'effect-6',
                    'allowPartialLastRow': true
                });
        }, time);
    }

    Product.ProductEditFormModel.prototype.SaveChanges = function () {
        var self = this;
        var model = self.Product.GetData();
        $.post(self.UrlSaveChanges, { model: model })
            .success(function (res) {
                if (res.IsSuccess) {
                    var formData = new FormData();
                    var photoFiles = document.getElementById('PhotoFiles').files;
                    if (photoFiles) {
                        $.each(photoFiles, function (i, file) {
                            formData.append("photoFiles[" + i + "]", file);
                        });
                    }

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
                            bootbox.alert(res.Message, e => location.href = self.UrlMoveToEdit + "/" + res.Value);
                        },
                        error: function (err) {
                            console.log(err);
                        }
                    });
                } else if (res.Status === Enums.EnumResponseStatus.Exception)
                    console.log("ex:", res.Message);
                else
                    bootbox.alert(res.Message);
            })
        .fail(function (res) {
            console.log('res:', res);
        });
    }

    Product.ProductEditFormModel.prototype.DeletePhoto = function (data) {
        var self = this;
        bootbox.confirm("Вы действительно хотите удалить изображение?", function (e) {
            if (e) {
                $.post(self.UrlDeletePhoto, {productId:self.Product.Id(),photo:data})
                    .success(function (res) {
                        if (res) {
                            self.Product.Photos.remove(data);
                        }
                    })
                    .fail(function (res) {
                        console.log('res:', res);
                    });;
            }
        });
    };

    Product.ProductEditFormModel.prototype.OpenSelectModal = function (data) {
        var self = this;

        ProdeuctSelectModal.SimilarProducts = self.Product.SimilarProducts;
        $(ProdeuctSelectModal.ModalSelector).modal("show");
    }

    Product.ProductEditFormModel.prototype.DeleteSimilarProduct = function (data) {
        var self = this;
        self.Product.SimilarProducts(self.Product.SimilarProducts().filter(function (item) { return item.Id() !== data.Id() }));
    }

})();