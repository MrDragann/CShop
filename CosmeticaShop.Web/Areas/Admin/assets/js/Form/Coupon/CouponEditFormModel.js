var Coupon = Coupon || {};

/*********************** Coupon.CouponEditFormModel **********************************************/
(function () {

    if (Coupon.CouponEditFormModel) {
        console.error('Coupon.CouponEditFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы с таблицей модели товара
     */
    Coupon.CouponEditFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlSaveChanges = theParams.UrlSaveChanges;
        this.UrlMoveToEdit = theParams.UrlMoveToEdit;

        this.Coupon = new Coupon.CouponModel(theParams.Model);

        return this;
    };

    /**
     * Определяем конструктор
     */
    Coupon.CouponEditFormModel.prototype.constructor = Coupon.CouponEditFormModel;

    Coupon.CouponEditFormModel.prototype.SaveChanges = function () {
        var self = this;
        var model = self.Coupon.GetData();
        if (model.IsSuccess) {
            $.post(self.UrlSaveChanges, { model: model })
                .success(function(res) {
                    if (res.IsSuccess) {
                        bootbox.alert(res.Message, e => location.href = self.UrlMoveToEdit + "?couponId=" + res.Value);
                    } else if (res.Status === Enums.EnumResponseStatus.Exception)
                        console.log("ex:", res.Message);
                    else
                        bootbox.alert(res.Message);
                })
                .fail(function(res) {
                    console.log('res:', res);
                });
        } else
            bootbox.alert(model.Message);
    }
    
    Coupon.CouponEditFormModel.prototype.GenerateCode = function(data) {
        var self = this;
        var code = "";
        var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        for (var i = 0; i < 10; i++)
            code += possible.charAt(Math.floor(Math.random() * possible.length));

        data.Code(code);
    }

})();