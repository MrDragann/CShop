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
        //this.SelectedProducts = ko.observableArray([]);
        this.Filter = new Product.ProductModel();
        this.ModalSelector = theParams.ModalSelector;
        
        this.Table = new Components.Table({ Url: theParams.UrlLoadItems, ModalClass: Product.ProductModel, Filter: this.Filter });

        this.Filter.Name.subscribe(function (item) {
            self.Table.Refresh();
        });

        this.Filter.Code.subscribe(function (item) {
            self.Table.Refresh();
        });
        
        return this;
    };

    /**
     * Определяем конструктор
     */
    Product.ProductSelectFilter.prototype.constructor = Product.ProductSelectFilter;
    
    Product.ProductSelectFilter.prototype.SelectProducts = function (data) {
        var self = this;
        var selectedProducts = self.Table.Items().filter(function(item) {
            return item.IsSelected();
        });
        selectedProducts.forEach(function(item) {
            var existSimilar = self.SimilarProducts().find(function(similar) { return similar.Id() === item.Id() });
            if (!existSimilar) {
                self.SimilarProducts.push(item);
            }
        });
    }


})();