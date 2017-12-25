var Coupon = Coupon || {};

(function () {

    if (Coupon.CouponModel) {
        console.error("Coupon.CouponModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели купона
     * @param {     
     * } theParams 
     * @returns {} 
     */
    Coupon.CouponModel = function (theParams) {
        theParams = theParams || {};

        this.Id = ko.observable(theParams.Id || "");
        this.Code = ko.observable(theParams.Code || "");
        this.Discount = ko.observable(theParams.Discount || 0);

        this.DateCreate = ko.observable(theParams.DateCreate || "");
        return this;
    };

    /**
     * Определяем конструктор
     */
    Coupon.CouponModel.prototype.constructor = Coupon.CouponModel;

    Coupon.CouponModel.prototype.log = function (text) {
        console.log("Ошибка в классе Coupon.CouponModel: " + text);
    };

    Coupon.CouponModel.prototype.GetData = function () {
        var model = {
            Id: this.Id(),
            Code: this.Code(),
            Discount: this.Discount(),
            IsSuccess: false
        };
        if (model.Code === "") {
            model.Message = "Код не может быть пустым";
        }
        else if (model.Discount === "") {
            model.Message = "Скидка не может быть пустой";
        }
        else if (model.Discount === 0) {
            model.Message = "Процент скидки не может быть равен нулю";
        } else
            model.IsSuccess = true;
        return model;
    }
})();