var Product = Product || {};

/*********************** Product.ProductSelectFilter **********************************************/
(function () {

    /**
     * Класс для выбора товаров
     */
    Product.ProductSelectFilter = function (theParams) {
        var self = this;
        theParams = theParams || {};

        this.SimilarProducts = theParams.SimilarProducts || [];
        this.SelectedProducts = ko.observableArray([]);
        this.SelectedProductsId = ko.computed(function () {
            return self.SelectedProducts().filter(x => x.IsSelected()).map(x => x.Id());
        });
        this.Filter = new Product.ProductModel();
        this.ModalSelector = theParams.ModalSelector;

        this.Table = new Components.Table({ Url: theParams.UrlLoadItems, ModalClass: Product.ProductModel, Filter: this.Filter });

        this.Filter.Name.subscribe(function (item) {
            self.Table.Refresh();
        });

        this.Filter.Code.subscribe(function (item) {
            self.Table.Refresh();
        });
        this.Table.IsLoading.subscribe(function (item) {
            if (item === false) {
                self.Table.Items().forEach(x => {
                    if (self.SelectedProductsId().includes(x.Id())) {
                        x.IsSelected(true);
                    }
                });
            }
        });
        return this;
    };

    /**
     * Определяем конструктор
     */
    Product.ProductSelectFilter.prototype.constructor = Product.ProductSelectFilter;

    Product.ProductSelectFilter.prototype.SelectProduct = function (data) {
        var self = this;
        data.IsSelected(!data.IsSelected());
        // если товар выбран, то добавляем его в список выбранных
        if (data.IsSelected()) {
            self.SelectedProducts.push(data);
        } else {
            self.SelectedProducts.remove(data);
        }
    }

    Product.ProductSelectFilter.prototype.SelectProducts = function (data) {
        var self = this;
        self.SelectedProducts().forEach(function (item) {
            self.SimilarProducts.push(item);
        });
        self.SelectedProducts([]);
    }


})();