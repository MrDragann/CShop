ko.bindingHandlers.ckeditor = {
    init: function (element, valueAccessor, allBindings) {
        var txtBoxID = $(element).attr("id");

        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            if (CKEDITOR.instances[txtBoxID]) {
                CKEDITOR.remove(CKEDITOR.instances[txtBoxID]);
            }
        });

        $(element).ckeditor();

        CKEDITOR.instances[txtBoxID].focusManager.blur = function () {
            var observable = valueAccessor();
            observable($(element).val());
        };
    },
    update: function (element, valueAccessor, allBindingsAccessor) {
        var val = ko.utils.unwrapObservable(valueAccessor());
        $(element).val(val);
    }
}