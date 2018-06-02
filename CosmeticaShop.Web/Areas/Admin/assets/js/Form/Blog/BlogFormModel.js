var Blog = Blog || {};

/*********************** Blog.BlogFormModel **********************************************/
(function () {

    if (Blog.BlogFormModel) {
        console.error('Blog.BlogFormModel уже был создан');
        return;
    }

    /**
     * Класс для работы с таблицей модели блога
     */
    Blog.BlogFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlDelete = theParams.UrlDelete;

        this.Filter = new Base.BaseFilterModel();
        this.Table = new Components.Table({ Url: theParams.UrlLoadItems, ModalClass: Blog.BlogModel, Filter: this.Filter });
        this.TableRefresh = this.Table.Refresh.bind(this.Table);

        return this;
    };

    /**
     * Определяем конструктор
     */
    Blog.BlogFormModel.prototype.constructor = Blog.BlogFormModel;

    Blog.BlogFormModel.prototype.DeleteBlog = function (data) {
        var self = this;

        bootbox.confirm("Вы действительно хотите удалить пост \"" + data.Title() + "\"?",
            function(e) {
                if (e) {
                    $.post(self.UrlDelete, { blogId: data.Id() })
                        .success(function(res) {
                            if (res.IsSuccess) {
                                bootbox.alert(res.Message);
                                self.TableRefresh();
                            }
                            else if (res.Status === Enums.EnumResponseStatus.Exception)
                                console.log("ex:", res.Message);
                            else 
                                bootbox.alert(res.Message);
                        });
                }
            });
    }

})();