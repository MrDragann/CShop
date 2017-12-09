var Components = Components || {};

        (function () {
            //Таблица с пагинацией подгружаемые данные через Ajax
            Components.Table = function (params) {
                //Значение с сервера
                this.Value = ko.observable(params.Value || "");
                this.Url = params.Url || "";
                this.UrlData = params.UrlData;
                this.Items = ko.observableArray(params.Items||[]);
                this.Pagination = new Components.Pagination();
                this.Filter = new Components.Filter({ Filter: params.Filter, IsSaveFilter: params.IsSaveFilter });
                this.Sort = new Components.Sort({Sort:params.Sort});
                this.ModalClass = params.ModalClass;             
                this.Pagination.bind("onChangePage", this.onChangePage.bind(this));
                this.Refresh();
                return this;
            };

            /**
            * Определяем конструктор
            */
            Components.Table.prototype.constructor = Components.Table;
            /**
             * Обновление таблицы
             * @returns {} 
             */
            Components.Table.prototype.Refresh = function () {
	            var data = this.getData();
                $.ajax({
                    type: "POST",
                    url: this.Url,                    
                    dataType: 'json',
                    data: data,
                    success: this.RefreshSuccess.bind(this),
                    error: this.RefreshError
                });
            };
            Components.Table.prototype.RefreshError = function (param) {

            }
            /**
             * 
             * @param {Массив записей} result 
             * @returns {} 
             */
            Components.Table.prototype.RefreshSuccess = function (result) {
                var self = this;
                this.Items(result.Items.map(function (item) {
                    item.UrlData = self.UrlData;
                    return new self.ModalClass(item);
                }));
                this.Pagination.Count(result.Count);
                
            }
            /**
             * Собирает данные с пагинации, фильтра и сортировки
             * @returns {} 
             */
            Components.Table.prototype.getData = function () {
                var data = {};
                this.Pagination.getData(data);
                this.Filter.getData(data);
                this.Sort.getData(data);
                return data;
            }
            Components.Table.prototype.onChangePage = function() {
               this.Refresh();
            }


        })();

