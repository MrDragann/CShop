var City = City || {};

/*********************** City.CityFormModel **********************************************/
(function () {

    if (City.CityFormModel) {
        console.error('City.CityFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы со справочником видов субконто
     */
    City.CityFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlSaveChanges = theParams.UrlSaveChanges;
        this.UrlDelete = theParams.UrlDelete;
        this.ModalSelector = theParams.ModalSelector;

        this.Filter = new Base.BaseFilterModel();
        this.Table = new Components.Table({ Url: theParams.UrlLoadItems, ModalClass: City.CityModel, Filter: this.Filter });
        this.TableRefresh = this.Table.Refresh.bind(this.Table);
        
        this.NewCity = ko.observable(new City.CityModel());

        return this;
    };

    /**
     * Определяем конструктор
     */
    City.CityFormModel.prototype.constructor = City.CityFormModel;

    City.CityFormModel.prototype.ChangeTemplate = function (data) {
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
    City.CityFormModel.prototype.SaveChanges = function (data) {
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
    City.CityFormModel.prototype.ResetTemplate = function (data) {
        var self = this;
        self.Table.Items().forEach(function (entry, i) {
            entry.IsEdit(false);
        });
        self.TableRefresh();
    };

    City.CityFormModel.prototype.DeleteCity = function (data) {
        var self = this;

        if (data) {
            bootbox.confirm("Вы действительно хотите удалить город " + data.Name() + " ?", function (e) {
                if (e) {
                    $.post(self.UrlDelete, {
                            cityId: data.Id()
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