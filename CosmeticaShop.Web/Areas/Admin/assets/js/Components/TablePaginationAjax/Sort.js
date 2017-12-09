var Components = Components || {};

(function () {
    //Таблица с пагинацией подгружаемые данные через Ajax
    Components.Sort = function (params) {
        params = params || {}
        this.Sort = params.Sort|| {};
    };

    /**
    * Определяем конструктор
    */
    Components.Sort.prototype.constructor = Components.Sort;



    Components.Sort.prototype.getData = function (data) {
        this.trigger('SortGetData');
        if (typeof this.Sort === "function")
            data.Sort = this.Sort();
        else {
            data.Sort = this.Sort;
        }
    }
    MicroEvent.mixin(Components.Sort);
})();
