ko.bindingHandlers.fileInput = {
    init: function (element, valueAccessor, allBindings) {
        var formData = valueAccessor(), files = element.files;

        if (files.length > 0) {
            files.forEach(function (file, i) {
                formData.append("PhotoFile" + files.length === 1 ? "" : i, file);
            });
        }

    }
}