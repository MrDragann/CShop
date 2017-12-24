var Order = Order || {};

/*********************** Order.OrderEditFormModel **********************************************/
(function () {

    if (Order.OrderEditFormModel) {
        console.error('Order.OrderEditFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы с таблицей заказа
     */
    Order.OrderEditFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlChangeStatus = theParams.UrlChangeStatus;
        this.UrlMoveToEdit = theParams.UrlMoveToEdit;

        this.OrderStatuses = theParams.OrderStatuses || [];
        this.Order = new Order.OrderHeaderModel(theParams.Model);

        return this;
    };

    /**
     * Определяем конструктор
     */
    Order.OrderEditFormModel.prototype.constructor = Order.OrderEditFormModel;

    Order.OrderEditFormModel.prototype.ChangeStatus = function () {
        var self = this;
        bootbox.confirm("Вы действительно хотите изменить статус заказу?",
            function(e) {
                $.post(self.UrlChangeStatus, { orderId: self.Order.Id(), status: self.Order.Status() })
                    .success(function (res) {
                        if (res.IsSuccess) {
                            bootbox.alert(res.Message, e => location.reload());
                        } else if (res.Status === Enums.EnumResponseStatus.Exception)
                            console.log("ex:", res.Message);
                        else
                            bootbox.alert(res.Message);
                    })
                    .fail(function (res) {
                        console.log('res:', res);
                    });
            });
    }

})();