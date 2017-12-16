var Product = Product || {};

(function () {

    Product.ProductViewModel = function (theParams) {        
        theParams = theParams || {};
        var self = this;
        this.UrlGetProducts = theParams.UrlGetProducts;
        this.Products = ko.observableArray(theParams.Products
            ? theParams.Products.map(function (item) {
                return new Product.ProductModel(item);
            })
            : []);
        this.Filter = new Products.ProductFilter(theParams.Filter);
        this.Filter.BrandiesId.subscribe(function() {
            self.UpdateProducts();
        });
        this.Filter.CategoriesId.subscribe(function () {
            self.UpdateProducts();
        });
        return this;
    };

    /**
    * Определяем конструктор
    */
    Product.ProductViewModel.prototype.constructor = Product.ProductViewModel;


    /**
    * Обновить товары
    */
    Product.ProductViewModel.prototype.UpdateProducts = function (page) {
        var self = this;
        var model = this.Filter.GetData();
        //if (page)
        //    this.Filter.Page(page);
        var filterParams = this.GetLocation();
        location.href = this.UrlGetProducts + filterParams;
    
    }

    /**
    * Получить параметры для фильтрации
    */
    Product.ProductViewModel.prototype.GetLocation = function () {
        var self = this;
        var paramsLocations = [];
        var model = this.Filter.GetData();
        for (item in model) {
            if (model.hasOwnProperty(item)) {
                if (model[item] !== "" && model[item] !== null) {
                    if (Array.isArray(model[item]) && model[item].length === 0) {
                        continue;
                    }
                    if (Array.isArray(model[item]) && model[item].length > 0) {
                        for (keyArr in model[item]) {
                            paramsLocations.push("&" + item + "=" + model[item][keyArr]);
                        }
                        continue;
                    }
                    paramsLocations.push("&" + item + "=" + model[item]);
                }
            }
        }
        var newString = paramsLocations.join("");
        newString = newString.slice(1, newString.length);
        return "?" + newString;
    }

})();
