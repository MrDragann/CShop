var Products = Products || {};

(function () {

    if (Products.ProductFilter) {
        console.error("Products.ProductFilter уже был создан");
        return;
    }

    /**
     * Класс для описания фильтра товаров
     * @param { } theParams 
     * @returns {} 
     */
    Products.ProductFilter = function (theParams) {
        theParams = theParams || {};
        this.BrandiesId = ko.observableArray(theParams.BrandiesId ? theParams.BrandiesId.map(function (item) { return item.toString() }) : []);  
        this.CategoriesId = ko.observableArray(theParams.CategoriesId ? theParams.CategoriesId.map(function (item) { return item.toString() }) : []);   
        this.Page = ko.observable(theParams.Page || "");    
        this.TagsId = ko.observable(theParams.TagsId ? theParams.TagsId.map(function (item) { return item.toString() }) : []);
        this.Search = ko.observable(theParams.Search || "");
        this.Discount = ko.observable(theParams.Discount || false);
       
        return this;
    };

    /**
     * Определяем конструктор
     */
    Products.ProductFilter.prototype.constructor = Products.ProductFilter;

    Products.ProductFilter.prototype.GetData = function () {
        var model =  {
            search: this.Search(),
            BrandiesId: this.BrandiesId(),
            Page: this.Page(),
            CategoriesId: this.CategoriesId(),
            TagsId:this.TagsId()
        }
        if (this.Discount())
            model.Discount = this.Discount();
        return model;
    }


    Products.ProductFilter.prototype.log = function (text) {
        console.log("Ошибка в классе Products.ProductFilterel: " + text);
    };
})();