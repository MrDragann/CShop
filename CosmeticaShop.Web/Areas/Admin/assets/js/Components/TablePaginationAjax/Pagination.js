var Components = Components || {};
        (function () {
            //Таблица с пагинацией подгружаемые данные через Ajax
            Components.Pagination = function (params) {
                params = params || {};
                var self = this;
                this.Count = ko.observable(params.Count);
                //сколько записей отображать на странице
                this.pageSize = ko.observable([params.pageSize || 10]);
                //список элементов для вывода
                this.data = ko.observableArray(params.data);
                this.showPagesCounts = params.showPagesCounts || [5, 10, 20, 50];
                this.currentPage = ko.observable(params.currentPage || 1);

                //последний номер страницы
                this.maxPage = ko.computed(function () {
                    var div = self.Count() / self.pageSize()[0];
                    if (div > Math.floor(div))
                        return Math.ceil(div);
                    return Math.floor(div);
                }, this);
                //текстовые константы
                var localizeConst = {
                    back: "Назад",
                    next: "Вперед",
                    noItems: "Нет записей",
                    pageSize: "Показать"
                }
                $.extend(localizeConst, params.localizeConst);
                //текстовые константы
                this.localize = ko.observable(localizeConst);
                return this;
            };

            /**
            * Определяем конструктор
            */
            Components.Pagination.prototype.constructor = Components.Pagination;

            Components.Pagination.prototype.onChangePage = function (page) {
               this.currentPage(page);
                this.trigger('onChangePage');
            }

            Components.Pagination.prototype.ToPage = function(page) {
                this.onChangePage(page);
            }
            Components.Pagination.prototype.getData = function(data) {
                data.Skip = (this.currentPage()-1) * this.pageSize()[0];
                data.Take = this.pageSize()[0];
            }
            MicroEvent.mixin(Components.Pagination);

        })();
