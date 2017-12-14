var Product = Product || {};

(function () {

    if (Product.ProductModel) {
        console.error("Product.ProductModel уже был создан");
        return;
    }

    /**
     * Класс для описания модели товара
     * @param {     
     * } theParams 
     * @returns {} 
     */
    Product.ProductModel = function (theParams) {
        theParams = theParams || {};

        this.Id = ko.observable(theParams.Id || "");
        this.BrandId = ko.observable(theParams.BrandId || "");
        this.Name = ko.observable(theParams.Name || "");
        this.KeyUrl = ko.observable(theParams.KeyUrl || "");
        this.Description = ko.observable(theParams.Description || "");

        this.Price = ko.observable(theParams.Price || "");
        this.Discount = ko.observable(theParams.Discount || "");

        this.SeoKeywords = ko.observable(theParams.SeoKeywords || "");
        this.SeoDescription = ko.observable(theParams.SeoDescription || "");

        this.IsInStock = ko.observable(theParams.IsInStock || false);
        this.IsActive = ko.observable(theParams.IsActive || false);

        this.CategoriesId = ko.observableArray(theParams.CategoriesId || []);
        this.TagsId = ko.observableArray(theParams.TagsId || []);

        this.PhotoUrl = ko.observable(theParams.PhotoUrl || "");
        return this;
    };

    /**
     * Определяем конструктор
     */
    Product.ProductModel.prototype.constructor = Product.ProductModel;

    Product.ProductModel.prototype.log = function (text) {
        console.log("Ошибка в классе Product.ProductModel: " + text);
    };

    Product.ProductModel.prototype.GetData = function () {
        return {
            Id: this.Id(),
            BrandId: this.BrandId(),
            Name: this.Name(),
            KeyUrl: this.KeyUrl(),
            Description: this.Description(),
            Price: this.Price(),
            Discount: this.Discount(),

            SeoKeywords: this.SeoKeywords(),
            SeoDescription:this.SeoDescription(),
            
            IsInStock: this.IsInStock(),
            IsActive: this.IsActive(),
            CategoriesId: this.CategoriesId(),
            TagsId: this.TagsId()
        }
    }
})();