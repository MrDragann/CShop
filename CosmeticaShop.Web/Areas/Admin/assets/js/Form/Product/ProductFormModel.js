var Product = Product || {};

/*********************** Product.ProductFormModel **********************************************/
(function () {

    if (Product.ProductFormModel) {
        console.error('Product.ProductFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы с таблицей модели брендов
     */
    Product.ProductFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlDelete = theParams.UrlDelete;

        this.Filter = new Base.BaseFilterModel();
        this.Table = new Components.Table({ Url: theParams.UrlLoadItems, ModalClass: Product.ProductModel, Filter: this.Filter });
        this.TableRefresh = this.Table.Refresh.bind(this.Table);

        return this;
    };

    /**
     * Определяем конструктор
     */
    Product.ProductFormModel.prototype.constructor = Product.ProductFormModel;

    Product.ProductFormModel.prototype.DeleteProduct = function (data) {
        var self = this;

        bootbox.confirm("Вы действительно хотите удалить товар \"" + data.Name() + "\"?",
            function (e) {
                if (e) {
                    $.post(self.UrlDelete, { productId: data.Id() })
                        .success(function (res) {
                            if (res.IsSuccess) {
                                bootbox.alert(res.Message);
                                self.TableRefresh();
                            }
                            else if (res.Status === Enums.EnumResponseStatus.Exception)
                                console.log("ex:", res.Message);
                            else
                                bootbox.alert(res.Message);
                        });
                }
            });
    }

})();