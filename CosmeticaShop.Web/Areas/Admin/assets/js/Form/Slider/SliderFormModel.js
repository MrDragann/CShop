var Slider = Slider || {};

/*********************** Slider.SliderFormModel **********************************************/
(function () {

    if (Slider.SliderFormModel) {
        console.error('Slider.SliderFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы с таблицей модели слайдов
     */
    Slider.SliderFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlDelete = theParams.UrlDelete;

        this.Filter = new Base.BaseFilterModel();
        this.Table = new Components.Table({ Url: theParams.UrlLoadItems, ModalClass: Slider.SliderModel, Filter: this.Filter });
        this.TableRefresh = this.Table.Refresh.bind(this.Table);

        return this;
    };

    /**
     * Определяем конструктор
     */
    Slider.SliderFormModel.prototype.constructor = Slider.SliderFormModel;

    Slider.SliderFormModel.prototype.DeleteSlide = function (data) {
        var self = this;

        bootbox.confirm("Вы действительно хотите удалить слайд?",
            function(e) {
                if (e) {
                    $.post(self.UrlDelete, { id: data.Id() })
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