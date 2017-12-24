var Order = Order || {};

/*********************** Order.OrderFormModel **********************************************/
(function () {

    if (Order.OrderFormModel) {
        console.error('Order.OrderFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы с таблицей модели брендов
     */
    Order.OrderFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlDelete = theParams.UrlDelete;

        this.Filter = new Base.BaseFilterModel();
        this.Table = new Components.Table({ Url: theParams.UrlLoadItems, ModalClass: Order.OrderHeaderModel, Filter: this.Filter });
        this.TableRefresh = this.Table.Refresh.bind(this.Table);

        return this;
    };

    /**
     * Определяем конструктор
     */
    Order.OrderFormModel.prototype.constructor = Order.OrderFormModel;
    
})();