var Authorization = Authorization || {};

(function () {

    Authorization.PasswordRecoveryMode = function (theParams) {
        theParams = theParams || {};
        this.UrlRecovery = theParams.UrlRecovery;
        this.UrlChangePassword = theParams.UrlChangePassword;
        this.UserModel = new User.UserModel(theParams.Model || {});
        this.ErrorMessage = ko.observable("");
        this.Step = ko.observable(1);
        if (theParams.Model && theParams.Model.token) {
            this.Token = theParams.Model.token;
            this.Step(3);
            this.UserModel.Email(decodeURIComponent(theParams.Model.email));
            $(theParams.IdModalRestorePassword).modal('show');
        }
        return this;
    };

    /**
    * Определяем конструктор
    */
    Authorization.PasswordRecoveryMode.prototype.constructor = Authorization.PasswordRecoveryMode;
    /**
    * Послать сообщение о востоновление пароля 
    */
    Authorization.PasswordRecoveryMode.prototype.Recovery = function () {
        var self = this;
        var model = this.UserModel.GetData();
        $.post(this.UrlRecovery, {
            Email: model.Email
        }).success(function (res) {
            if (res.IsSuccess) {
                self.Step(2);
            }
            else {
                self.ErrorMessage(res.Message);
            }
        });
    }
    /**
    * Сменить пароль
    */
    Authorization.PasswordRecoveryMode.prototype.ChangePassword = function () {
        var self = this;
        var model = this.UserModel.GetData();
        var isSuccess = this.Validation(model);
        if (isSuccess) {
            $.post(this.UrlChangePassword,
                {
                    Email: model.Email,
                    Password: model.Password,
                    token: this.Token
                }).success(function(res) {
                if (res.IsSuccess) {
                    self.Step(4);
                    history.pushState(null, null, "/" + "");
                } else {
                    self.ErrorMessage(res.Message);
                }
            });
        }
    }
    /**
* Валидация
*/
    Authorization.PasswordRecoveryMode.prototype.Validation = function (model) {
        var self = this;
        if (this.ConfrimPassword !== model.Password) {
            this.ErrorMessage("Parolele nu se potrivesc");
            return false;
        }
        return true;
    }


})();

