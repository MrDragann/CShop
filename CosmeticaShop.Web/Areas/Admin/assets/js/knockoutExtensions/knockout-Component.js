var ComponentsArray = [];
        ko.bindingHandlers.Component = {
            init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
                var tag = $(element)[0].tagName;
                var component = ComponentsArray.find(function(item) {
                    return item.Name.toLowerCase() === tag.toLowerCase();
                });
                if (component.Url) {
                    $.ajax({
                        type: "GET",
                        url: component.Url,
                        contentType: false,
                        processData: false,
                        success: function(data) {
                            var html = $(data);
                            $(element).replaceWith(html);
                            ko.applyBindings(valueAccessor(), html.get()[0]);
                        },
                        error: function(xhr) {
                            console.log(xhr.responseText);
                        }
                    });
                }
                if (component.UrlStyle) {
                    component.UrlStyle.map(function(item) {
                        var link = document.createElement('link');
                        document.getElementsByTagName('head')[0].appendChild(link);
                        link.setAttribute('type', 'text/css');
                        link.setAttribute('rel', 'stylesheet');
                        link.setAttribute('href', item);
                    });
                }
            }
        };
        ko.components.custom = ko.components.custom || {};
        ko.components.custom.register = function (name, url, urlStyle) {
            var isRegister = ComponentsArray.some(function(item) {
                return item.Name === name;
            });
            if (isRegister) {
                console.error(name," уже был зарегистрирован");
                return;
            }
            ComponentsArray.push({ Name: name, Url: url, UrlStyle: urlStyle});
        }