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
        this.Code = ko.observable(theParams.Code || "");
        this.KeyUrl = ko.observable(theParams.KeyUrl || "");
        this.Description = ko.observable(theParams.Description || "");

        this.Price = ko.observable(theParams.Price || 0);
        this.Discount = ko.observable(theParams.Discount || 0);

        this.SeoKeywords = ko.observable(theParams.SeoKeywords || "");
        this.SeoDescription = ko.observable(theParams.SeoDescription || "");

        this.IsRecommended = ko.observable(theParams.IsRecommended || false);
        this.IsInStock = ko.observable(theParams.IsInStock || false);
        this.IsActive = ko.observable(theParams.IsActive || false);
        
        this.CategoriesId = ko.observableArray(theParams.CategoriesId || []);
        this.TagsId = ko.observableArray(theParams.TagsId || []);
        this.SimilarProducts = ko.observableArray(theParams.SimilarProducts ? theParams.SimilarProducts.map(function (item) { return new Product.ProductModel(item) }) : []);

        this.PhotoUrl = ko.observable(theParams.PhotoUrl || "");
        this.Photos = ko.observableArray(theParams.Photos || []);

        this.IsSelected = ko.observable(theParams.IsSelected || false);
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
            Code: this.Code(),
            KeyUrl: this.KeyUrl(),
            Description: this.Description(),
            Price: this.Price(),
            Discount: this.Discount(),

            SeoKeywords: this.SeoKeywords(),
            SeoDescription:this.SeoDescription(),
            
            IsRecommended: this.IsRecommended(),
            IsInStock: this.IsInStock(),
            IsActive: this.IsActive(),
            CategoriesId: this.CategoriesId(),
            TagsId: this.TagsId()
        }
    }
})();