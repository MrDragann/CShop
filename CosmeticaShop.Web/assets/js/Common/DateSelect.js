


var GetDateDaySelect = function() {
    var dateArray = [];
    for (var i = 1; i <= 31; i++) {
        dateArray.push(i);
    }
    return dateArray;
}
var GetDateMonthSelect = function () {
    var dateArray = [];
    for (var i = 1; i <= 12; i++) {
        dateArray.push(i);
    }
    return dateArray;
}
var GetDateYearSelect = function () {
    var dateArray = [];
    for (var i = 2018; i >= 1900; i--) {
        dateArray.push(i);
    }
    return dateArray;
}
var DateDaySelect = ko.observableArray(GetDateDaySelect());
var DateMonthSelect = ko.observableArray(GetDateMonthSelect());
var DateYearSelect = ko.observableArray(GetDateYearSelect());

