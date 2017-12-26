var Authorization = Authorization || {};

(function () {

    Authorization.RegisterFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlRegister = theParams.UrlRegister;
        this.UrlGetAllCities = theParams.UrlGetAllCities;
        this.UserModel = new User.UserModel();
        this.DateDay = ko.observable();
        this.DateMonth = ko.observable();
        this.DateYear = ko.observable();
        this.ConfrimPassword = ko.observable();
        this.ErrorMessage = ko.observable("");
        this.IsSuccessRegister = ko.observable(false);
        this.IsPrivacyPolicy = ko.observable(false);
        this.Cities = ko.observableArray([]);
        this.GetAllCities();
        return this;
    };

    /**
    * Определяем конструктор
    */
    Authorization.RegisterFormModel.prototype.constructor = Authorization.RegisterFormModel;
    /**
    * Зарегистрироваться 
    */
    Authorization.RegisterFormModel.prototype.Register = function () {
        var self = this;
        var model = this.UserModel.GetData();
        var isSuccess = this.Validation(model);
        var response = this.RetrieveDate();
        if (isSuccess && response.IsSuccess) {
            model.DateBirth = response.Value;
            $.post(this.UrlRegister, model).success(function (res) {
                if (res.IsSuccess) {
                    self.IsSuccessRegister(true);
                    self.ErrorMessage("");
                }
                else {
                    self.ErrorMessage(res.Message);
                }
            });
        }
    }
    /**
    * Получить список городов 
    */
    Authorization.RegisterFormModel.prototype.GetAllCities = function () {
        var self = this;
        $.post(this.UrlGetAllCities).success(function (res) {
            self.Cities(res);
        });
    }

    /**
    * Валидация
    */
    Authorization.RegisterFormModel.prototype.Validation = function (model) {
        var self = this;
        if (!this.IsPrivacyPolicy()) {
            this.ErrorMessage("Este necesar să fie de acord cu politica de securitate");
            return false;
        }
        if (model.Password === "" || model.Name === "" || model.LastName === "" || model.Email === "") {
            this.ErrorMessage("Completați toate câmpurile");
            return false;
        }
        if (this.ConfrimPassword() !== model.Password) {
            this.ErrorMessage("Parolele nu se potrivesc");
            return false;
        }
        return true;
    }

    /**
    * Получить дату из 3 полей
    */
    Authorization.RegisterFormModel.prototype.RetrieveDate = function () {
        var date = new Date(this.DateYear(), this.DateMonth(), this.DateDay());
        if (date) {
            return { IsSuccess: true, Value: date.toISOString() };
        }
        this.ErrorMessage("Data nevalidă");
        return new { IsSuccess: false };
    }
})();

