var Authorization = Authorization || {};

(function () {

    Authorization.AuthorizationFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlLogin = theParams.UrlLogin;
        this.UserModel = new User.UserModel();
        this.ErrorMessage = ko.observable("");
        return this;
    };

    /**
    * Определяем конструктор
    */
    Authorization.AuthorizationFormModel.prototype.constructor = Authorization.AuthorizationFormModel;
    /**
    * Авторизоваться 
    */
    Authorization.AuthorizationFormModel.prototype.Login = function () {
        var self = this;
        var model = this.UserModel.GetData();
        var isSuccess = this.Validation(model);
        if (isSuccess) {
            $.post(this.UrlLogin, {
                Email: model.Email,
                Password: model.Password
            }).success(function (res) {
                if (res.IsSuccess) {
                    location.href = res.Value;
                }
                else {
                    self.ErrorMessage(res.Message);
                }
            });
        }
    }

    /**
    * Валидация
    */
    Authorization.AuthorizationFormModel.prototype.Validation = function (model) {
        var self = this;
        if (model.Password === "" || model.Email === "") {
            this.ErrorMessage("Completați toate câmpurile");
            return false;
        }
        return true;
    }


})();

