var Contacts = Contacts || {};

    (function () {

        Contacts.ViewModel = function (params) {
            params = params || {};
            this.ConstStatePage = {
                Contacte: "Contacte",
                DespreNoi: "DespreNoi",
                Informație: "Informație",
                VânzărAangro: "VânzărAangro",
                Rechizite: "Rechizitem",
                LivrareReturnare: "LivrareReturnare",
                Politica: "Politica",
                OfertaPublică: "OfertaPublică"
            };
            this.StatePage = ko.observable(this.ConstStatePage.Contacte);
            return this;
        };

        /**
        * Определяем конструктор
        */
        Contacts.ViewModel.prototype.constructor = Contacts.ViewModel;


    })();

