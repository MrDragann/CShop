var User = User || {};

(function () {

    User.UserModel = function (theParams) {
        theParams = theParams || {};
        this.Id = ko.observable(theParams.Id || "");
        this.Email = ko.observable(theParams.Email || "");
        this.Name = ko.observable(theParams.Name || "");
        this.LastName = ko.observable(theParams.LastName || "");
        this.Password = ko.observable(theParams.Password || "");
        this.DateBirth = ko.observable(theParams.DateBirth || "");
        this.Town = ko.observable(theParams.Town || "");
        return this;
    };

    /**
    * Определяем конструктор
    */
    User.UserModel.prototype.constructor = User.UserModel;

    /**
    * Получить данные модели
    */
    User.UserModel.prototype.GetData = function () {
        return {
            Id: this.Id(),
            Email: this.Email(),
            Name: this.Name(),
            LastName: this.LastName(),
            Password: this.Password(),
            DateBirth: this.DateBirth(),
            Town:this.Town()
        }
    }

})();

