var Coupon = Coupon || {};

/*********************** Coupon.CouponFormModel **********************************************/
(function () {

    if (Coupon.CouponFormModel) {
        console.error('Coupon.CouponFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы с таблицей модели брендов
     */
    Coupon.CouponFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlDelete = theParams.UrlDelete;

        this.Filter = new Base.BaseFilterModel();
        this.Table = new Components.Table({ Url: theParams.UrlLoadItems, ModalClass: Coupon.CouponModel, Filter: this.Filter });
        this.TableRefresh = this.Table.Refresh.bind(this.Table);

        return this;
    };

    /**
     * Определяем конструктор
     */
    Coupon.CouponFormModel.prototype.constructor = Coupon.CouponFormModel;

    Coupon.CouponFormModel.prototype.Delete = function (data) {
        var self = this;

        bootbox.confirm("Вы действительно хотите удалить купон \"" + data.Code() + "\"?",
            function (e) {
                if (e) {
                    $.post(self.UrlDelete, { couponId: data.Id() })
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