var Category = Category || {};

/*********************** Category.CategoryFormModel **********************************************/
(function () {

    if (Category.CategoryFormModel) {
        console.error('Category.CategoryFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы с таблицей модели категорий
     */
    Category.CategoryFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlRefresh = theParams.UrlRefresh;
        this.UrlUpPriority = theParams.UrlUpPriority;
        this.UrlDownPriority = theParams.UrlDownPriority;
        this.Categories = ko.observableArray(theParams.Categories ? theParams.Categories.map(function (item) { return new Category.CategoryModel(item) }) : []);

        this.IsLoading = ko.observable(false);
        this.IsNotFind = ko.observable(false);
        return this;
    };

    /**
     * Определяем конструктор
     */
    Category.CategoryFormModel.prototype.constructor = Category.CategoryFormModel;

    Category.CategoryFormModel.prototype.LoadData = function () {
        var self = this;
        self.IsLoading(true);

        $.post(self.UrlRefresh)
            .success(function (res) {
                if (res.length) {
                    self.Categories(res.map(function (item) { return new Category.CategoryModel(item) }));
                    self.IsLoading(false);
                }
            })
            .fail(function (res) {
                bootbox.alert('При загрузке категорий произошла ошибка');
            });
    }

    Category.CategoryFormModel.prototype.UpCategoryPriority = function (data) {
        var self = this;
        self.IsLoading(true);
        $.post(self.UrlUpPriority, {
                parentId: data.ParentId(), categoryId: data.Id()
            })
            .success(function (res) {
                if (res.IsSuccess) {
                    self.LoadData();
                    self.IsLoading(false);
                }
            })
            .fail(function (res) {
                bootbox.alert('При загрузке категорий произошла ошибка');
            });
    }

    Category.CategoryFormModel.prototype.DownCategoryPriority = function (data) {
        var self = this;
        self.IsLoading(true);
        $.post(self.UrlDownPriority, {
                parentId: data.ParentId(), categoryId: data.Id()
            })
            .success(function (res) {
                if (res.IsSuccess) {
                    self.LoadData();
                    self.IsLoading(false);
                }
            })
            .fail(function (res) {
                bootbox.alert('При загрузке категорий произошла ошибка');
            });
    }

    Category.CategoryFormModel.prototype.CollapseChildrens = function (data) {
        var self = this;

        this.IsClosed(!this.IsClosed());
    }

})();