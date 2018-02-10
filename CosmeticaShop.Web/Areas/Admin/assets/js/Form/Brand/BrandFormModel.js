var Brand = Brand || {};

/*********************** Brand.BrandFormModel **********************************************/
(function () {

    if (Brand.BrandFormModel) {
        console.error('Brand.BrandFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы с таблицей модели брендов
     */
    Brand.BrandFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlDelete = theParams.UrlDelete;

        this.Filter = new Base.BaseFilterModel();
        this.Table = new Components.Table({ Url: theParams.UrlLoadItems, ModalClass: Brand.BrandModel, Filter: this.Filter });
        this.TableRefresh = this.Table.Refresh.bind(this.Table);

        return this;
    };

    /**
     * Определяем конструктор
     */
    Brand.BrandFormModel.prototype.constructor = Brand.BrandFormModel;

    Brand.BrandFormModel.prototype.DeleteBrand = function (data) {
        var self = this;

        bootbox.confirm("Вы действительно хотите удалить бренд \"" + data.Name() + "\"?",
            function(e) {
                if (e) {
                    $.post(self.UrlDelete, { brandId: data.Id() })
                        .success(function(res) {
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