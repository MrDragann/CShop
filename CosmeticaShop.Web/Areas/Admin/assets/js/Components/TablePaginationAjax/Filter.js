var Components = Components || {};

(function () {
    //Таблица с пагинацией подгружаемые данные через Ajax
    Components.Filter = function (params) {
        params = params || {};
        this.Filter = params.Filter;
        this.IsSaveFilter = params.IsSaveFilter || false;
    };

    /**
    * Определяем конструктор
    */
    Components.Filter.prototype.constructor = Components.Filter;

    /**
     * Собирает данные с фильтра
     * @returns {} 
     */
    Components.Filter.prototype.getData = function (data) {
        this.trigger('FilterGetData');
        if (typeof this.Filter === "function")
            data.Filter = this.Filter();
        else {
            var model = {};
            for (key in this.Filter) {
                if (this.Filter.hasOwnProperty(key))
                    if (typeof this.Filter[key] === 'function') {
                        model[key] = this.Filter[key]();
                        if (this.IsSaveFilter)
                            $.cookie(key, model[key]);
                    }
            }
            data.Filter = model;
        }
    };
    MicroEvent.mixin(Components.Filter);
})();