var Cabinet = Cabinet || {};

(function () {

    Cabinet.PersonalFormModel = function (theParams) {
        theParams = theParams || {};
        this.UrlEdit = theParams.UrlEdit;
        this.UrlGetAllCities = theParams.UrlGetAllCities;
        this.UserModel = new User.UserModel(theParams.Model || {});
        this.ErrorMessage = ko.observable("");
        this.ConfrimPassword = ko.observable();
        this.Cities = ko.observableArray(theParams.Cities || []);
        return this;
    };

    /**
    * Определяем конструктор
    */
    Cabinet.PersonalFormModel.prototype.constructor = Cabinet.PersonalFormModel;

    /**
    * Изменить личные данные 
    */
    Cabinet.PersonalFormModel.prototype.EditData = function () {
        var self = this;
        var model = this.UserModel.GetData();
        var isSuccess = this.Validation(model);
        if (isSuccess) {
            $.post(this.UrlEdit, model).success(function (res) {
                if (res.IsSuccess) {
                    location.reload();
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
    Cabinet.PersonalFormModel.prototype.Validation = function (model) {
        var self = this;
        if (model.Email === "" || model.FirstName === "" || model.LastName === "" || model.City === "" || model.Country === "" || model.Phone === "" || model.Address === "") {
            this.ErrorMessage("Completați toate câmpurile");
            return false;
        }
        if (model.Password !== "") {
            if (model.Password !== this.ConfrimPassword()) {
                this.ErrorMessage("Parolele nu se potrivesc");
                return false;                
            }
          
            bootbox.confirm("Chiar vrei să schimbi parola?", function(result) {
                if (result) {
                    $.post(self.UrlEdit, model).success(function (res) {
                        if (res.IsSuccess) {
                            location.reload();
                        }
                        else {
                            self.ErrorMessage(res.Message);
                        }
                    });
                }
                   
                return false;
            });
            return false;
        }
        return true;
    }


})();
