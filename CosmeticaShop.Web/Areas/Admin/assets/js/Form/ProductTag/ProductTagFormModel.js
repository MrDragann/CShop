var ProductTag = ProductTag || {};

/*********************** ProductTag.ProductTagFormModel **********************************************/
(function () {

    if (ProductTag.ProductTagFormModel) {
        console.error('ProductTag.ProductTagFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы со справочником видов субконто
     */
    ProductTag.ProductTagFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlSaveChanges = theParams.UrlSaveChanges;
        this.UrlDelete = theParams.UrlDelete;
        this.ModalSelector = theParams.ModalSelector;

        this.Filter = new Base.BaseFilterModel();
        this.Table = new Components.Table({ Url: theParams.UrlLoadItems, ModalClass: ProductTag.ProductTagModel, Filter: this.Filter });
        this.TableRefresh = this.Table.Refresh.bind(this.Table);
        
        this.NewProductTag = ko.observable(new ProductTag.ProductTagModel());

        return this;
    };

    /**
     * Определяем конструктор
     */
    ProductTag.ProductTagFormModel.prototype.constructor = ProductTag.ProductTagFormModel;

    ProductTag.ProductTagFormModel.prototype.ChangeTemplate = function (data) {
        var self = this;
        self.Table.Items().forEach(function (entry, i) {
            entry.IsEdit(false);
        });
        data.IsEdit(true);
    };

    /**
     * Сохраняем изменения
     * @param {} data 
     * @returns {} 
     */
    ProductTag.ProductTagFormModel.prototype.SaveChanges = function (data) {
        var self = this;
        var model = data.GetData();
        if (model.IsSuccess) {
            if (self.UrlSaveChanges) {
                $.post(self.UrlSaveChanges, { model: model })
                    .success(function (res) {
                        if (res.IsSuccess) {
                            if (model.Id === "")
                                $(self.ModalSelector).hide();
                            self.TableRefresh();
                        }
                        else {
                            bootbox.alert(res.Message);
                        }
                    })
                    .fail(function (res) {
                        console.log(res);
                    });
            }
        } else {
            bootbox.alert(model.Message);
        }
    };
    /**
     * Сбрасываем шаблон на обычный
     * @param {} data 
     * @returns {} 
     */
    ProductTag.ProductTagFormModel.prototype.ResetTemplate = function (data) {
        var self = this;
        self.Table.Items().forEach(function (entry, i) {
            entry.IsEdit(false);
        });
        self.TableRefresh();
    };

    ProductTag.ProductTagFormModel.prototype.DeleteProductTag = function (data) {
        var self = this;

        if (data) {
            bootbox.confirm("Вы действительно хотите удалить тег товара " + data.Name() + " ?", function (e) {
                if (e) {
                    $.post(self.UrlDelete, {
                            ProductTagId: data.Id()
                        })
                        .done(function (res) {
                            self.TableRefresh();
                            bootbox.alert(res.Message);
                        })
                        .fail(function (res) {
                            bootbox.alert('Неизвестная ошибка при загрузке');
                        });
                }
            });
        }
    }

})();