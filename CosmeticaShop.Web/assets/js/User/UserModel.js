var User = User || {};

(function () {

    User.UserModel = function (theParams) {
        theParams = theParams || {};
        this.Id = ko.observable(theParams.Id || "");
        this.Email = ko.observable(theParams.Email || "");
        this.FirstName = ko.observable(theParams.FirstName || "");
        this.LastName = ko.observable(theParams.LastName || "");
        this.Password = ko.observable(theParams.Password || "");
        this.DateBirth = ko.observable(theParams.DateBirth || "");
        this.City = ko.observable(theParams.City || "");
        this.Country = ko.observable(theParams.Country || "");
        this.Address = ko.observable(theParams.Address || "");
        this.Phone = ko.observable(theParams.Phone || "");

        this.DateDay = ko.observable(theParams.DateDay || 0);
        this.DateMonth = ko.observable(theParams.DateMonth || 0);
        this.DateYear = ko.observable(theParams.DateYear || 0);
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
        if (this.DateYear()  !== "" && this.DateMonth() !== "" && this.DateDay() !== "") {
            this.DateBirth(new Date(this.DateYear(), this.DateMonth()-1, this.DateDay()).toISOString());
        }
        return {
            Id: this.Id(),
            Email: this.Email(),
            FirstName: this.FirstName(),
            LastName: this.LastName(),
            Password: this.Password(),
            DateBirth: this.DateBirth(),
            City: this.City(),
            Country: this.Country(),
            Address: this.Address(),
            Phone: this.Phone()
        }
    }

})();

